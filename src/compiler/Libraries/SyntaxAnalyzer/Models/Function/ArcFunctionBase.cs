using Antlr4.Runtime;
using Arc.Compiler.SyntaxAnalyzer.Interfaces;

namespace Arc.Compiler.SyntaxAnalyzer.Models.Function
{
    public abstract class ArcFunctionBase<T>: IArcTraceable<T> where T : ParserRuleContext
    {
        public ArcFunctionDeclarator Declarator { get; set; }

        public ArcFunctionBody Body { get; set; }

        public T Context { get; internal init; }
    }
}
