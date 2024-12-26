using Arc.Compiler.PackageGenerator.Models.Generation;
using Arc.Compiler.PackageGenerator.Models.PrimitiveInstructions;
using Arc.Compiler.PackageGenerator.Models.Relocation;

namespace Arc.Compiler.PackageGenerator.Generators
{
    internal class ArcLoopControlGenerator
    {
        public static ArcPartialGenerationResult GenerateBreak(ArcGenerationSource source)
        {
            var result = new ArcPartialGenerationResult();

            var jumpCommand = new ArcUnconditionalJumpInstruction()
            {
                Target = new()
                {
                    TargetType = ArcRelocationTargetType.Label,
                    Parameter = 1,
                    Label = ArcRelocationLabelType.EndLoopBlock
                }
            };

            result.Append(jumpCommand.Encode(source));

            return result;
        }

        public static ArcPartialGenerationResult GenerateContinue(ArcGenerationSource source)
        {
            var result = new ArcPartialGenerationResult();

            var jumpCommand = new ArcUnconditionalJumpInstruction()
            {
                Target = new()
                {
                    TargetType = ArcRelocationTargetType.Label,
                    Parameter = -1,
                    Label = ArcRelocationLabelType.BeginLoopBlock
                },
            };

            result.Append(jumpCommand.Encode(source));

            return result;
        }
    }
}
