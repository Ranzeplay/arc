using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Arc.Compiler.Shared.Parsing.Components.Data
{
    public class DataType
    {
        public Identifier FullTypeIdentifier { get; }

        public bool IsArray { get; }

        public DataType(Identifier fullTypeIdentifier, bool isArray)
        {
            FullTypeIdentifier = fullTypeIdentifier;
            IsArray = isArray;
        }
    }
}
