using Arc.Compiler.Shared.LexicalAnalysis;
using Arc.Compiler.Shared.Parsing.AST;
using Arc.Compiler.Shared.Utils;

namespace Arc.Compiler.Parser.Builders.Blocks
{
    internal class LoopBreakActionBuilder
    {
        public static SectionBuildResult<ASTNode>? Build(Token[] tokens)
        {
            var leadingKeyword = tokens[0].GetKeyword().GetValueOrDefault();
            return leadingKeyword == KeywordToken.Break && tokens[1].TokenType == TokenType.Semicolon
                ? (new(new(ASTNodeType.LoopBreak), 2))
                : null;
        }
    }
}
