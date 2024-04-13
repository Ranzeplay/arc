namespace Arc.Compiler.SyntaxAnalyzer.Models.Function
{
    internal abstract class ArcFunctionBase
    {
        public ArcFunctionDeclarator Declarator { get; set; }

        public ArcFunctionBody Body { get; set; }
    }
}
