using Arc.Compiler.Lexer.Rules;
using Arc.Compiler.Shared.Compilation;
using Arc.Compiler.Shared.LexicalAnalysis;

namespace Arc.Compiler.Lexer
{
    public class Tokenizer
    {
        public static TokenizedFile Tokenize(SourceFile source, bool trimWhitespace)
        {
            var tokens = new List<Token>();

            int currentIndex = 0;
            while (currentIndex < source.Content.Length)
            {
                var comment = Comment.Build(source, currentIndex);
                if (comment is not null)
                {
                    if (comment.Section is not null)
                    {
                        tokens.Add(comment.Section);
                        currentIndex += comment.Length;
                    }
                }

                var semicolon = Semicolon.Build(source, currentIndex);
                if (semicolon is not null)
                {
                    if (semicolon.Section is not null)
                    {
                        tokens.Add(semicolon.Section);
                        currentIndex += semicolon.Length;
                    }
                }

                var keyword = Keyword.Build(source, currentIndex);
                if (keyword is not null)
                {
                    if (keyword.Section is not null)
                    {
                        tokens.Add(keyword.Section);
                        currentIndex += keyword.Length;
                    }
                }

                var identifier = Identifier.Build(source, currentIndex);
                if (identifier is not null)
                {
                    if (identifier.Section is not null)
                    {
                        tokens.Add(identifier.Section);
                        currentIndex += identifier.Length;
                    }
                }

                var op = Operator.Build(source, currentIndex);
                if (op is not null)
                {
                    if (op.Section is not null)
                    {
                        tokens.Add(op.Section);
                        currentIndex += op.Length;
                    }
                }

                var number = Number.Build(source, currentIndex);
                if (number is not null)
                {
                    if (number.Section is not null)
                    {
                        tokens.Add(number.Section);
                        currentIndex += number.Length;
                    }
                }

                var str = Str.Build(source, currentIndex);
                if (str is not null)
                {
                    if (str.Section is not null)
                    {
                        tokens.Add(str.Section);
                        currentIndex += str.Length;
                    }
                }

                var container = Container.Build(source, currentIndex);
                if (container is not null)
                {
                    if (container.Section is not null)
                    {
                        tokens.Add(container.Section);
                        currentIndex += container.Length;
                    }
                }

                var whitespace = Whitespace.Build(source, currentIndex);
                if (whitespace is not null)
                {
                    if (whitespace.Section is not null)
                    {
                        if (!trimWhitespace)
                        {
                            tokens.Add(whitespace.Section);
                        }

                        currentIndex += whitespace.Length;
                    }
                }

            }

            return new(source, tokens.ToArray());
        }
    }
}