using Arc.Compiler.SyntaxAnalyzer.Generated.ANTLR;
using Arc.Compiler.SyntaxAnalyzer.Models.Data.DataType;
using Arc.Compiler.SyntaxAnalyzer.Models.Identifier;

namespace Arc.Compiler.SyntaxAnalyzer.Models.Function
{
    internal class ArcFunctionParameter(ArcSourceCodeParser.Arc_data_declarationContext source)
    {
        public ArcSingleIdentifier Identifier { get; set; } = new ArcSingleIdentifier(source.arc_single_identifier());

        public ArcDataType DataType { get; set; } = new ArcDataType(source.arc_data_type());
    }
}
