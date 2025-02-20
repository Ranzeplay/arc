using Arc.Compiler.PackageGenerator.Helpers;
using Arc.Compiler.PackageGenerator.Models.Builtin;
using Arc.Compiler.PackageGenerator.Models.Descriptors;
using Arc.Compiler.PackageGenerator.Models.Generation;
using Arc.Compiler.PackageGenerator.Models.Intermediate;
using Arc.Compiler.PackageGenerator.Models.PrimitiveInstructions;
using Arc.Compiler.PackageGenerator.Models.Scope;
using Arc.Compiler.SyntaxAnalyzer.Models.Components;
using Arc.Compiler.SyntaxAnalyzer.Models.Components.CallChain;

namespace Arc.Compiler.PackageGenerator.Generators.Instructions
{
    internal class ArcCallChainGenerator
    {
        public static ArcPartialGenerationResult Generate(ArcGenerationSource source, ArcCallChain callChain)
        {
            var result = new ArcPartialGenerationResult();
            ArcDataDeclarationDescriptor lastTermTypeDecl;
            var locator = new ArcStackDataOperationDescriptor(ArcDataSourceType.Invalid, ArcMemoryStorageType.Value, -1, true);

            // First term maybe be variant, so handle it separately
            if (callChain.Terms.First().Type == ArcCallChainTermType.Identifier)
            {
                locator.Source = ArcDataSourceType.DataSlot;

                var identifier = callChain.Terms.First().Identifier!;
                var slot = source.LocalDataSlots.First(s => s.DeclarationDescriptor.SyntaxTree.Identifier.Name == identifier.Name);
                locator.LocationId = slot.SlotId;
                lastTermTypeDecl = slot.DeclarationDescriptor;

                if (callChain.Terms.Count() == 1)
                {
                    result.Append(new ArcLoadDataToStackInstruction(locator).Encode(source));

                    foreach (var expr in callChain.Terms.First().Indices)
                    {
                        result.Append(ArcExpressionEvaluationGenerator.GenerateEvaluationCommand(source, expr));

                        var arrayOperationDesc = new ArcStackDataOperationDescriptor(ArcDataSourceType.ArrayElement, ArcMemoryStorageType.Value, 0, false);
                        result.Append(new ArcLoadDataToStackInstruction(arrayOperationDesc).Encode(source));
                    }

                    return result;
                }
            }
            else
            {
                var call = callChain.Terms.First().FunctionCall!;
                result.Append(ArcFunctionCallGenerator.Generate(source, call, false));

                var targetFunctionId = ArcFunctionHelper.GetFunctionId(source, call);
                var function = source.CurrentNode.Root.GetSpecificChild<ArcScopeTreeFunctionNodeBase>(f => f.Id == targetFunctionId, true);
                lastTermTypeDecl = function.Descriptor.ReturnValueType;
            }

            foreach (var expr in callChain.Terms.First().Indices)
            {
                result.Append(ArcExpressionEvaluationGenerator.GenerateEvaluationCommand(source, expr));

                var arrayOperationDesc = new ArcStackDataOperationDescriptor(ArcDataSourceType.ArrayElement, ArcMemoryStorageType.Value, 0, false);
                result.Append(new ArcLoadDataToStackInstruction(arrayOperationDesc).Encode(source));
            }

            foreach (var term in callChain.Terms.Skip(1))
            {
                if (lastTermTypeDecl.Type is ArcBaseType)
                {
                    throw new InvalidDataException("Cannot call a primitive data type");
                }

                var dataType = (lastTermTypeDecl.Type as ArcComplexType)!;
                var group = source.CurrentNode.Root.GetSpecificChild<ArcScopeTreeGroupNode>(g => g.Descriptor.Id == dataType.GroupId, true)!;

                if (term.Type == ArcCallChainTermType.FunctionCall)
                {
                    // Handle the function call of this term
                    result.Append(ArcFunctionCallGenerator.Generate(source, term.FunctionCall!, true, group));

                    foreach (var expr in term.Indices)
                    {
                        result.Append(ArcExpressionEvaluationGenerator.GenerateEvaluationCommand(source, expr));

                        var arrayOperationDesc = new ArcStackDataOperationDescriptor(ArcDataSourceType.ArrayElement, ArcMemoryStorageType.Value, 0, false);
                        result.Append(new ArcLoadDataToStackInstruction(arrayOperationDesc).Encode(source));
                    }
                }
                else if (term.Type == ArcCallChainTermType.Identifier)
                {
                    var field = group.Descriptor.Fields.First(f => f.IdentifierName == term.Identifier!.Name);
                    var fieldLocator = new ArcStackDataOperationDescriptor(ArcDataSourceType.Field, ArcMemoryStorageType.Value, field.Id, true);
                    result.Append(new ArcLoadDataToStackInstruction(fieldLocator).Encode(source));

                    foreach (var expr in term.Indices)
                    {
                        result.Append(ArcExpressionEvaluationGenerator.GenerateEvaluationCommand(source, expr));

                        var arrayOperationDesc = new ArcStackDataOperationDescriptor(ArcDataSourceType.ArrayElement, ArcMemoryStorageType.Value, 0, false);
                        result.Append(new ArcLoadDataToStackInstruction(arrayOperationDesc).Encode(source));
                    }
                }
            }

            return result;
        }
    }
}
