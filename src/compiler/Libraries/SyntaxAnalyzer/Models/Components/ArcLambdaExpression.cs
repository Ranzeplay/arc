using Arc.Compiler.SyntaxAnalyzer.Generated.ANTLR;
using Arc.Compiler.SyntaxAnalyzer.Models.Data.DataType;
using Arc.Compiler.SyntaxAnalyzer.Models.Function;

namespace Arc.Compiler.SyntaxAnalyzer.Models.Components;

public class ArcLambdaExpression : ArcNamelessFunction<ArcSourceCodeParser.Arc_lambda_expressionContext>
{
    public ArcLambdaExpression(ArcSourceCodeParser.Arc_lambda_expressionContext context)
    {
        Context = context;
        Declarator = new ArcNamelessFunctionDeclarator
        {
            ReturnType = new ArcDataType(context.arc_data_type()),
            Arguments = context.arc_wrapped_arg_list()?
                .arc_arg_list()
                .arc_data_declarator()
                .Select(d => new ArcFunctionArgument(d)) ?? []
        };
        Body = new ArcFunctionBody(context.arc_wrapped_function_body());
    }
}
