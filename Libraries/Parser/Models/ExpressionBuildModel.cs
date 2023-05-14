using Arc.Compiler.Shared.LexicalAnalysis;
using Arc.Compiler.Shared.Parsing.Components.Data;
using Arc.Compiler.Shared.Parsing.Components.Function;

namespace Arc.Compiler.Parser.Models
{
    public record ExpressionBuildModel(Token[] Tokens, DataDeclarator[] DeclaredData, FunctionDeclarator[] DeclaredFunctions)
    {
        public ExpressionBuildModel SkipTokens(int count)
        {
            return new(Tokens[count..], DeclaredData, DeclaredFunctions);
        }
    }
}
