using Arc.Compiler.PackageGenerator.Models.Generation;
using Arc.Compiler.PackageGenerator.Models.PrimitiveInstructions;
using Arc.Compiler.PackageGenerator.Models.Relocation;
using Arc.Compiler.SyntaxAnalyzer.Models.Blocks;

namespace Arc.Compiler.PackageGenerator.Generators
{
    internal class ArcConditionLoopBlockGenerator
    {
        public static ArcPartialGenerationResult Encode(ArcGenerationSource source, ArcBlockConditionalLoop clBlock)
        {
            var result = new ArcPartialGenerationResult();

            var expr = ArcExpressionEvaluationGenerator.GenerateEvaluationCommand(source, clBlock.ConditionalBlock.Expression);

            var body = ArcSequentialExecutionGenerator.Generate(source, clBlock.ConditionalBlock.Body);
            var bodyLength = body.GeneratedData.LongCount();

            var jumpOutRelocator = new ArcRelocationTarget()
            {
                TargetType = ArcRelocationTargetType.Relative,
                Offset = body.GeneratedData.LongCount()
            };
            var jumpOutInstruction = new ArcConditionalJumpInstruction(jumpOutRelocator).Encode(source);

            var jumpBackRelocator = new ArcRelocationTarget()
            {
                TargetType = ArcRelocationTargetType.Relative,
                Offset = -(expr.GeneratedData.LongCount() + bodyLength + jumpOutInstruction.GeneratedData.LongCount())
            };
            var jumpBackInstruction = new ArcConditionalJumpInstruction(jumpBackRelocator).Encode(source);

            result.Append(expr);
            result.Append(jumpOutInstruction);
            result.Append(body);
            result.Append(jumpBackInstruction);

            return result;
        }
    }
}
