using Arc.Compiler.Shared.Compilation;
using Arc.Compiler.Shared.LexicalAnalysis;
using Arc.Compiler.Shared.Utils;

namespace Arc.Compiler.Lexer.Rules
{
    internal class Comment
    {
        /// <summary>
        /// Try to match comment token from source code.
        /// </summary>
        /// <param name="source">The source code required to match.</param>
        /// <param name="baseIndex">The string index that starts to match the comment.</param>
        /// <returns>
        /// The content of the token contains the leading sequence "//", so does the length.
        /// If returns null, means that the source code doesn't met the requirement.
        /// </returns>
        public static SectionBuildResult<Token>? Build(SourceFile source, int baseIndex)
        {
            if (source.Content[baseIndex..].StartsWith(TokenConstants.CommentLeadingSequence))
            {
                int endPos = source.Content.IndexOf('\n', baseIndex);
                if(endPos == -1)
                {
                    endPos = source.Content.Length;
                }

                int len = endPos - baseIndex;
                if (len > 0)
                {
                    return new(new Token(TokenType.Comment, new TokenPosition(source, baseIndex, len)), len);
                }
            }

            return null;
        }
    }
}
