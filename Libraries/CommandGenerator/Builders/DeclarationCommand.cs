using Arc.Compiler.Shared.CommandGeneration;
using Arc.Compiler.Shared.Parsing.AST;
using Arc.CompilerCommandGenerator.Models;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Arc.CompilerCommandGenerator.Builders
{
    public class DeclarationCommand
    {
        public static PartialGenerationResult? Build(GenerationSource<DataDeclarationBlock> source)
        {
            var commands = Utils.CombineLeadingCommand((byte)RootCommand.Object, (byte)ObjectCommand.CreateLocal).ToList();

            commands.Add((byte)(source.ActionBlock.DataType.IsArray ? 0x01 : 0x00));

            return new(commands.ToArray(), source.ActionBlock);
        }
    }
}
