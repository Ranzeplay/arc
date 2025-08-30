using Arc.Compiler.SyntaxAnalyzer.Generated.ANTLR;
using Arc.Compiler.SyntaxAnalyzer.Models.Data.DataType;
using Arc.Compiler.SyntaxAnalyzer.Models.Function;

namespace Arc.Compiler.SyntaxAnalyzer.Models.Group;

public class ArcGroupDestructor : ArcGroupLifecycleFunction<ArcSourceCodeParser.Arc_group_destructorContext>
{
    public ArcGroupDestructor(ArcSourceCodeParser.Arc_group_destructorContext dtorContext)
    {
        Declarator = new ArcNamelessFunctionDeclarator
        {
            Arguments = [],
            ReturnType = new ArcDataType(dtorContext.arc_data_type())
        };
        Body = new ArcFunctionBody(dtorContext.arc_wrapped_function_body());
        Context = dtorContext;
    }
}
