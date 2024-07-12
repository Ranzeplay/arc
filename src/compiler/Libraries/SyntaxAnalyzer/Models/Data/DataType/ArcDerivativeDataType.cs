using Arc.Compiler.SyntaxAnalyzer.Generated.ANTLR;
using Arc.Compiler.SyntaxAnalyzer.Models.Identifier;

namespace Arc.Compiler.SyntaxAnalyzer.Models.Data.DataType
{
    internal class ArcDerivativeDataType(ArcSourceCodeParser.Arc_flexible_identifierContext context)
    {
        public ArcFlexibleIdentifier Identifier { get; set; } = new(context);
    }
}
