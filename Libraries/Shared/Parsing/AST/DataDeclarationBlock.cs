using Arc.Compiler.Shared.Parsing.Components;
using Arc.Compiler.Shared.Parsing.Components.Data;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Arc.Compiler.Shared.Parsing.AST
{
    public class DataDeclarationBlock : DataDeclarator
    {
        public DataDeclarationBlock(DataType dataType, Identifier identifier, bool isConstant)
            : base(dataType, identifier, isConstant)
        {
        }
    }
}
