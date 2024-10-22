using Arc.Compiler.PackageGenerator.Models.Generation;
using Arc.Compiler.PackageGenerator.Models.Intermediate;
using Arc.Compiler.PackageGenerator.Models.PrimitiveInstructions;
using Arc.Compiler.PackageGenerator.Models.Relocation;
using Arc.Compiler.SyntaxAnalyzer.Models.Blocks;

namespace Arc.Compiler.PackageGenerator.Generators
{
    internal class ArcConditionBlockGenerator
    {
        public static ArcPartialGenerationResult Encode(ArcGenerationSource source, ArcBlockConditional conditionalBlock)
        {
            var result = new ArcPartialGenerationResult();

            var expr = ExpressionEvaluator.GenerateEvaluationCommand(source, conditionalBlock.Expression);
            result.Append(expr);

            var body = ArcSequentialExecutionGenerator.Generate(source, conditionalBlock.Body);
            var bodyLength = body.GeneratedData.LongCount();

            var jumpOutRelocation = new ArcRelocationTarget
            {
                TargetType = ArcRelocationTargetType.Relative,
                Offset = bodyLength
            };

            result.Append(new ArcConditionalJumpInstruction(jumpOutRelocation).Encode(source));
            result.Append(body);

            return result;
        }

        public static ArcPartialGenerationResult Encode(ArcGenerationSource source, ArcBlockIf ifBlock)
        {
            var result = new ArcPartialGenerationResult();

            var beginIfLabel = new ArcRelocationLabel()
            {
                Location = 0,
                Type = ArcRelocationLabelType.BeginIfBlock,
                Name = "begin"
            };
            var endIfLabel = new ArcRelocationLabel()
            {
                Location = -1,
                Type = ArcRelocationLabelType.EndIfBlock,
                Name = "end"
            };

            var conditionalBlocks = new List<ArcPartialGenerationResult>();
            foreach (var block in ifBlock.ConditionalBlocks)
            {
                var cbResult = new ArcPartialGenerationResult();
                var expr = ExpressionEvaluator.GenerateEvaluationCommand(source, block.Expression);

                var nextBlockLabel = new ArcRelocationLabel()
                {
                    Location = 0,
                    Type = ArcRelocationLabelType.BeginIfSubBlock,
                    Name = "next"
                };

                var jumpOutInstruction = new ArcConditionalJumpInstruction(new() { TargetType = ArcRelocationTargetType.Label, Label = endIfLabel }).Encode(source);
                var jumpNextInstruction = new ArcConditionalJumpInstruction(new() { TargetType = ArcRelocationTargetType.Label, Label = nextBlockLabel }).Encode(source);

                var body = ArcSequentialExecutionGenerator.Generate(source, block.Body);

                cbResult.Append(expr);
                cbResult.Append(jumpNextInstruction);
                cbResult.Append(body);
                cbResult.Append(jumpOutInstruction);

                conditionalBlocks.Add(cbResult);
            }

            conditionalBlocks.ForEach(result.Append);

            // Now the ElseBlock
            if (ifBlock.ElseBody != null)
            {
                var bResult = ArcSequentialExecutionGenerator.Generate(source, ifBlock.ElseBody);
                result.Append(bResult);
            }

            endIfLabel.Location = result.GeneratedData.LongCount();

            result.RelocationLabels = result.RelocationLabels.Concat([beginIfLabel, endIfLabel]);

            return result;
        }
    }
}
