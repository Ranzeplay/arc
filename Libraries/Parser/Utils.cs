using Arc.Compiler.Shared.LexicalAnalysis;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Arc.Compiler.Parser
{
    internal class Utils
    {
        /// <summary>
        /// Get the token contained from the first token if it is a container token.
        /// </summary>
        /// <param name="tokens">Token array that the first token is a container token.</param>
        /// <returns>The return array contains the container on both sides</returns>
        internal static Token[]? PairContainer(Token[] tokens)
        {
            var container = tokens[0].GetContainer().GetValueOrDefault();
            if (container != ContainerToken.Invalid)
            {
                var antiContainer = GetAntiContainer(container);
                int containerLevel = 0;
                for (int i = 0; i < tokens.Length; i++)
                {
                    var currentContainer = tokens[i].GetContainer().GetValueOrDefault();
                    if (currentContainer == container)
                    {
                        containerLevel++;
                    }
                    else if (currentContainer == antiContainer)
                    {
                        containerLevel--;
                    }

                    // If this is the top layer from the start of the array
                    // Return at first couldn't happen because containerLevel had been added by 1 at the beginning
                    if (containerLevel == 0)
                    {
                        return tokens[..(i + 1)];
                    }
                }
            }

            return null;
        }

        internal static ContainerToken GetAntiContainer(ContainerToken container)
        {
            ContainerToken antiContainer;
            switch (container)
            {
                case ContainerToken.Brace:
                    antiContainer = ContainerToken.AntiBrace;
                    break;
                case ContainerToken.Bracket:
                    antiContainer = ContainerToken.AntiBracket;
                    break;
                case ContainerToken.Index:
                    antiContainer = ContainerToken.AntiIndex;
                    break;
                default:
                    antiContainer = ContainerToken.Invalid;
                    break;
            }

            return antiContainer;
        }

        internal static List<List<Token>> SplitCommaExpression(Token[] tokens)
        {
            if (tokens.Length == 0)
            {
                return new List<List<Token>>();
            }

            var result = new List<List<Token>>
            {
                // With 1 expression at first
                new()
            };

            foreach (var token in tokens)
            {
                var op = token.GetOperator();
                if (op is not null)
                {
                    if (op.Type == OperatorTokenType.Comma)
                    {
                        // Add new list when encountered comma operator token
                        result.Add(new());
                        continue;
                    }
                }

                // Otherwise, add it into last list
                result[^1].Add(token);
            }

            return result;
        }

        internal static int GetNextSemicolonPos(Token[] tokens, int defaultPos = -1)
        {
            var first = tokens.FirstOrDefault(t => t.TokenType == TokenType.Semicolon);
            if (first != null)
            {
                return Array.IndexOf(tokens, first);
            }
            else
            {
                return defaultPos;
            }
        }
    }
}
