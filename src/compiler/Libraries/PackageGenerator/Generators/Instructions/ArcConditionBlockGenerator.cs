using Arc.Compiler.PackageGenerator.Models.Generation;
using Arc.Compiler.PackageGenerator.Models.PrimitiveInstructions;
using Arc.Compiler.PackageGenerator.Models.Relocation;
using Arc.Compiler.PackageGenerator.Models.Scope;
using Arc.Compiler.SyntaxAnalyzer.Models.Blocks;

namespace Arc.Compiler.PackageGenerator.Generators.Instructions
{
    internal class ArcConditionBlockGenerator
    {
        public static ArcPartialGenerationResult Encode(ArcGenerationSource source, ArcBlockIf ifBlock, ArcScopeTreeFunctionNodeBase fnNode)
        {
            var relocationLayer = Guid.NewGuid();

            var result = new ArcPartialGenerationResult();

            var beginIfLabelInstruction = new ArcLabellingInstruction(ArcRelocationLabelType.BeginIfBlock, "begin", relocationLayer).Encode(source);
            result.Append(beginIfLabelInstruction);

            var conditionalBlocks = new List<ArcPartialGenerationResult>();
            foreach (var block in ifBlock.ConditionalBlocks)
            {
                var cbResult = new ArcPartialGenerationResult();
                var expr = ArcExpressionEvaluationGenerator.GenerateEvaluationCommand(source, block.Expression, true);

                var beginSubBlockLabel = new ArcLabellingInstruction(ArcRelocationLabelType.BeginIfSubBlock, "next", relocationLayer).Encode(source);
                var jumpNextInstruction = new ArcConditionalJumpInstruction(new()
                {
                    TargetType = ArcRelocationTargetType.Label,
                    Label = ArcRelocationLabelType.BeginIfSubBlock,
                    Parameter = 1,
                    Layer = relocationLayer
                }).Encode(source);
                var body = ArcSequentialExecutionGenerator.Generate(source, block.Body, fnNode);
                var jumpOutInstruction = new ArcUnconditionalJumpInstruction(new()
                {
                    TargetType = ArcRelocationTargetType.Label,
                    Label = ArcRelocationLabelType.EndIfBlock,
                    Parameter = 1,
                    Layer = relocationLayer
                }).Encode(source);
                var endSubBlockLabel = new ArcLabellingInstruction(ArcRelocationLabelType.EndIfSubBlock, "end", relocationLayer).Encode(source);

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
            var bResult = new ArcPartialGenerationResult();
            var beginSubBlock = new ArcLabellingInstruction(ArcRelocationLabelType.BeginIfSubBlock, "begin", relocationLayer);
            bResult.Append(beginSubBlock.Encode(source));
            if (ifBlock.ElseBody != null)
            {
                var block = ArcSequentialExecutionGenerator.Generate(source, ifBlock.ElseBody, fnNode);
                bResult.Append(block);
            }
            var endSubBlock = new ArcLabellingInstruction(ArcRelocationLabelType.EndIfSubBlock, "end", relocationLayer);
            bResult.Append(endSubBlock.Encode(source));

            result.Append(bResult);

            var endIfLabelInstruction = new ArcLabellingInstruction(ArcRelocationLabelType.EndIfBlock, "end", relocationLayer).Encode(source);
            result.Append(endIfLabelInstruction);

            return result;
        }
    }
}
