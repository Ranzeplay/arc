using Arc.Compiler.PackageGenerator.Models.Generation;
using Arc.Compiler.PackageGenerator.Models.Intermediate;
using Arc.Compiler.PackageGenerator.Models.PrimitiveInstructions;
using Arc.Compiler.SyntaxAnalyzer.Models.Components.CallChain;
using Arc.Compiler.SyntaxAnalyzer.Models.Statements;

namespace Arc.Compiler.PackageGenerator.Generators.Instructions
{
    internal static class ArcAssignmentStatementGenerator
    {
        public static ArcPartialGenerationResult Generate(this ArcStatementAssign assign, ArcGenerationSource source)
        {
            var result = new ArcPartialGenerationResult();

            var exprResult = ArcExpressionEvaluationGenerator.GenerateEvaluationCommand(source, assign.Expression);
            result.Append(exprResult);

            // Pop top element to the target
            if (assign.CallChain.Terms.Any(t => t.Type != ArcCallChainTermType.Identifier))
            {
                throw new InvalidDataException("Only simple assignment is supported");
            }

            if (assign.CallChain.Terms.Count() == 1)
            {
                var targetSymbol = source.LocalDataSlots.First(ds => ds.Declarator.Identifier.Name == assign.CallChain.Terms.First().Identifier!.Name);
                result.Append(new ArcPopToSlotInstruction(targetSymbol).Encode(source));
                return result;
            }

            var identifierSequence = assign.CallChain.Terms.Select(t => t.Identifier!.Name);
            var initialSlot = source.LocalDataSlots.First(s => s.Declarator.Identifier.Name == identifierSequence.First());
            var currentDataType = Utils.GetDataTypeNode(source, initialSlot.Declarator.DataType);

            var locator = new ArcDataLocator(ArcDataSourceType.DataSlot, initialSlot.SlotId, [], []);
            foreach (var identifier in identifierSequence.Skip(1))
            {
                if (currentDataType == null)
                {
                    throw new InvalidDataException("Invalid field sequence");
                }

                if (currentDataType.DataType is ArcComplexType ct)
                {
                    var groupType = Utils.GetDataTypeGroupNode(source, ct);
                    var field = groupType.Descriptor.Fields.First(f => f.IdentifierName == identifier);
                    locator.FieldChain.Add(field);
                    currentDataType = Utils.GetDataTypeNode(source, field.DataType.Type);
                }
                else
                {
                    throw new InvalidDataException("Base type does not have further fields");
                }
            }

            result.Append(new ArcSaveDataFromStackInstruction(locator).Encode(source));

            return result;
        }
    }
}
