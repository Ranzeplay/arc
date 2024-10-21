using Arc.Compiler.PackageGenerator.Models.Generation;
using Arc.Compiler.PackageGenerator.Models.PrimitiveInstructions;
using Arc.Compiler.PackageGenerator.Models.Relocation;

namespace Arc.Compiler.PackageGenerator.Generators
{
    internal class LoopControlGenerator
    {
        public static ArcPartialGenerationResult GenerateBreak(ArcGenerationSource source)
        {
            var result = new ArcPartialGenerationResult();

            var jumpCommand = new ArcUnconditionalJumpInstruction()
            {
                Target = new()
                {
                    TargetType = ArcRelocationTargetType.Label,
                    Location = 1,
                    Label = new ArcRelocationLabel()
                    {
                        Name = "loop_end",
                        Location = 1,
                        Type = ArcRelocationLabelType.EndWhileBlock,
                    }
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
                    Location = -1,
                    Label = new ArcRelocationLabel()
                    {
                        Name = "loop_begin",
                        Location = -1,
                        Type = ArcRelocationLabelType.BeginWhileBlock,
                    }
                },
            };

            result.Append(jumpCommand.Encode(source));

            return result;
        }
    }
}
