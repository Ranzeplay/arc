using Arc.Compiler.Parser.Builders.Blocks;
using Arc.Compiler.Parser.Builders.Components.Data;
using Arc.Compiler.Parser.Models;
using Arc.Compiler.Shared.LexicalAnalysis;
using Arc.Compiler.Shared.Parsing.AST;
using Arc.Compiler.Shared.Parsing.Components.Function;
using Arc.Compiler.Shared.Utils;

namespace Arc.Compiler.Parser.Builders.Components
{
    internal class FunctionBlockBaseBuilder
    {
        public static SectionBuildResult<FunctionBlock>? Build(ExpressionBuildModel model)
        {
            var declaratorResult = BuildFunctionDeclarator(model);
            if (declaratorResult != null)
            {
                // Build ActionBlock
                var currentIndex = declaratorResult.Length;
                #region FunctionBody
                if (model.Tokens[currentIndex].GetContainer().GetValueOrDefault() != ContainerToken.Brace)
                {
                    return null;
                }

                var actionBlockZone = Utils.PairContainer(model.Tokens[currentIndex..]);
                if (actionBlockZone == null)
                {
                    return null;
                }

                currentIndex += actionBlockZone.Length;
                actionBlockZone = actionBlockZone[1..^1];
                var block = ActionBlockBuilder.Build(new(actionBlockZone, model.DeclaredData, model.DeclaredFunctions));

                if (block != null)
                {
                    return new(new(declaratorResult.Section, block.Section), currentIndex);
                }
                #endregion
            }

            return null;
        }

        public static SectionBuildResult<FunctionDeclarator>? BuildFunctionDeclarator(ExpressionBuildModel model)
        {
            // Build function identifier
            #region FunctionIdentifier
            var identifierResult = IdentifierBuilder.Build(model.Tokens);
            if (identifierResult == null)
            {
                return null;
            }
            #endregion

            // Build parameters
            #region FunctionParameters
            var currentIndex = identifierResult.Length;
            if (model.Tokens[currentIndex].GetContainer().GetValueOrDefault() != ContainerToken.Bracket)
            {
                return null;
            }

            var parameterZone = Utils.PairContainer(model.Tokens[currentIndex..]);
            if (parameterZone == null)
            {
                return null;
            }

            currentIndex += parameterZone.Length;
            parameterZone = parameterZone.Skip(1).SkipLast(1).ToArray();
            var rawParameters = Utils.SplitCommaExpression(parameterZone);

            var paramList = new List<FunctionParameter>();
            foreach (var rawParam in rawParameters)
            {
                var paramDeclarator = DataDeclaratorBuilder.Build(rawParam.ToArray());
                if (paramDeclarator != null)
                {
                    paramList.Add(new(paramDeclarator.Section.DataType, paramDeclarator.Section.Identifier, paramDeclarator.Section.IsConstant));
                }
                else
                {
                    throw new ArgumentException("Invalid data declarator");
                }
            }
            #endregion

            // Build return value
            #region FunctionReturnValue
            if (model.Tokens[currentIndex].GetContainer().GetValueOrDefault() != ContainerToken.Index)
            {
                return null;
            }

            var returnValueZone = Utils.PairContainer(model.Tokens[currentIndex..]);
            if (returnValueZone == null)
            {
                return null;
            }

            currentIndex += returnValueZone.Length;
            returnValueZone = returnValueZone.Skip(1).SkipLast(1).ToArray();

            var returnDataTypeResult = DataTypeBuilder.Build(returnValueZone);
            if (returnDataTypeResult == null)
            {
                return null;
            }
            #endregion

            return new(new(identifierResult.Section, returnDataTypeResult.Section, paramList.ToArray()), currentIndex);
        }
    }
}
