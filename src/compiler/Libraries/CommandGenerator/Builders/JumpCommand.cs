using Arc.Compiler.CommandGenerator;
using Arc.Compiler.CommandGenerator.Models;
using Arc.Compiler.Shared.CommandGeneration.Mappings;
using Arc.Compiler.Shared.CommandGeneration.Relocation;

namespace Arc.Compiler.CommandGenerator.Builders
{
    internal class JumpCommand
    {
        public static PartialGenerationResult BuildRelative(GenerationContext<RelativeRelocator> source)
        {
            var commands = Utils.CombineLeadingCommand((byte)RootCommand.Jump, (byte)Shared.CommandGeneration.Mappings.JumpCommand.ToRelative).ToList();
            commands.AddRange(source.PackageMetadata.GenerateEmptyAddress());

            var relocationTarget = RelocationTarget.NewRelativeLocation(0, commands.Count, source.Component);

            return new PartialGenerationResult(commands, relocationDescriptors: new List<RelocationTarget>() { relocationTarget });
        }

        public static PartialGenerationResult BuildConditional(GenerationContext<JumpRelativeCommandViewModel> source)
        {
            var commands = Utils.CombineLeadingCommand((byte)RootCommand.Jump, (byte)Shared.CommandGeneration.Mappings.JumpCommand.Conditional).ToList();
            var relocationTargets = new List<RelocationTarget>();

            // True condition
            relocationTargets.Add(RelocationTarget.NewRelativeLocation(0, commands.Count, source.Component.TrueTarget));
            commands.AddRange(source.PackageMetadata.GenerateEmptyAddress());

            // False condition
            relocationTargets.Add(RelocationTarget.NewRelativeLocation(0, commands.Count, source.Component.FalseTarget));
            commands.AddRange(source.PackageMetadata.GenerateEmptyAddress());

            return new PartialGenerationResult(commands, relocationDescriptors: relocationTargets);
        }
    }
}
