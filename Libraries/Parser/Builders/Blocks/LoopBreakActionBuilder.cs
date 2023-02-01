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
    public class LoopBreakActionBuilder
    {
        public static SectionBuildResult<ASTNode>? Build(Token[] tokens)
        {
            var leadingKeyword = tokens[0].GetKeyword().GetValueOrDefault();
            if (leadingKeyword == KeywordToken.Break && tokens[1].TokenType == TokenType.Semicolon)
            {
                return new(new(ASTNodeType.LoopBreak), 2);
            }
            else
            {
                return null;
            }
        }
    }
}
