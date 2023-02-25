using Arc.Compiler.Shared.Parsing.Components.Data;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Arc.CompilerCommandGenerator.Models
{
    public class PartialGenerationResult
    {
        public byte[] Commands { get; }

        public DataDeclarator? NewDataDeclarator { get; }

        public PartialGenerationResult(byte[] commands, DataDeclarator? newDataDeclarator = null)
        {
            Commands = commands;
            NewDataDeclarator = newDataDeclarator;
        }
    }
}
