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
    public class Container
    {
        public static SectionBuildResult<Token>? Build(SourceFile source, int baseIndex)
        {
            var result = TokenConstants.ContainerMappings
                .FirstOrDefault(k => source.Content[baseIndex..].StartsWith(k.Value), new(ContainerToken.Invalid, string.Empty));

            if (result.Key == ContainerToken.Invalid)
            {
                return null;
            }
            else
            {
                return new(new Token(result.Key, new TokenPosition(source, baseIndex, result.Value.Length)), result.Value.Length);
            }
        }
    }
}
