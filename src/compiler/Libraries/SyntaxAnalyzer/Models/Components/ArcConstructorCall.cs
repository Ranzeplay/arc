using Arc.Compiler.SyntaxAnalyzer.Generated.ANTLR;
using Arc.Compiler.SyntaxAnalyzer.Models.Data.DataType;
using Arc.Compiler.SyntaxAnalyzer.Models.Expression;
using Arc.Compiler.SyntaxAnalyzer.Models.Identifier;

namespace Arc.Compiler.SyntaxAnalyzer.Models.Components
{
    public class ArcConstructorCall(ArcSourceCodeParser.Arc_constructor_callContext context)
    {
        public ArcFlexibleIdentifier DataType { get; set; } = new(context.arc_flexible_identifier());

        public IEnumerable<ArcExpression> Parameters { get; set; } = context.arc_wrapped_param_list()
                .arc_param_list()?
                .arc_expression()
                .Select(e => new ArcExpression(e)) ?? [];

        public IEnumerable<ArcDataType> SpecializedGenericTypes { get; set; } = context.arc_generic_specialization_wrapper()?
                .arc_data_type()
                .Select(g => new ArcDataType(g)) ?? [];

        public ArcSourceCodeParser.Arc_constructor_callContext Context { get; set; } = context;
    }
}
