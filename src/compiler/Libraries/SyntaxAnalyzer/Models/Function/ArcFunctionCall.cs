using Arc.Compiler.SyntaxAnalyzer.Generated.ANTLR;
using Arc.Compiler.SyntaxAnalyzer.Models.Data.DataType;
using Arc.Compiler.SyntaxAnalyzer.Models.Identifier;

namespace Arc.Compiler.SyntaxAnalyzer.Models.Function
{
    public class ArcFunctionCall(ArcSourceCodeParser.Arc_function_call_baseContext context)
    {
        public ArcFlexibleIdentifier Identifier { get; set; } = new(context.arc_flexible_identifier());

        public IEnumerable<ArcFunctionCallArgument> Arguments { get; set; } = context.arc_wrapped_param_list().arc_param_list()?.arc_expression().Select(arg => new ArcFunctionCallArgument(arg)) ?? [];

        public IEnumerable<ArcDataType> SpecializedGenericTypes { get; set; } = context.arc_generic_specialization_wrapper()?.arc_data_type().Select(g => new ArcDataType(g)) ?? [];

        public ArcSourceCodeParser.Arc_function_call_baseContext Context { get; set; } = context;
    }
}
