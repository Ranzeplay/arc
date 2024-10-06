using Arc.Compiler.PackageGenerator.Models.Generation;
using Arc.Compiler.PackageGenerator.Models.Primitives;
using Arc.Compiler.PackageGenerator.Models.Relocation;
using Arc.Compiler.SyntaxAnalyzer.Models.Blocks;

namespace Arc.Compiler.PackageGenerator.Models.Intermediate
{
    internal class ConditionBlockGenerator
    {
        public static ArcPartialGenerationResult Encode(ArcGenerationSource source, ArcBlockConditional conditionalBlock)
        {
            var result = new ArcPartialGenerationResult();

            var expr = ExpressionEvaluator.GenerateEvaluationCommand(source, conditionalBlock.Expression);
            result.Append(expr);

            var body = Flow.GenerateSequentialExecutionFlow(source, conditionalBlock.Body);
            var bodyLength = body.GeneratedData.LongCount();

            var jumpOutRelocation = new ArcRelocationTarget
            {
                TargetType = ArcRelocationTargetType.Relative,
                Offset = bodyLength
            };

            result.Append(new ConditionalJumpInstruction(jumpOutRelocation).Encode(source));
            result.Append(body);

            return result;
        }

        // TODO: Complete function
        public static ArcPartialGenerationResult Encode(ArcGenerationSource source, ArcBlockIf ifBlock)
        {
            var result = new ArcPartialGenerationResult();

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

            var conditionalBlocks = new List<ArcPartialGenerationResult>();
            foreach (var block in ifBlock.ConditionalBlocks)
            {
                var cbResult = new ArcPartialGenerationResult();
                var expr = ExpressionEvaluator.GenerateEvaluationCommand(source, block.Expression);


                // var jumpOutInstruction = new ConditionalJumpInstruction(new() { TargetType = ArcRelocationTargetType.Label, Label = endIfLabel}).Encode(source);

                var body = Flow.GenerateSequentialExecutionFlow(source, block.Body);
            }

            return result;
        }
    }
}
