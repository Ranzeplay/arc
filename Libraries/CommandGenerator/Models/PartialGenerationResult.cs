using Arc.Compiler.Shared.CommandGeneration;
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
        public byte[] Commands { get; set; }

        public DataDeclarator? NewDataDeclarator { get; set; }

        public IEnumerable<GeneratedConstant> GeneratedConstants { get; set; }

        public PartialGenerationResult(byte[] commands, DataDeclarator? newDataDeclarator = null, IEnumerable<GeneratedConstant>? generatedConstants = null)
        {
            Commands = commands;
            NewDataDeclarator = newDataDeclarator;
            GeneratedConstants = generatedConstants ?? Enumerable.Empty<GeneratedConstant>();
        }

        public PartialGenerationResult()
        {
            Commands = Array.Empty<byte>();
            GeneratedConstants = Enumerable.Empty<GeneratedConstant>();
            NewDataDeclarator = null;
        }
    }
}
