using Arc.Compiler.PackageGenerator.Models.Generation;
using Arc.Compiler.PackageGenerator.Models.PrimitiveInstructions;
using Arc.Compiler.PackageGenerator.Models.Relocation;
using Arc.Compiler.SyntaxAnalyzer.Models.Blocks;

namespace Arc.Compiler.PackageGenerator.Generators.Instructions
{
    internal class ArcConditionLoopBlockGenerator
    {
        public static ArcPartialGenerationResult Encode(ArcGenerationSource source, ArcBlockConditionalLoop clBlock)
        {
            var result = new ArcPartialGenerationResult();

            var beginBlockLabel = new ArcLabellingInstruction(ArcRelocationLabelType.BeginLoopBlock, "begin").Encode(source);

            var expr = ArcExpressionEvaluationGenerator.GenerateEvaluationCommand(source, clBlock.ConditionalBlock.Expression, true);

            var body = ArcSequentialExecutionGenerator.Generate(source, clBlock.ConditionalBlock.Body);

            var jumpOutRelocator = new ArcRelocationTarget()
            {
                TargetType = ArcRelocationTargetType.Label,
                Label = ArcRelocationLabelType.EndLoopBlock,
                Parameter = 1
            };
            var jumpOutInstruction = new ArcConditionalJumpInstruction(jumpOutRelocator).Encode(source);

            var jumpBackRelocator = new ArcRelocationTarget()
            {
                TargetType = ArcRelocationTargetType.Label,
                Label = ArcRelocationLabelType.BeginLoopBlock,
                Parameter = -1
            };
            var jumpBackInstruction = new ArcUnconditionalJumpInstruction(jumpBackRelocator).Encode(source);

            var endBlockLabel = new ArcLabellingInstruction(ArcRelocationLabelType.EndLoopBlock, "end").Encode(source);

            result.Append(beginBlockLabel);
            result.Append(expr);
            result.Append(jumpOutInstruction);
            result.Append(body);
            result.Append(jumpBackInstruction);
            result.Append(endBlockLabel);

            return result;
        }
    }
}
