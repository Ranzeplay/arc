using Arc.Compiler.Shared.CommandGeneration.Mappings;
using Arc.Compiler.Shared.CommandGeneration.Relocation;
using Arc.Compiler.Shared.Parsing.AST;
using Arc.CompilerCommandGenerator.Models;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Arc.CompilerCommandGenerator.Builders
{
    internal class LoopCommand
    {
        public static PartialGenerationResult? Build(GenerationContext<LoopBlock> source)
        {
            var body = ActionBlockCommand.Build(source.TransferToNewComponent(source.Component.Block))!;

            // Jump to start
            var jump = Utils.CombineLeadingCommand((byte)RootCommand.Jump, (byte)JumpCommand.ToRelative).ToList();
            var placeholder = source.PackageMetadata.GenerateEmptyAddress();
            jump.AddRange(placeholder);

            var currentLoc = body.Commands.Count;
            var reloc = RelocationDescriptor.NewRelativeLocation(currentLoc, -currentLoc);

            body.Commands.AddRange(jump);
            body.RelocationDescriptors.Add(reloc);

            return body;
        }
    } 
}
