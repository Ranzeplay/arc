using Arc.Compiler.Parser.Builders.Components;
using Arc.Compiler.Shared.LexicalAnalysis;
using Arc.Compiler.Shared.Parsing.AST;
using Arc.Compiler.Shared.Utils;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Arc.Compiler.Parser.Builders.Blocks
{
    public class LinkBlockBuilder
    {
        public static SectionBuildResult<LinkBlock>? Build(Token[] tokens)
        {
            // Check leading token
            if (tokens[0].GetKeyword().GetValueOrDefault() != KeywordToken.Link)
            {
                return null;
            }

            // Try to build with each type of link
            var identifierResult = IdentifierBuilder.Build(tokens[1..]);
            if (identifierResult != null)
            {
                if (tokens[identifierResult.Length + 1]?.TokenType == TokenType.Semicolon)
                {
                    return new(new(identifierResult.Section), identifierResult.Length + 2);
                }
            }

            var path = tokens[1].GetString();
            if (path != null)
            {
                if (tokens[2]?.TokenType == TokenType.Semicolon)
                {
                    return new(new(path), 3);
                }
            }

            return null;
        }
    }
}
