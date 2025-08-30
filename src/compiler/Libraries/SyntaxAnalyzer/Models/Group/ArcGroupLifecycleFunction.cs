using Antlr4.Runtime;
using Arc.Compiler.SyntaxAnalyzer.Interfaces;
using Arc.Compiler.SyntaxAnalyzer.Models.Components;
using Arc.Compiler.SyntaxAnalyzer.Models.Function;

namespace Arc.Compiler.SyntaxAnalyzer.Models.Group;

public class ArcGroupLifecycleFunction<T> : ArcNamelessFunction<T>, IArcAccessible
    where T : ParserRuleContext
{
    public ArcGroupLifecycleStageType LifecycleStage { get; set; }

    public ArcAccessibility Accessibility { get; set; }
}
