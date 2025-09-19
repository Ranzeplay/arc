using Arc.Compiler.PackageGenerator.Helpers;
using Arc.Compiler.PackageGenerator.Models.Generation;
using Arc.Compiler.PackageGenerator.Models.Intermediate;
using Arc.Compiler.PackageGenerator.Models.Logging;
using Arc.Compiler.PackageGenerator.Models.PrimitiveInstructions;
using Arc.Compiler.SyntaxAnalyzer.Models.Components;
using Arc.Compiler.SyntaxAnalyzer.Models.Components.CallChain;
using Arc.Compiler.SyntaxAnalyzer.Models.Expression;
using Arc.Compiler.SyntaxAnalyzer.Models.Statements;
using Microsoft.Extensions.Logging;

namespace Arc.Compiler.PackageGenerator.Generators.Instructions
{
    internal static class ArcAssignmentStatementGenerator
    {
        public static ArcPartialGenerationResult Generate(this ArcStatementAssign assign, ArcGenerationSource source)
        {
            var result = new ArcPartialGenerationResult();

            if (!assign.CallChain.Terms.All(t => t.Type == ArcCallChainTermType.Identifier))
            {
                result.Logs.Add(new ArcSourceLocatableLog(LogLevel.Error, 0, "Invalid assignment target", source.Name, assign.CallChain.Context));
            }

            // Handle lhs
            var lhsResult = GenerateLhs(assign.CallChain, source);

            // Handle rhs
            var firstTerm = assign.CallChain.Terms.First();
            var initialSlot = source.LocalDataSlots
                .First(s => s.DeclarationDescriptor.SyntaxTree.Identifier.Name == firstTerm.Identifier!.Name);
            var rhsResult = GenerateRhs(assign.Expression, source);

            // Combine the results
            result.Append(rhsResult);
            result.Append(lhsResult);

            return result;
        }

        private static ArcPartialGenerationResult GenerateLhs(ArcCallChain lhs, ArcGenerationSource source)
        {
            // Handle the first element of the call chain since it is from a data slot
            var firstTerm = lhs.Terms.First();
            var initialSlot = source.LocalDataSlots
                .First(s => s.DeclarationDescriptor.SyntaxTree.Identifier.Name == firstTerm.Identifier!.Name);

            bool isDirectAssignment = lhs.Terms.Count() == 1 && !lhs.Terms.First().Indices.Any();
            if (isDirectAssignment)
            {
                // If lhs has only one term, and it is being accessed direcly, then we can directly put the value into the slot
                return GenerateLhsDirectAssignment(initialSlot, source);
            }
            else
            {
                return GenerateLhsComplexAssignment(lhs, source);
            }
        }

        private static ArcPartialGenerationResult GenerateLhsDirectAssignment(ArcDataSlot slot, ArcGenerationSource source)
        {
            var result = new ArcPartialGenerationResult();
            var pushSlotDesc = new ArcStackDataOperationDescriptor(ArcDataSourceType.DataSlot, slot.SlotId, false);
            result.Append(new ArcSaveDataFromStackInstruction(pushSlotDesc).Encode(source));

            return result;
        }

        private static ArcPartialGenerationResult GenerateLhsComplexAssignment(ArcCallChain callChain, ArcGenerationSource source)
        {
            var firstTerm = callChain.Terms.First();
            var initialSlot = source.LocalDataSlots
                .First(s => s.DeclarationDescriptor.SyntaxTree.Identifier.Name == firstTerm.Identifier!.Name);
            var currentDataType = ArcDataTypeHelper.GetDataType(source, initialSlot.DeclarationDescriptor.SyntaxTree.DataType);

            var result = new ArcPartialGenerationResult();

            var firstSlotStackOperation = new ArcStackDataOperationDescriptor(ArcDataSourceType.DataSlot, initialSlot.SlotId, false);
            result.Append(new ArcLoadDataToStackInstruction(firstSlotStackOperation).Encode(source));

            foreach (var expr in firstTerm.Indices)
            {
                result.Append(ArcExpressionEvaluationGenerator.GenerateEvaluationCommand(source, expr));

                var arrayOperationDesc = new ArcStackDataOperationDescriptor(ArcDataSourceType.ArrayElement, 0, true);
                result.Append(new ArcLoadDataToStackInstruction(arrayOperationDesc).Encode(source));
            }

            // Handle the rest of the call chain since it is from the stack top
            foreach (var term in callChain.Terms.Skip(1))
            {
                if (currentDataType == null)
                {
                    result.Logs.Add(new ArcSourceLocatableLog(LogLevel.Error, 0, "Invalid field sequence since there is no matching data type", source.Name, term.Context));
                    break;
                }

                if (currentDataType.ResolvedType is ArcComplexType)
                {
                    var groupType = ArcDataTypeHelper.GetDataTypeNode(source, currentDataType.ResolvedType)!.ComplexTypeGroup!;
                    var field = groupType.Fields.First(f => f.IdentifierName == term.Identifier!.Name);

                    var stackOperation = new ArcStackDataOperationDescriptor(ArcDataSourceType.Field, field.Id, true);
                    result.Append(new ArcLoadDataToStackInstruction(stackOperation).Encode(source));

                    foreach (var expr in term.Indices)
                    {
                        result.Append(ArcExpressionEvaluationGenerator.GenerateEvaluationCommand(source, expr));

                        var arrayOperationDesc = new ArcStackDataOperationDescriptor(ArcDataSourceType.ArrayElement, 0, false);
                        result.Append(new ArcLoadDataToStackInstruction(arrayOperationDesc).Encode(source));
                    }

                    currentDataType = ArcDataTypeHelper.GetDataTypeNode(source, field.DataType.Type);
                }
                else
                {
                    result.Logs.Add(new ArcSourceLocatableLog(LogLevel.Error, 0, "Base type does not have further fields", source.Name, term.Context));
                }
            }

            // If lhs has more than one term, then the lhs should be a reference in the stack top
            result.Append(new ArcReplaceStackTopInstruction().Encode(source));

            return result;
        }

        private static ArcPartialGenerationResult GenerateRhs(ArcExpression expression, ArcGenerationSource source)
        {
            var rhsResult = ArcExpressionEvaluationGenerator.GenerateEvaluationCommand(
                source,
                expression
            );
            return rhsResult;
        }
    }
}
