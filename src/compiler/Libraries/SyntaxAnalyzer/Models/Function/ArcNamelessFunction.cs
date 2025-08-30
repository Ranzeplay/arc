using Antlr4.Runtime;

namespace Arc.Compiler.SyntaxAnalyzer.Models.Function;

public class ArcNamelessFunction<T> : ArcFunctionBase<T, ArcFunctionMinimalDeclarator> 
    where T : ParserRuleContext
{
}
