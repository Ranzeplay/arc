using Arc.Compiler.SyntaxAnalyzer.Generated.ANTLR;
using Arc.Compiler.SyntaxAnalyzer.Models.Components;
using Arc.Compiler.SyntaxAnalyzer.Models.Data.DataType;
using Arc.Compiler.SyntaxAnalyzer.Models.Identifier;

namespace Arc.Compiler.SyntaxAnalyzer.Models.Function
{
    public class ArcFunctionArgument(ArcSourceCodeParser.Arc_data_declaratorContext source)
    {
        public ArcSingleIdentifier Identifier { get; set; } = new ArcSingleIdentifier(source.arc_single_identifier());

        public ArcDataType DataType { get; set; } = new ArcDataType(source.arc_data_type());

        public ArcDataDeclarator DataDeclarator { get; set; } = new ArcDataDeclarator(source);
    }
}
