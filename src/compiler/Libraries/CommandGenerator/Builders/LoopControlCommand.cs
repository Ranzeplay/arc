using Arc.Compiler.CommandGenerator;
using Arc.Compiler.CommandGenerator.Models;
using Arc.Compiler.Shared.CommandGeneration;
using Arc.Compiler.Shared.CommandGeneration.Mappings;
using Arc.Compiler.Shared.CommandGeneration.Relocation;

namespace Arc.Compiler.CommandGenerator.Builders
{
    internal class LoopControlCommand
    {
        public static PartialGenerationResult BuildBreakCommand(PackageMetadata metadata)
        {
            var jump = Utils.CombineLeadingCommand((byte)RootCommand.Jump, (byte)Shared.CommandGeneration.Mappings.JumpCommand.ToRelative).ToList();
            var relocation = RelocationTarget.NewRelativeLocation(0, jump.Count, new RelativeRelocator(RelativeRelocatorType.IterationEnd));
            jump.AddRange(metadata.GenerateEmptyAddress());

            return new PartialGenerationResult(jump, null, null, new List<RelocationTarget> { relocation });
        }

        public static PartialGenerationResult BuildContinueCommand(PackageMetadata metadata)
        {
            var jump = Utils.CombineLeadingCommand((byte)RootCommand.Jump, (byte)Shared.CommandGeneration.Mappings.JumpCommand.ToRelative).ToList();
            var relocation = RelocationTarget.NewRelativeLocation(0, jump.Count, new RelativeRelocator(RelativeRelocatorType.IterationEntry));
            jump.AddRange(metadata.GenerateEmptyAddress());

            return new PartialGenerationResult(jump, null, null, new List<RelocationTarget> { relocation });
        }
    }
}
