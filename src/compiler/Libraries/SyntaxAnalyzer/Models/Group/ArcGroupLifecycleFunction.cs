using Antlr4.Runtime;
using Arc.Compiler.SyntaxAnalyzer.Generated.ANTLR;
using Arc.Compiler.SyntaxAnalyzer.Interfaces;
using Arc.Compiler.SyntaxAnalyzer.Models.Components;
using Arc.Compiler.SyntaxAnalyzer.Models.Data.DataType;
using Arc.Compiler.SyntaxAnalyzer.Models.Function;

namespace Arc.Compiler.SyntaxAnalyzer.Models.Group;

public class ArcGroupLifecycleFunction : ArcNamelessFunction<ArcSourceCodeParser.Arc_group_lifecycle_functionContext>, IArcAccessible
{
    public ArcGroupLifecycleStageType LifecycleStage { get; set; }

    public ArcAccessibility Accessibility { get; set; }
    
    public ArcGroupLifecycleFunction(ArcSourceCodeParser.Arc_group_lifecycle_functionContext context)
    {
        Declarator = new ArcNamelessFunctionDeclarator
        {
            Arguments = context.arc_wrapped_arg_list()?
                .arc_arg_list()?
                .arc_data_declarator()
                .Select(p => new ArcFunctionArgument(p)) ?? [],
            ReturnType = new ArcDataType(context.arc_data_type())
        };
        Body = new ArcFunctionBody(context.arc_wrapped_function_body());

        var stage = context.arc_group_lifecycle_keyword();
        if (stage.KW_CONSTRUCTOR() != null) LifecycleStage = ArcGroupLifecycleStageType.Construction;
        else if (stage.KW_DESTRUCTOR() != null) LifecycleStage = ArcGroupLifecycleStageType.Destruction;
        else if (stage.KW_CLONE() != null) LifecycleStage = ArcGroupLifecycleStageType.DeepCopy;
        else if (stage.KW_VALUE() != null) LifecycleStage = ArcGroupLifecycleStageType.ShallowCopy;
        else throw new InvalidOperationException("Invalid lifecycle stage");
        
        Context = context;
    }
    
}
