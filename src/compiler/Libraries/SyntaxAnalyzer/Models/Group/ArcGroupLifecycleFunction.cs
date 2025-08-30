using Antlr4.Runtime;
using Arc.Compiler.SyntaxAnalyzer.Interfaces;
using Arc.Compiler.SyntaxAnalyzer.Models.Function;

namespace Arc.Compiler.SyntaxAnalyzer.Models.Group;

public class ArcGroupLifecycleFunction<T> : ArcNamelessFunction<T> where T : ParserRuleContext, IArcAccessible
{
    public ArcGroupLifecycleStageType LifecycleStage { get; set; }
}
