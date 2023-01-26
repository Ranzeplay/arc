using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Arc.Compiler.Shared.Parsing.Components.Data
{
    public class DataDeclarator
    {
        public DataType DataType { get; }

        public Identifier Identifier { get; }

        public bool IsConstant { get; }

        public DataDeclarator(DataType dataType, Identifier identifier, bool isConstant)
        {
            DataType = dataType;
            Identifier = identifier;
            IsConstant = isConstant;
        }
    }
}
