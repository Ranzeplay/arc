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
using Microsoft.Extensions.Logging;

namespace Arc.Compiler.PackageGenerator.Generators.Instructions
{
    internal class ArcCallChainGenerator
    {
        public static ArcPartialGenerationResult Generate(ArcGenerationSource source, ArcCallChain callChain, ArcMemoryStorageType requiredMemotyStorageType)
        {
            var result = new ArcPartialGenerationResult();
            ArcDataDeclarationDescriptor lastTermTypeDecl;

            // First term maybe be variant, so handle it separately
            var firstTerm = callChain.Terms.First();
            if (firstTerm.Type == ArcCallChainTermType.Identifier)
            {
                var identifier = firstTerm.Identifier!;
                var slot = source.LocalDataSlots.First(s => s.DeclarationDescriptor.SyntaxTree.Identifier.Name == identifier.Name);
                lastTermTypeDecl = slot.DeclarationDescriptor;
                var locator = new ArcStackDataOperationDescriptor(ArcDataSourceType.DataSlot, requiredMemotyStorageType, slot.SlotId, false);

                result.Append(new ArcLoadDataToStackInstruction(locator).Encode(source));
            }
            else
            {
                var call = firstTerm.FunctionCall!;
                result.Append(ArcFunctionCallGenerator.Generate(source, call, false));

                var (targetFuncId, logs) = ArcFunctionHelper.GetFunctionId(source, call);
                if(logs.Any())
                {
                    result.Logs.AddRange(logs);
                    return result;
                }


                var function = source.CurrentNode.Root.GetSpecificChild<ArcScopeTreeFunctionNodeBase>(f => f.Id == targetFuncId, true);

                if (function == null)
                {
                    result.Logs.Add(new ArcSourceLocatableLog(LogLevel.Error, 0, "Function not found", source.Name, call.Context));
                    return result;
                }

                lastTermTypeDecl = function.ReturnValueType;
            }

            foreach (var expr in firstTerm.Indices)
            {
                result.Append(ArcExpressionEvaluationGenerator.GenerateEvaluationCommand(source, expr, true));

                var arrayOperationDesc = new ArcStackDataOperationDescriptor(ArcDataSourceType.ArrayElement, requiredMemotyStorageType, 0, true);
                result.Append(new ArcLoadDataToStackInstruction(arrayOperationDesc).Encode(source));
            }

            foreach (var term in callChain.Terms.Skip(1))
            {
                if (lastTermTypeDecl.Type is ArcBaseType)
                {
                    result.Logs.Add(new ArcSourceLocatableLog(LogLevel.Error, 0, "Cannot access fields of a base type", source.Name, term.Context));
                }

                var dataType = (lastTermTypeDecl.Type as ArcComplexType)!;
                var group = source.CurrentNode.Root.GetSpecificChild<ArcScopeTreeDataTypeNode>(g => g.ComplexTypeGroup?.Id == dataType.GroupId, true)?.ComplexTypeGroup!;

                if (term.Type == ArcCallChainTermType.FunctionCall)
                {
                    // Handle the function call of this term
                    result.Append(ArcFunctionCallGenerator.Generate(source, term.FunctionCall!, true, group));
                }
                else if (term.Type == ArcCallChainTermType.Identifier)
                {
                    var field = group.Fields.First(f => f.IdentifierName == term.Identifier!.Name);
                    var fieldLocator = new ArcStackDataOperationDescriptor(ArcDataSourceType.Field, requiredMemotyStorageType, field.Id, true);
                    result.Append(new ArcLoadDataToStackInstruction(fieldLocator).Encode(source));
                }
                else
                {
                    result.Logs.Add(new ArcSourceLocatableLog(LogLevel.Error, 0, "Invalid call chain term", source.Name, term.Context));
                }

                foreach (var expr in term.Indices)
                {
                    result.Append(ArcExpressionEvaluationGenerator.GenerateEvaluationCommand(source, expr, true));

                    var arrayOperationDesc = new ArcStackDataOperationDescriptor(ArcDataSourceType.ArrayElement, requiredMemotyStorageType, 0, false);
                    result.Append(new ArcLoadDataToStackInstruction(arrayOperationDesc).Encode(source));
                }
            }

            return result;
        }
    }
}
