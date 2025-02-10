using Arc.Compiler.SyntaxAnalyzer.Generated.ANTLR;
using Arc.Compiler.SyntaxAnalyzer.Models.Components;
using Arc.Compiler.SyntaxAnalyzer.Models.Data.DataType;
using Arc.Compiler.SyntaxAnalyzer.Models.Identifier;

namespace Arc.Compiler.SyntaxAnalyzer.Models.Function
{
    public class ArcFunctionArgument
    {
        public ArcSingleIdentifier Identifier { get; set; }

        public ArcDataType DataType { get; set; }

        public ArcDataDeclarator DataDeclarator { get; set; }

        public ArcFunctionArgument(ArcSourceCodeParser.Arc_data_declaratorContext source)
        {
            Identifier = new ArcSingleIdentifier(source.arc_single_identifier());
            DataType = new ArcDataType(source.arc_data_type());
            DataDeclarator = new ArcDataDeclarator(source);
        }

        public ArcFunctionArgument(ArcSourceCodeParser.Arc_self_data_declaratorContext source)
        {
            Identifier = new ArcSingleIdentifier("self");
            DataType = new ArcDataType(source.arc_data_type());
            DataDeclarator = new ArcDataDeclarator(source);
        }
    }
}
