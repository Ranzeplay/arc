using Arc.Compiler.PackageGenerator.Models.Generation;
using Arc.Compiler.PackageGenerator.Models.Intermediate;
using Arc.Compiler.PackageGenerator.Models.PrimitiveInstructions;
using Arc.Compiler.SyntaxAnalyzer.Models.Statements;

namespace Arc.Compiler.PackageGenerator.Generators
{
    internal class ArcSequenceReturnGenerator
    {
        public static ArcPartialGenerationResult Generate(ArcGenerationSource source, ArcStatementReturn stmt)
        {
            var result = new ArcPartialGenerationResult();

            result.Append(new ArcReturnFromFunctionInstruction(stmt.Expression is not null).Encode(source));
            if (stmt.Expression is not null)
            {
                result.Append(ExpressionEvaluator.GenerateEvaluationCommand(source, stmt.Expression));
            }

            return result;
        }
    }
}
