using Arc.Compiler.Shared.Parsing.Components.Data;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Arc.Compiler.Shared.Parsing.Components.Function
{
    public class FunctionDeclarator
    {
        public Identifier Identifier { get; }

        public DataType ReturnDataType { get; }

        public FunctionParameter[] Parameters { get; }

        public FunctionDeclarator(Identifier identifier, DataType returnDataType, FunctionParameter[] parameters)
        {
            Identifier = identifier;
            ReturnDataType = returnDataType;
            Parameters = parameters;
        }
    }
}
