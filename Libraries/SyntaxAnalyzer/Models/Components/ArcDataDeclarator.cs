using Arc.Compiler.SyntaxAnalyzer.Generated.ANTLR;
using Arc.Compiler.SyntaxAnalyzer.Models.Data;
using Arc.Compiler.SyntaxAnalyzer.Models.Data.DataType;
using Arc.Compiler.SyntaxAnalyzer.Models.Identifier;

namespace Arc.Compiler.SyntaxAnalyzer.Models.Components
{
    internal class ArcDataDeclarator(ArcSourceCodeParser.Arc_data_declarationContext context)
    {
        public ArcParameterType ParameterType { get; set; } = ArcParameterTypeUtils.FromToken(context.arc_param_type());

        public ArcReassignability Reassignability { get; set; } = ArcReassignabilityUtils.FromToken(context.arc_reassignability());

        public ArcSingleIdentifier Identifier { get; set; } = new ArcSingleIdentifier(context.arc_single_identifier());

        public ArcDataType DataType { get; set; } = new ArcDataType(context.arc_data_type());
    }
}
