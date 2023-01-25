using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Arc.Compiler.Shared.LexicalAnalysis
{
    public class Token
    {
        public TokenType TokenType { get; }

        public TokenPosition Position { get; }

        private object? Target { get; }

        public Token(TokenType tokenType, string target, TokenPosition position)
        {
            TokenType = tokenType;
            Position = position;
            Target = target;
        }

        public Token(TokenType tokenType, TokenPosition position)
        {
            TokenType = tokenType;
            Position = position;
        }

        public Token(OperatorToken op, TokenPosition position)
        {
            TokenType = TokenType.Operator;
            Position = position;
            Target = op;
        }

        public Token(ContainerToken container, TokenPosition position)
        {
            TokenType = TokenType.Container;
            Position = position;
            Target = container;
        }

        public Token(KeywordToken keyword, TokenPosition position)
        {
            TokenType = TokenType.Keyword;
            Position = position;
            Target = keyword;
        }

        public string? GetIdentifier()
        {
            if (TokenType == TokenType.Identifier)
            {
                return Target as string;
            }
            else
            {
                return null;
            }
        }

        public string? GetNumber()
        {
            if (TokenType == TokenType.Number)
            {
                return Target as string;
            }
            else
            {
                return null;
            }
        }

        public string? GetString()
        {
            if (TokenType == TokenType.String)
            {
                return Target as string;
            }
            else
            {
                return null;
            }
        }

        public string? GetComment()
        {
            if (TokenType == TokenType.Comment)
            {
                return Target as string;
            }
            else
            {
                return null;
            }
        }

        public KeywordToken? GetKeyword()
        {
            if (TokenType == TokenType.Keyword && Target is not null)
            {
                return (KeywordToken)Target;
            }
            else
            {
                return null;
            }
        }

        public OperatorToken? GetOperator()
        {
            if (TokenType == TokenType.Operator)
            {
                return Target as OperatorToken;
            }
            else
            {
                return null;
            }
        }

        public ContainerToken? GetContainer()
        {
            if (TokenType == TokenType.Container && Target is not null)
            {
                return (ContainerToken)Target;
            }
            else
            {
                return null;
            }
        }

        public bool IsSemicolon() => TokenType == TokenType.Semicolon;

        public bool IsWhitespace() => TokenType == TokenType.Whitespace;
    }
}
