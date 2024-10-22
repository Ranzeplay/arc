using Antlr4.Runtime;

namespace Arc.Compiler.SyntaxAnalyzer.Interfaces;

public interface IArcTraceable<out T> where T : ParserRuleContext
{
    public T Context { get; }
}