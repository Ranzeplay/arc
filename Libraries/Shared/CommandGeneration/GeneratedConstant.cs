using Arc.Compiler.Shared.Parsing.Components.Data;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Arc.Compiler.Shared.CommandGeneration
{
    public class GeneratedConstant
    {
        public long Id { get; }

        public DataType DataType { get; }

        public byte[] GeneratedBytes { get; }

        public GeneratedConstant(long id, DataType dataType, byte[] generatedBytes)
        {
            Id = id;
            DataType = dataType;
            GeneratedBytes = generatedBytes;
        }
    }
}
