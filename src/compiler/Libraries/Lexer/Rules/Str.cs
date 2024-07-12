using Arc.Compiler.Shared.Compilation;
using Arc.Compiler.Shared.LexicalAnalysis;
using Arc.Compiler.Shared.Utils;

namespace Arc.Compiler.Lexer.Rules
{
    internal class Str
    {
        public static SectionBuildResult<Token>? Build(SourceFile source, int baseIndex)
        {
            if (source.Content[baseIndex] == TokenConstants.QuoteContainer)
            {
                int endPos;
                for (endPos = baseIndex + 1; endPos < source.Content.Length; endPos++)
                {
                    if (source.Content[endPos] == '\\')
                    {
                        endPos += 1;
                    }
                    else if (source.Content[endPos] == TokenConstants.QuoteContainer)
                    {
                        break;
                    }
                }

                var len = endPos - baseIndex + 1;
                return new(new Token(TokenType.String, source.Content.Substring(baseIndex, len), new TokenPosition(source, baseIndex, len)), len);
            }

            return null;
        }
    }
}
