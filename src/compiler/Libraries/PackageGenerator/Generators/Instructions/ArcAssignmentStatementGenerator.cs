using Arc.Compiler.PackageGenerator.Helpers;
using Arc.Compiler.PackageGenerator.Models.Generation;
using Arc.Compiler.PackageGenerator.Models.Intermediate;
using Arc.Compiler.PackageGenerator.Models.PrimitiveInstructions;
using Arc.Compiler.SyntaxAnalyzer.Models.Components;
using Arc.Compiler.SyntaxAnalyzer.Models.Components.CallChain;
using Arc.Compiler.SyntaxAnalyzer.Models.Statements;

namespace Arc.Compiler.PackageGenerator.Generators.Instructions
{
    internal static class ArcAssignmentStatementGenerator
    {
        public static ArcPartialGenerationResult Generate(this ArcStatementAssign assign, ArcGenerationSource source)
        {
            var result = new ArcPartialGenerationResult();

            // Pop top element to the target
            if (assign.CallChain.Terms.Any(t => t.Type != ArcCallChainTermType.Identifier))
            {
                throw new InvalidDataException("Only simple assignment is supported");
            }

            if (assign.CallChain.Terms.Count() == 1)
            {
                var targetSymbol = source.LocalDataSlots
                    .First(ds => ds.DeclarationDescriptor.SyntaxTree.Identifier.Name == assign.CallChain.Terms.First().Identifier!.Name);
                result.Append(new ArcPopToSlotInstruction(targetSymbol).Encode(source));
                return result;
            }

            var initialSlot = source.LocalDataSlots
                .First(s => s.DeclarationDescriptor.SyntaxTree.Identifier.Name == assign.CallChain.Terms.First().Identifier!.Name);
            var currentDataType = ArcDataTypeHelper.GetDataTypeNode(source, initialSlot.DeclarationDescriptor.SyntaxTree.DataType);

            foreach (var term in assign.CallChain.Terms.Skip(1))
            {
                if (currentDataType == null)
                {
                    throw new InvalidDataException("Invalid field sequence");
                }

                if (currentDataType.DataType is ArcComplexType ct)
                {
                    var groupType = ArcDataTypeHelper.GetDataTypeGroupNode(source, ct);
                    var field = groupType.Descriptor.Fields.First(f => f.IdentifierName == term.Identifier!.Name);

                    var stackOperation = new ArcStackDataOperationDescriptor(ArcDataSourceType.Field, ArcMemoryStorageType.Reference, initialSlot.SlotId, true);
                    result.Append(new ArcSaveDataFromStackInstruction(stackOperation).Encode(source));

                    foreach (var expr in term.Indices)
                    {
                        result.Append(ArcExpressionEvaluationGenerator.GenerateEvaluationCommand(source, expr));

                        var arrayOperationDesc = new ArcStackDataOperationDescriptor(ArcDataSourceType.ArrayElement, ArcMemoryStorageType.Reference, 0, true);
                        result.Append(new ArcLoadDataToStackInstruction(arrayOperationDesc).Encode(source));
                    }

                    currentDataType = ArcDataTypeHelper.GetDataTypeNode(source, field.DataType.Type);
                }
                else
                {
                    throw new InvalidDataException("Base type does not have further fields");
                }
            }

            var exprResult = ArcExpressionEvaluationGenerator.GenerateEvaluationCommand(source, assign.Expression);
            result.Append(exprResult);

            var targetOperation = new ArcStackDataOperationDescriptor(ArcDataSourceType.StackTop, ArcMemoryStorageType.Reference, initialSlot.SlotId, true);
            result.Append(new ArcSaveDataFromStackInstruction(targetOperation).Encode(source));

            return result;
        }
    }
}
