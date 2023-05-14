using Arc.Compiler.Shared.CommandGeneration.Mappings;
using Arc.Compiler.Shared.CommandGeneration.Relocation;
using Arc.Compiler.Shared.Parsing.AST;
using Arc.CompilerCommandGenerator.Models;

namespace Arc.CompilerCommandGenerator.Builders
{
    internal class LoopCommand
    {
        public static PartialGenerationResult? Build(GenerationContext<LoopBlock> source)
        {
            var body = ActionBlockCommand.Build(source.TransferToNewComponent(source.Component.Block))!;

            // Jump to start
            var currentLoc = body.Commands.Count;
            var jump = Utils.CombineLeadingCommand((byte)RootCommand.Jump, (byte)JumpCommand.ToRelative).ToList();

            var reloc = RelocationTarget.NewRelativeLocation(currentLoc, jump.Count, new RelativeRelocator(RelativeRelocatorType.Address, -currentLoc));

            var placeholder = source.PackageMetadata.GenerateEmptyAddress();
            jump.AddRange(placeholder);

            body.Commands.AddRange(jump);
            body.RelocationTargets.Add(reloc);

            body.RelocationReferences.Add(new(0, RelocationReferenceType.LoopEntrance));
            body.RelocationReferences.Add(new(body.Commands.Count, RelocationReferenceType.EndLoop));

            return body;
        }
    }
}
