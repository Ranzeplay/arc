using Arc.Compiler.Parser.Builders.Blocks;
using Arc.Compiler.Parser.Builders.Components;
using Arc.Compiler.Parser.Builders.Group;
using Arc.Compiler.Parser.Models;
using Arc.Compiler.Shared.Compilation;
using Arc.Compiler.Shared.LexicalAnalysis;
using Arc.Compiler.Shared.Parsing.AST;
using Arc.Compiler.Shared.Parsing.Components.Function;
using Arc.Compiler.Shared.Parsing.Components.Group;

namespace Arc.Compiler.Parser
{
    public class Pipeline
    {
        public static PartialParsingResult? BuildAll(ExpressionBuildModel model)
        {
            var links = new List<LinkBlock>();
            var functions = new List<FunctionBlock>();
            var groups = new List<GroupBlock>();

            var currentIndex = 0;
            while (currentIndex < model.Tokens.Length)
            {
                var link = LinkBlockBuilder.Build(model.Tokens[currentIndex..]);
                if (link != null)
                {
                    links.Add(link.Section);
                    currentIndex += link.Length;
                    continue;
                }

                var group = GroupDeclarationBuilder.BuildGroupBlock(model.SkipTokens(currentIndex));
                if (group != null)
                {
                    groups.Add(group.Section);
                    currentIndex += group.Length;
                    continue;
                }

                var func = FunctionBlockBuilder.Build(model.SkipTokens(currentIndex));
                if (func != null)
                {
                    functions.Add(func.Section);
                    currentIndex += func.Length;
                    continue;
                }

                throw new Exception("Invalid root node (only accepts Link, Group and Func)");
            }

            return new(links, groups, functions);
        }

        public static IEnumerable<FunctionDeclarator> BuildAllFunctionDeclarators(ExpressionBuildModel model)
        {
            var tokens = model.Tokens.ToList();
            var declarators = new List<FunctionDeclarator>();

            var index = 0;
            while (index < tokens.Count)
            {
                // Find next function declarator
                var nextLeadingIndex = tokens.FindIndex(index, t => t.GetKeyword() == KeywordToken.Declare);
                if (nextLeadingIndex < 0)
                {
                    break;
                }
                index = nextLeadingIndex;

                if (tokens[index + 1].GetKeyword() == KeywordToken.Func)
                {
                    var decl = FunctionBlockBaseBuilder.BuildFunctionDeclarator(model.SkipTokens(index + 2));
                    if (decl != null)
                    {
                        index += decl.Length;
                        declarators.Add(decl.Section);
                    }
                }
                else
                {
                    index += 2;
                }
            }

            return declarators;
        }
    }
}
