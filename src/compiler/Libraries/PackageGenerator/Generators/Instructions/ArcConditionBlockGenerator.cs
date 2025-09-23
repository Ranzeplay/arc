using Arc.Compiler.PackageGenerator.Models.Generation;
using Arc.Compiler.PackageGenerator.Models.PrimitiveInstructions;
using Arc.Compiler.PackageGenerator.Models.Relocation;
using Arc.Compiler.PackageGenerator.Models.Scope;
using Arc.Compiler.SyntaxAnalyzer.Models.Blocks;
using Arc.Compiler.SyntaxAnalyzer.Models.Function;

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
                var cbResult = GenerateConditionalBlock(source, block, fnNode, relocationLayer);

                conditionalBlocks.Add(cbResult);
            }

            conditionalBlocks.ForEach(result.Append);

            // Now the ElseBlock
            result.Append(GenerateOtherwiseBlock(source, ifBlock.ElseBody, fnNode, relocationLayer));

            var endIfLabelInstruction = new ArcLabellingInstruction(ArcRelocationLabelType.EndIfBlock, "end", relocationLayer).Encode(source);
            result.Append(endIfLabelInstruction);

            return result;
        }

        private static ArcPartialGenerationResult GenerateConditionalBlock(ArcGenerationSource source, ArcBlockConditional block, ArcScopeTreeFunctionNodeBase fnNode, Guid relocationLayerId)
        {
            var result = new ArcPartialGenerationResult();
            var beginSubBlockLabel = new ArcLabellingInstruction(ArcRelocationLabelType.BeginIfSubBlock, "begin", Guid.NewGuid()).Encode(source);
            var expression = ArcExpressionEvaluationGenerator.GenerateEvaluationCommand(source, block.Expression);
            var jumpNextInstruction = new ArcConditionalJumpInstruction(new()
            {
                TargetType = ArcRelocationTargetType.Label,
                Label = ArcRelocationLabelType.BeginIfSubBlock,
                Parameter = 1,
                Layer = relocationLayerId
            }).Encode(source);
            var body = ArcSequentialExecutionGenerator.Generate(source, block.Body, fnNode);
            var jumpOutInstruction = new ArcUnconditionalJumpInstruction(new()
            {
                TargetType = ArcRelocationTargetType.Label,
                Label = ArcRelocationLabelType.EndIfBlock,
                Parameter = 1,
                Layer = relocationLayerId
            }).Encode(source);
            var endSubBlockLabel = new ArcLabellingInstruction(ArcRelocationLabelType.EndIfSubBlock, "end", Guid.NewGuid()).Encode(source);
            result.Append(beginSubBlockLabel);
            result.Append(expression);
            result.Append(jumpNextInstruction);
            result.Append(body);
            result.Append(jumpOutInstruction);
            result.Append(endSubBlockLabel);
            return result;
        }

        private static ArcPartialGenerationResult GenerateOtherwiseBlock(ArcGenerationSource source, ArcFunctionBody? elseBody, ArcScopeTreeFunctionNodeBase fnNode, Guid relocationLayerId)
        {
            var result = new ArcPartialGenerationResult();
            var beginSubBlock = new ArcLabellingInstruction(ArcRelocationLabelType.BeginIfSubBlock, "begin", relocationLayerId);
            result.Append(beginSubBlock.Encode(source));
            if (elseBody != null)
            {
                var block = ArcSequentialExecutionGenerator.Generate(source, elseBody, fnNode);
                result.Append(block);
            }
            var endSubBlock = new ArcLabellingInstruction(ArcRelocationLabelType.EndIfSubBlock, "end", relocationLayerId);
            result.Append(endSubBlock.Encode(source));

            return result;
        }
    }
}
