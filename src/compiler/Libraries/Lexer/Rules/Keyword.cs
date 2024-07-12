using Arc.Compiler.Shared.Compilation;
using Arc.Compiler.Shared.LexicalAnalysis;
using Arc.Compiler.Shared.Utils;

namespace Arc.Compiler.Lexer.Rules
{
    internal class Keyword
    {
        public static SectionBuildResult<Token>? Build(SourceFile source, int baseIndex)
        {
            var identifier = Identifier.Build(source, baseIndex);
            if (identifier != null)
            {
                var result = TokenConstants.KeywordMappings
                    .FirstOrDefault(k => identifier.Section.GetIdentifier() == k.Value, new(KeywordToken.Invalid, string.Empty));

                if (result.Key != KeywordToken.Invalid)
                {
                    return new(new Token(result.Key, new TokenPosition(source, baseIndex, result.Value.Length)), result.Value.Length);
                }
            }

            return null;
        }
    }
}
