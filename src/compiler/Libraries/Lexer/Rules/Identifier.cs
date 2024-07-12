using Arc.Compiler.Shared.Compilation;
using Arc.Compiler.Shared.LexicalAnalysis;
using Arc.Compiler.Shared.Utils;

namespace Arc.Compiler.Lexer.Rules
{
    internal class Identifier
    {
        public static SectionBuildResult<Token>? Build(SourceFile source, int baseIndex)
        {
            var identifier = TokenConstants.IdentifierRegex.Match(source.Content[baseIndex..]);
            if(identifier.Success)
            {
                var text = identifier.Groups[1].Value;
                return new(new Token(TokenType.Identifier, text, new TokenPosition(source, baseIndex, text.Length)), text.Length);
            }

            return null;
        }
    }
}
