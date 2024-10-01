using Arc.Compiler.PackageGenerator.Models.Primitives;
using Arc.Compiler.SyntaxAnalyzer.Models.Blocks;

namespace Arc.Compiler.PackageGenerator.Models.Intermediate
{
    internal class ConditionLoopBlockGenerator
    {
        public static ArcGenerationResult Encode(ArcGenerationSource<ArcBlockConditionalLoop> source)
        {
            var result = new ArcGenerationResult();

            var expr = ExpressionEvaluator.GenerateEvaluationCommand(source.Migrate(source.Value.ConditionalBlock.Expression));

            var body = Flow.GenerateSequentialExecutionFlow(source.Migrate(source.Value.ConditionalBlock.Body));
            var bodyLength = body.GeneratedData.LongCount();

            var jumpOutInstruction = new ConditionalJumpInstruction().Encode(source.Migrate((ArcRelocationType.Relative, bodyLength)));
            var jumpBackInstruction = new ConditionalJumpInstruction().Encode(source.Migrate((
                    ArcRelocationType.Relative,
                    -(expr.GeneratedData.LongCount() + bodyLength + jumpOutInstruction.GeneratedData.LongCount())
                )));

            result.Append(expr);
            result.Append(jumpOutInstruction);
            result.Append(body);
            result.Append(jumpBackInstruction);

            return result;
        }
    }
}
