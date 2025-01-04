using Arc.Compiler.PackageGenerator.Models.Generation;
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

            var expr = ArcExpressionEvaluationGenerator.GenerateEvaluationCommand(source, conditionalBlock.Expression);
            result.Append(expr);

            var body = ArcSequentialExecutionGenerator.Generate(source, conditionalBlock.Body);
            var bodyLength = body.GeneratedData.Count;

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

            var beginIfLabelInstruction = new ArcLabellingInstruction(ArcRelocationLabelType.BeginIfBlock, "begin").Encode(source);
            result.Append(beginIfLabelInstruction);

            var conditionalBlocks = new List<ArcPartialGenerationResult>();
            foreach (var block in ifBlock.ConditionalBlocks)
            {
                var cbResult = new ArcPartialGenerationResult();
                var expr = ArcExpressionEvaluationGenerator.GenerateEvaluationCommand(source, block.Expression);

                var beginSubBlockLabel = new ArcLabellingInstruction(ArcRelocationLabelType.BeginIfSubBlock, "next").Encode(source);
                var jumpNextInstruction = new ArcConditionalJumpInstruction(new()
                {
                    TargetType = ArcRelocationTargetType.Label,
                    Label = ArcRelocationLabelType.BeginIfSubBlock,
                    Parameter = 1
                }).Encode(source);
                var body = ArcSequentialExecutionGenerator.Generate(source, block.Body);
                var jumpOutInstruction = new ArcConditionalJumpInstruction(new()
                {
                    TargetType = ArcRelocationTargetType.Label,
                    Label = ArcRelocationLabelType.EndIfBlock,
                    Parameter = 1
                }).Encode(source);
                var endSubBlockLabel = new ArcLabellingInstruction(ArcRelocationLabelType.EndIfSubBlock, "end").Encode(source);

                cbResult.Append(beginSubBlockLabel);
                cbResult.Append(expr);
                cbResult.Append(jumpNextInstruction);
                cbResult.Append(body);
                cbResult.Append(jumpOutInstruction);
                cbResult.Append(endSubBlockLabel);

                conditionalBlocks.Add(cbResult);
            }

            conditionalBlocks.ForEach(result.Append);

            // Now the ElseBlock
            if (ifBlock.ElseBody != null)
            {
                var bResult = new ArcPartialGenerationResult();
                var beginSubBlock = new ArcLabellingInstruction(ArcRelocationLabelType.BeginIfSubBlock, "begin");
                var block = ArcSequentialExecutionGenerator.Generate(source, ifBlock.ElseBody);
                var endSubBlock = new ArcLabellingInstruction(ArcRelocationLabelType.EndIfSubBlock, "end");

                bResult.Append(beginSubBlock.Encode(source));
                bResult.Append(bResult);
                bResult.Append(endSubBlock.Encode(source));

                result.Append(bResult);
            }

            var endIfLabelInstruction = new ArcLabellingInstruction(ArcRelocationLabelType.EndIfBlock, "end").Encode(source);
            result.Append(endIfLabelInstruction);

            return result;
        }
    }
}
