using Arc.Compiler.SyntaxAnalyzer.Generated.ANTLR;
using Arc.Compiler.SyntaxAnalyzer.Models.Data.DataType;
using Arc.Compiler.SyntaxAnalyzer.Models.Function;

namespace Arc.Compiler.SyntaxAnalyzer.Models.Group;

public class ArcGroupConstructor : ArcGroupLifecycleFunction<ArcSourceCodeParser.Arc_group_constructorContext>
{
    public ArcGroupConstructor(ArcSourceCodeParser.Arc_group_constructorContext ctorContext)
    {
        Declarator = new ArcNamelessFunctionDeclarator
        {
            Arguments = ctorContext.arc_wrapped_arg_list()?
                .arc_arg_list()?
                .arc_data_declarator()
                .Select(p => new ArcFunctionArgument(p)) ?? [],
            ReturnType = new ArcDataType(ctorContext.arc_data_type())
        };
        Body = new ArcFunctionBody(ctorContext.arc_wrapped_function_body());
        Context = ctorContext;
    }
}
