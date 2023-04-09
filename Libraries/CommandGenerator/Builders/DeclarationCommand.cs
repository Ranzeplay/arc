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
    internal class DeclarationCommand
    {
        public static PartialGenerationResult? Build(GenerationContext<DataDeclarationBlock> source)
        {
            var commands = Utils.CombineLeadingCommand((byte)RootCommand.Object, (byte)ObjectCommand.CreateLocal).ToList();

            commands.Add((byte)(source.Component.DataType.IsArray ? 0x01 : 0x00));

            return new(commands.ToArray(), source.Component);
        }
    }
}
