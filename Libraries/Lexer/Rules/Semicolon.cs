using Arc.Compiler.Shared.Compilation;
using Arc.Compiler.Shared.LexicalAnalysis;
using Arc.Compiler.Shared.Utils;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Arc.Compiler.Lexer.Rules
{
    public class Semicolon
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
