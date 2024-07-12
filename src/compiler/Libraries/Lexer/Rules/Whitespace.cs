using Arc.Compiler.Shared.Compilation;
using Arc.Compiler.Shared.LexicalAnalysis;
using Arc.Compiler.Shared.Utils;

namespace Arc.Compiler.Lexer.Rules
{
    internal class Whitespace
    {
        public static SectionBuildResult<Token>? Build(SourceFile source, int baseIndex)
        {
            int remain = source.Content[baseIndex..].TrimStart().Length;
            int len = source.Content.Length - remain - baseIndex;
            if (len > 0)
            {
                return new(new Token(TokenType.Whitespace, new TokenPosition(source, baseIndex, len)), len);
            }
            else
            {
                return null;
            }
        }
    }
}
