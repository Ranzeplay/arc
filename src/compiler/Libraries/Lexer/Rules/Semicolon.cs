using Arc.Compiler.Shared.Compilation;
using Arc.Compiler.Shared.LexicalAnalysis;
using Arc.Compiler.Shared.Utils;

namespace Arc.Compiler.Lexer.Rules
{
    internal class Semicolon
    {
        public static SectionBuildResult<Token>? Build(SourceFile source, int baseIndex)
        {
            if (source.Content[baseIndex] == TokenConstants.SemicolonToken)
            {
                return new(new Token(TokenType.Semicolon, new TokenPosition(source, baseIndex, 1)), 1);
            }
            else
            {
                return null;
            }
        }
    }
}
