using Arc.Compiler.PackageGenerator.Helpers;
using Arc.Compiler.PackageGenerator.Models.Builtin;
using Arc.Compiler.PackageGenerator.Models.Descriptors;
using Arc.Compiler.PackageGenerator.Models.Generation;
using Arc.Compiler.PackageGenerator.Models.Intermediate;
using Arc.Compiler.PackageGenerator.Models.Logging;
using Arc.Compiler.PackageGenerator.Models.PrimitiveInstructions;
using Arc.Compiler.PackageGenerator.Models.Scope;
using Arc.Compiler.SyntaxAnalyzer.Models.Components;
using Arc.Compiler.SyntaxAnalyzer.Models.Components.CallChain;
using Arc.Compiler.SyntaxAnalyzer.Models.Function;
using Microsoft.Extensions.Logging;

namespace Arc.Compiler.PackageGenerator.Generators.Instructions
{
    internal class ArcCallChainGenerator
    {
        public static ArcPartialGenerationResult Generate(ArcGenerationSource source, ArcCallChain callChain)
        {
            var result = new ArcPartialGenerationResult();

            ArcDataDeclarationDescriptor? lastTermTypeDecl;
            
            // Unable to resolve, resolve as a regular call chain

            int skipCount = 0;
            // First term maybe be variant, so handle it separately
            if (callChain.ConstructorCall == null)
            {
                // Handle as enum member
                var enumResult = TryResolveAsEnum(source, callChain);
                if (enumResult != null)
                {
                    return enumResult;
                }
                
                // Handle as normal call chain
                
                var (firstTermResult, d) = GenerateFirstTerm(callChain.Terms.First(), source);
                result.Append(firstTermResult);
                lastTermTypeDecl = d;
                skipCount = 1;
            }
            else
            {
                var (firstTermResult, d) = GenerateFirstTermConstructor(callChain.ConstructorCall, source);

                result.Append(firstTermResult);
                lastTermTypeDecl = d;
            }

            if (lastTermTypeDecl == null)
            {
                result.Logs.Add(new ArcSourceLocatableLog(LogLevel.Error, 0, "Cannot determine the data type of the first term", source.Name, callChain.Terms.First().Context));
                return result;
            }

            var remainingTermsResult = GenerateRemainingTerm(callChain.Terms.Skip(skipCount), lastTermTypeDecl, source);
            result.Append(remainingTermsResult);

            return result;
        }

        private static ArcPartialGenerationResult? TryResolveAsEnum(ArcGenerationSource source, ArcCallChain callChain)
        {
            if (callChain.Terms.Count() != 2)
            {
                return null;
            }
            
            var firstTerm = callChain.Terms.First();
            if (firstTerm.Type != ArcCallChainTermType.Identifier)
            {
                return null;
            }
            
            var enumTypeNode = ArcDataTypeHelper.GetComplexType(source, firstTerm.Identifier!) as ArcScopeTreeDataTypeNode;
            if (enumTypeNode == null)
            {
                return null;
            }

            if (enumTypeNode.ArcDataTypeType != ArcDataTypeType.Enum)
            {
                return null;
            }

            var enumType = enumTypeNode.EnumType!;
            
            var secondTerm = callChain.Terms.Last();
            if (secondTerm is not { Type: ArcCallChainTermType.Identifier })
            {
                return null;
            }
            
            var enumMember = enumType.Members.FirstOrDefault(m => m.Name.Equals(secondTerm.Identifier!.Name));
            if (enumMember == null)
            {
                return null;
            }
            
            var result = new ArcPartialGenerationResult();
            result.Append(new ArcLoadDataToStackInstruction(
                new(
                    ArcDataSourceType.Symbol, 
                    enumMember.Id, false)
                ).Encode(source)
            );

            return result;
        }

        private static (ArcPartialGenerationResult, ArcDataDeclarationDescriptor?) GenerateFirstTerm(ArcCallChainTerm firstTerm, ArcGenerationSource source)
        {
            var result = new ArcPartialGenerationResult();
            ArcDataDeclarationDescriptor termTypeDecl;
            if (firstTerm.Type == ArcCallChainTermType.Identifier)
            {
                var identifier = firstTerm.Identifier!;
                var slot = source.LocalDataSlots.First(s => s.DeclarationDescriptor.SyntaxTree.Identifier.Name == identifier.Name);
                termTypeDecl = slot.DeclarationDescriptor;
                var locator = new ArcStackDataOperationDescriptor(ArcDataSourceType.DataSlot, slot.SlotId, false);

                result.Append(new ArcLoadDataToStackInstruction(locator).Encode(source));
            }
            else
            {
                var call = firstTerm.FunctionCall!;
                result.Append(ArcFunctionCallGenerator.Generate(source, call, false));

                var (targetFuncId, logs) = ArcFunctionHelper.GetFunctionId(source, call);
                if (logs.Any())
                {
                    result.Logs.AddRange(logs);
                    return (result, null);
                }

                var function = source.CurrentNode.Root.GetSpecificChild<ArcScopeTreeFunctionNodeBase>(f => f.Id == targetFuncId, true);

                if (function == null)
                {
                    result.Logs.Add(new ArcSourceLocatableLog(LogLevel.Error, 0, "Function not found", source.Name, call.Context));
                    return (result, null);
                }

                termTypeDecl = function.ReturnValueType;
            }

            foreach (var expr in firstTerm.Indices)
            {
                result.Append(ArcExpressionEvaluationGenerator.GenerateEvaluationCommand(source, expr));

                var arrayOperationDesc = new ArcStackDataOperationDescriptor(ArcDataSourceType.ArrayElement, 0, true);
                result.Append(new ArcLoadDataToStackInstruction(arrayOperationDesc).Encode(source));
            }

            return (result, termTypeDecl);
        }

        private static (ArcPartialGenerationResult, ArcDataDeclarationDescriptor?) GenerateFirstTermConstructor(ArcConstructorCall constructor, ArcGenerationSource source)
        {
            var result = new ArcPartialGenerationResult();

            var dataTypeProxy = ArcDataTypeHelper.GetComplexType(source, constructor.DataType);
            if (dataTypeProxy == null)
            {
                result.Logs.Add(new ArcSourceLocatableLog(LogLevel.Error, 0, "Data type not found", source.Name, constructor.Context));
                return (result, null);
            }

            var dataTypeNode = ArcDataTypeHelper.GetDataTypeNode(source, dataTypeProxy.ResolvedType)!;

            // Constructor is a function that uses `self`
            foreach (var param in constructor.Parameters)
            {
                result.Append(ArcExpressionEvaluationGenerator.GenerateEvaluationCommand(source, param));
            }

            // Currently we determine constructor by the number of parameters
            // TODO: Implement a more robust way to determine the constructor
            var dataTypeGroup = dataTypeNode.ComplexTypeGroup!;
            var constructorFunction = dataTypeGroup.LifecycleFunctions
                .Where(f => f.SyntaxTree.LifecycleStage == ArcGroupLifecycleStageType.Construction)
                .First(c => c.Parameters.Count() == constructor.Parameters.Count() + 1);
            
            // Constructor function will be called automatically
            result.Append(new ArcNewObjectInstruction(dataTypeNode, constructor.SpecializedGenericTypes, constructorFunction).Encode(source));
            
            var dataDeclDesc = new ArcDataDeclarationDescriptor
            {
                Type = dataTypeNode.DataType,
                AllowNone = false,
                Dimension = 0,
                SyntaxTree = null!
            };

            return (result, dataDeclDesc);
        }

        private static ArcPartialGenerationResult GenerateRemainingTerm(IEnumerable<ArcCallChainTerm> terms, ArcDataDeclarationDescriptor lastTermTypeDecl, ArcGenerationSource source)
        {
            var result = new ArcPartialGenerationResult();
            foreach (var term in terms)
            {
                if (lastTermTypeDecl.Type is ArcBaseType)
                {
                    result.Logs.Add(new ArcSourceLocatableLog(LogLevel.Error, 0, "Cannot access fields of a base type", source.Name, term.Context));
                }

                var dataType = (lastTermTypeDecl.Type as ArcComplexType)!;
                var group = source.CurrentNode.Root
                    .GetSpecificChild<ArcScopeTreeDataTypeNode>(g => g.ComplexTypeGroup?.Id == dataType.GroupId, true)?
                    .ComplexTypeGroup!;

                switch (term.Type)
                {
                    case ArcCallChainTermType.FunctionCall:
                        // Handle the function call of this term
                        var fn = ArcGroupHelper.ResolveSelfFunction(group, term.FunctionCall!, source);
                        if (fn == null)
                        {
                            result.Logs.Add(new ArcSourceLocatableLog(LogLevel.Error, 0, $"Function '{term.FunctionCall!.Identifier.Name}' not found in group '{group.Name}' and its base groups", source.Name, term.Context));
                            return result;
                        }
                        result.Append(ArcFunctionCallGenerator.Generate(source, term.FunctionCall!, fn, true, group));
                        break;
                    case ArcCallChainTermType.Identifier:
                    {
                        var field = ArcGroupHelper.ResolveField(group, term.Identifier!.Name, source);
                        if (field == null)
                        {
                            result.Logs.Add(new ArcSourceLocatableLog(LogLevel.Error, 0, $"Field '{term.Identifier.Name}' not found in group '{group.Name}' and its base groups", source.Name, term.Context));
                            return result;
                        }
                        var fieldLocator = new ArcStackDataOperationDescriptor(ArcDataSourceType.Field, field.Id, true);
                        result.Append(new ArcLoadDataToStackInstruction(fieldLocator).Encode(source));
                        break;
                    }
                    default:
                        result.Logs.Add(new ArcSourceLocatableLog(LogLevel.Error, 0, "Invalid call chain term", source.Name, term.Context));
                        break;
                }

                foreach (var expr in term.Indices)
                {
                    result.Append(ArcExpressionEvaluationGenerator.GenerateEvaluationCommand(source, expr));

                    var arrayOperationDesc = new ArcStackDataOperationDescriptor(ArcDataSourceType.ArrayElement, 0, false);
                    result.Append(new ArcLoadDataToStackInstruction(arrayOperationDesc).Encode(source));
                }
            }

            return result;
        }
    }
}
