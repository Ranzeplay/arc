using Antlr4.Runtime;

namespace Arc.Compiler.SyntaxAnalyzer.Models.Function
{
    internal abstract class ArcFunctionBase<T>: IArcTraceable<T> where T : ParserRuleContext
    {
        public ArcFunctionDeclarator Declarator { get; set; }

        public ArcFunctionBody Body { get; set; }
        
        public T Context { get; internal init; }
    }
}
