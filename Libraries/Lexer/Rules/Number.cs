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
    public class Number
    {
        public static SectionBuildResult<Token>? Build(SourceFile source, int baseIndex)
        {
            var identifier = TokenConstants.NumberRegex.Match(source.Content[baseIndex..]);
            if (identifier.Success)
            {
                var text = identifier.Groups[1].Value;
                return new(new Token(TokenType.Number, text, new TokenPosition(source, baseIndex, text.Length)), text.Length);
            }

            return null;
        }
    }
}
