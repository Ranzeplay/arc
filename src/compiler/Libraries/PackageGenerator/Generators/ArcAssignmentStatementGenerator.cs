using Arc.Compiler.PackageGenerator.Models.Generation;
using Arc.Compiler.PackageGenerator.Models.PrimitiveInstructions;
using Arc.Compiler.SyntaxAnalyzer.Models.Statements;

namespace Arc.Compiler.PackageGenerator.Generators
{
    internal static class ArcAssignmentStatementGenerator
    {
        public static ArcPartialGenerationResult Generate(this ArcStatementAssign assign, ArcGenerationSource source)
        {
            var result = new ArcPartialGenerationResult();

            var exprResult = ArcExpressionEvaluationGenerator.GenerateEvaluationCommand(source, assign.Expression);
            result.Append(exprResult);

            // Pop top element to the target
            var targetSymbol = source.LocalDataSlots.First(ds => ds.Declarator.Identifier.Name == assign.Identifier.Name);
            result.Append(new ArcPopToSlotInstruction(targetSymbol).Encode(source));

            return result;
        }
    }
}
