using Arc.Compiler.SyntaxAnalyzer.Generated.ANTLR;
using Arc.Compiler.SyntaxAnalyzer.Models.Identifier;

namespace Arc.Compiler.SyntaxAnalyzer.Models.Data.DataType
{
    internal class ArcDerivativeDataType(ArcSourceCodeParser.Arc_derivative_data_typeContext context)
    {
        public ArcScopedIdentifier Identifier { get; set; } = new ArcScopedIdentifier(context.arc_scoped_identifier());
    }
}
