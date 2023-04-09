using Arc.Compiler.Parser.Builders.Blocks;
using Arc.Compiler.Parser.Builders.Components;
using Arc.Compiler.Parser.Builders.Components.Data;
using Arc.Compiler.Parser.Models;
using Arc.Compiler.Shared.LexicalAnalysis;
using Arc.Compiler.Shared.Parsing.Components.Group;
using Arc.Compiler.Shared.Utils;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Linq.Expressions;
using System.Text;
using System.Threading.Tasks;

namespace Arc.Compiler.Parser.Builders.Group
{
    internal class GroupDeclarationBuilder
    {
        public static SectionBuildResult<GroupBlock>? BuildGroupBlock(ExpressionBuildModel model)
        {
            // Check leading tokens
            if (model.Tokens[0].GetKeyword().GetValueOrDefault() != KeywordToken.Declare)
            {
                return null;
            }
            if (model.Tokens[1].GetKeyword().GetValueOrDefault() != KeywordToken.Group)
            {
                return null;
            }


            // Build identifier
            var identifier = IdentifierBuilder.Build(model.Tokens[2..]);
            if (identifier == null)
            {
                return null;
            }

            // Build inner properties
            var currentIndex = 2 + identifier.Length;
            if (model.Tokens[currentIndex].GetContainer().GetValueOrDefault() != ContainerToken.Brace)
            {
                return null;
            }

            var declaratorZone = Utils.PairContainer(model.Tokens[currentIndex..]);
            if (declaratorZone != null)
            {
                currentIndex += declaratorZone.Length;
                declaratorZone = declaratorZone[1..^1];

                var fields = new List<GroupField>();
                var methods = new List<GroupMethod>();
                var functions = new List<GroupFunction>();

                var zoneIndex = 0;
                var zoneModel = new ExpressionBuildModel(declaratorZone, model.DeclaredData, model.DeclaredFunctions);
                while (zoneIndex < declaratorZone.Length)
                {
                    var field = BuildGroupField(zoneModel.SkipTokens(zoneIndex));
                    if (field != null)
                    {
                        fields.Add(field.Section);
                        zoneIndex += field.Length;
                        continue;
                    }

                    var method = BuildGroupMethod(zoneModel.SkipTokens(zoneIndex));
                    if (method != null)
                    {
                        methods.Add(method.Section);
                        zoneIndex += method.Length;
                        continue;
                    }

                    var func = BuildGroupFunction(zoneModel.SkipTokens(zoneIndex));
                    if (func != null)
                    {
                        functions.Add(func.Section);
                        zoneIndex += func.Length;
                        continue;
                    }
                }

                return new(new(identifier.Section, fields.ToArray(), methods.ToArray(), functions.ToArray()), currentIndex);
            }

            return null;
        }

        public static SectionBuildResult<GroupField>? BuildGroupField(ExpressionBuildModel model)
        {
            // Check leading tokens
            if (model.Tokens[0].GetKeyword().GetValueOrDefault() != KeywordToken.Field)
            {
                return null;
            }

            // Build identifier
            var declarator = DataDeclaratorBuilder.Build(model.Tokens[1..]);
            if (declarator == null)
            {
                return null;
            }

            var currentIndex = 1 + declarator.Length;

            // Build getter and setter
            if (model.Tokens[currentIndex].GetContainer().GetValueOrDefault() != ContainerToken.Bracket)
            {
                return null;
            }

            var gsZone = Utils.PairContainer(model.Tokens[currentIndex..]);
            if (gsZone != null)
            {
                currentIndex += gsZone.Length;
                // Remove bracket pair
                gsZone = gsZone[1..^1];

                // Hard code
                var gsIndex = 0;
                var firstGS = BuildGSBlock(new(gsZone, model.DeclaredData, model.DeclaredFunctions));
                if (firstGS != null)
                {
                    gsIndex += firstGS.Length;
                }

                SectionBuildResult<GSBlockBuildResult>? secondGS = null;
                if (gsZone.Length > gsIndex)
                {
                    // Splited by comma token
                    if (gsZone[gsIndex].GetOperator()?.Type == OperatorTokenType.Comma)
                    {
                        gsIndex++;
                    }
                    else
                    {
                        throw new Exception("Getter and setter should be splited by a comma token");
                    }

                    secondGS = BuildGSBlock(new(gsZone[gsIndex..], model.DeclaredData, model.DeclaredFunctions));
                    if (secondGS != null)
                    {
                        gsIndex += secondGS.Length;
                    }
                }


                if (gsIndex != gsZone.Length)
                {
                    throw new Exception("Invalid GS tokens");
                }

                if (model.Tokens[currentIndex].TokenType == TokenType.Semicolon)
                {
                    // Check getter and setter
                    GSBlock? getter = null;
                    GSBlock? setter = null;

                    if (firstGS != null)
                    {
                        if (firstGS.Section.GSType == KeywordToken.Get)
                        {
                            if (getter == null)
                            {
                                getter = firstGS.Section.Block;
                            }
                            else
                            {
                                throw new Exception("Duplicated getter");
                            }
                        }
                        else if (firstGS.Section.GSType == KeywordToken.Set)
                        {
                            if (setter == null)
                            {
                                setter = firstGS.Section.Block;
                            }
                            else
                            {
                                throw new Exception("Duplicated setter");
                            }
                        }
                    }

                    if (secondGS != null)
                    {
                        if (secondGS.Section.GSType == KeywordToken.Get)
                        {
                            if (getter == null)
                            {
                                getter = secondGS.Section.Block;
                            }
                            else
                            {
                                throw new Exception("Duplicated getter");
                            }
                        }
                        else if (secondGS.Section.GSType == KeywordToken.Set)
                        {
                            if (setter == null)
                            {
                                setter = secondGS.Section.Block;
                            }
                            else
                            {
                                throw new Exception("Duplicated setter");
                            }
                        }
                    }


                    return new(new(declarator.Section, getter, setter), currentIndex + 1);
                }
            }

            return null;
        }

        private static SectionBuildResult<GSBlockBuildResult>? BuildGSBlock(ExpressionBuildModel model)
        {
            if (!(model.Tokens[0].GetKeyword().GetValueOrDefault() == KeywordToken.Get || model.Tokens[0].GetKeyword().GetValueOrDefault() == KeywordToken.Set))
            {
                return null;
            }

            if (model.Tokens.ElementAtOrDefault(1)?.GetContainer().GetValueOrDefault() == ContainerToken.Brace)
            {
                // With a customized getter ot setter
                var actionBlockZone = Utils.PairContainer(model.Tokens[1..]);
                if (actionBlockZone == null)
                {
                    return null;
                }

                // Remove pair container token
                actionBlockZone = actionBlockZone[1..^1];

                var actionBlock = ActionBlockBuilder.Build(new(actionBlockZone, model.DeclaredData, model.DeclaredFunctions));
                if (actionBlock != null)
                {
                    return new(new(model.Tokens[0].GetKeyword().GetValueOrDefault(), new(true, actionBlock.Section)), actionBlockZone.Length + 3);
                }

                return null;
            }
            else
            {
                // Doesn't customize getter or setter
                return new(new(model.Tokens[0].GetKeyword().GetValueOrDefault(), new(true, null)), 1);
            }
        }

        private static SectionBuildResult<GroupMethod>? BuildGroupMethod(ExpressionBuildModel model)
        {
            // Validate leading tokens
            if (model.Tokens[0].GetKeyword().GetValueOrDefault() != KeywordToken.Method)
            {
                return null;
            }

            var result = FunctionBlockBaseBuilder.Build(model.SkipTokens(1));
            if (result != null)
            {
                return new(new(result.Section.Declarator, result.Section.Actions), result.Length + 1);
            }

            return null;
        }

        private static SectionBuildResult<GroupFunction>? BuildGroupFunction(ExpressionBuildModel model)
        {
            // Validate leading tokens
            if (model.Tokens[0].GetKeyword().GetValueOrDefault() != KeywordToken.Func)
            {
                return null;
            }

            var result = FunctionBlockBaseBuilder.Build(model.SkipTokens(1));
            if (result != null)
            {
                return new(new(result.Section.Declarator, result.Section.Actions), result.Length + 1);
            }

            return null;
        }
    }
}
