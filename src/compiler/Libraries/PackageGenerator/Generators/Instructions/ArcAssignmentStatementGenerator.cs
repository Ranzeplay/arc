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
            if (!assign.CallChain.Terms.All(t => t.Type == ArcCallChainTermType.Identifier))
            {
                throw new InvalidDataException("Only simple assignment is supported");
            }

            // Handle lhs
            var lhsResult = new ArcPartialGenerationResult();

            // Handle the first element of the call chain since it is from a data slot
            var firstTerm = assign.CallChain.Terms.First();
            var initialSlot = source.LocalDataSlots
                .First(s => s.DeclarationDescriptor.SyntaxTree.Identifier.Name == firstTerm.Identifier!.Name);
            var currentDataType = ArcDataTypeHelper.GetDataTypeNode(source, initialSlot.DeclarationDescriptor.SyntaxTree.DataType);

            if (assign.CallChain.Terms.Count() == 1 && !assign.CallChain.Terms.First().Indices.Any())
            {
                // If lhs has only one term, and it is being accessed direcly, then we can directly put the value into the slot
                var pushSlotDesc = new ArcStackDataOperationDescriptor(ArcDataSourceType.DataSlot, ArcMemoryStorageType.Reference, initialSlot.SlotId, false);
                lhsResult.Append(new ArcSaveDataFromStackInstruction(pushSlotDesc).Encode(source));
            }
            else
            {
                // If lhs has more than one term, then the lhs should be a reference in the stack top
                lhsResult.Append(new ArcReplaceStackTopInstruction().Encode(source));
            }

            foreach (var expr in firstTerm.Indices)
            {
                lhsResult.Append(ArcExpressionEvaluationGenerator.GenerateEvaluationCommand(source, expr, true));

                var arrayOperationDesc = new ArcStackDataOperationDescriptor(ArcDataSourceType.ArrayElement, ArcMemoryStorageType.Reference, 0, false);
                lhsResult.Append(new ArcLoadDataToStackInstruction(arrayOperationDesc).Encode(source));
            }

            // Handle the rest of the call chain since it is from the stack top
            var memoryStorageType = initialSlot.DeclarationDescriptor.MemoryStorageType;
            foreach (var term in assign.CallChain.Terms.Skip(1))
            {
                if (currentDataType == null)
                {
                    throw new InvalidDataException("Invalid field sequence");
                }

                if (currentDataType.DataType is ArcComplexType)
                {
                    var groupType = currentDataType.ComplexTypeGroup!;
                    var field = groupType.Fields.First(f => f.IdentifierName == term.Identifier!.Name);

                    var stackOperation = new ArcStackDataOperationDescriptor(ArcDataSourceType.Field, ArcMemoryStorageType.Reference, initialSlot.SlotId, true);
                    lhsResult.Append(new ArcSaveDataFromStackInstruction(stackOperation).Encode(source));

                    foreach (var expr in term.Indices)
                    {
                        lhsResult.Append(ArcExpressionEvaluationGenerator.GenerateEvaluationCommand(source, expr, true));

                        var arrayOperationDesc = new ArcStackDataOperationDescriptor(ArcDataSourceType.ArrayElement, ArcMemoryStorageType.Reference, 0, false);
                        lhsResult.Append(new ArcLoadDataToStackInstruction(arrayOperationDesc).Encode(source));
                    }

                    currentDataType = ArcDataTypeHelper.GetDataTypeNode(source, field.DataType.Type);

                    memoryStorageType = field.DataType.MemoryStorageType;
                }
                else
                {
                    throw new InvalidDataException("Base type does not have further fields");
                }
            }

            // Handle rhs
            var rhsResult = ArcExpressionEvaluationGenerator.GenerateEvaluationCommand(source, assign.Expression, memoryStorageType == ArcMemoryStorageType.Reference);

            // Combine the results
            var result = new ArcPartialGenerationResult();
            result.Append(rhsResult);
            result.Append(lhsResult);

            return result;
        }
    }
}
