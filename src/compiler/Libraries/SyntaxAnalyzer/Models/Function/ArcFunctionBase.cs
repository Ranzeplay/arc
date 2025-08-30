using Antlr4.Runtime;
using Arc.Compiler.SyntaxAnalyzer.Interfaces;
using Arc.Compiler.SyntaxAnalyzer.Models.Data.DataType;
using Arc.Compiler.SyntaxAnalyzer.Models.Identifier;

namespace Arc.Compiler.SyntaxAnalyzer.Models.Function
{
    public abstract class ArcFunctionBase<TSyntax, TDeclarator> : IArcTraceable<TSyntax> 
        where TSyntax : ParserRuleContext 
        where TDeclarator : ArcFunctionMinimalDeclarator
    {
        public TDeclarator Declarator { get; set; }

        public ArcFunctionBody Body { get; set; }

        public TSyntax Context { get; internal init; }
    }
}
