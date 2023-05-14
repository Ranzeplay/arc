using Arc.Compiler.Shared.Parsing.Components;
using Arc.Compiler.Shared.Parsing.Components.Data;

namespace Arc.Compiler.Shared.Parsing.AST
{
    public record DataDeclarationBlock : DataDeclarator
    {
        public DataDeclarationBlock(DataType dataType, Identifier identifier, bool isConstant)
            : base(dataType, identifier, isConstant)
        {
        }
    }
}
