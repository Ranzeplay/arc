using Arc.Compiler.SyntaxAnalyzer.Generated.ANTLR;
using Arc.Compiler.SyntaxAnalyzer.Models.Data.DataType;
using Arc.Compiler.SyntaxAnalyzer.Models.Function;

namespace Arc.Compiler.SyntaxAnalyzer.Models.Components;

public class ArcLambdaExpression(ArcSourceCodeParser.Arc_lambda_expressionContext context)
{
    public IEnumerable<ArcFunctionArgument> Parameters { get; set; } = context.arc_wrapped_arg_list().arc_arg_list()?.arc_data_declarator().Select(arg => new ArcFunctionArgument(arg)) ?? [];

    public ArcDataType ReturnType { get; set; } = new(context.arc_data_type());
    
    public ArcSourceCodeParser.Arc_lambda_expressionContext Context { get; set; } = context;
}