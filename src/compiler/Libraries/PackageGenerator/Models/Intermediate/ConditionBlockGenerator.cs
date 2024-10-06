using Arc.Compiler.PackageGenerator.Models.Primitives;
using Arc.Compiler.SyntaxAnalyzer.Models.Blocks;

namespace Arc.Compiler.PackageGenerator.Models.Intermediate
{
    internal class ConditionBlockGenerator
    {
        public static ArcGenerationResult Encode(ArcGenerationSource<ArcBlockConditional> source)
        {
            var result = new ArcGenerationResult();

            var expr = ExpressionEvaluator.GenerateEvaluationCommand(source.Migrate(source.Value.Expression));
            result.Append(expr);

            var body = Flow.GenerateSequentialExecutionFlow(source.Migrate(source.Value.Body));
            var bodyLength = body.GeneratedData.LongCount();

            result.Append(new ConditionalJumpInstruction().Encode(source.Migrate((ArcRelocationType.Relative, bodyLength))));
            result.Append(body);

            return result;
        }

        public static ArcGenerationResult Encode(ArcGenerationSource<ArcBlockIf> source)
        {
            var result = new ArcGenerationResult();

            var beginIfLabel = new ArcLabel()
            {
                Id = new Random().Next(),
                Position = 0,
                Type = ArcLabelType.BeginIf,
                Name = "begin"
            };
            var endIfLabel = new ArcLabel()
            {
                Id = new Random().Next(),
                Position = -1,
                Type = ArcLabelType.EndIf,
                Name = "end"
            };

            var conditionalBlocks = new List<ArcGenerationResult>();
            foreach (var block in source.Value.ConditionalBlocks)
            {
                var cbResult = new ArcGenerationResult();
                var expr = ExpressionEvaluator.GenerateEvaluationCommand(source.Migrate(block.Expression));
                var jumpOutInstruction = new ConditionalJumpInstruction().Encode(source.Migrate((ArcRelocationType.Relative, 0)));
                var body = Flow.GenerateSequentialExecutionFlow(source.Migrate(block.Body));
            }

            return result;
        }
    }
}
