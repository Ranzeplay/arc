using Arc.Compiler.Shared.LexicalAnalysis;
using Arc.Compiler.Shared.Parsing.Components.Data;
using Arc.Compiler.Shared.Utils;

namespace Arc.Compiler.Parser.Builders.Components.Data
{
    internal class DataTypeBuilder
    {
        public static SectionBuildResult<DataType>? Build(Token[] tokens)
        {
            // Keyword such as number, str and bool are types
            if (tokens[0].TokenType == TokenType.Keyword)
            {
                var isArray = tokens.Length > 1 ? CheckArray(tokens[1..]) : false;
                var keyword = tokens[0].GetKeyword();
                if (keyword != null)
                {
                    if (keyword == KeywordToken.String)
                    {
                        return new(new(new(Array.Empty<string>(), "string"), isArray), isArray ? 3 : 1);
                    }
                    else if (keyword == KeywordToken.Number)
                    {
                        return new(new(new(Array.Empty<string>(), "number"), isArray), isArray ? 3 : 1);
                    }
                    else if (keyword == KeywordToken.Bool)
                    {
                        return new(new(new(Array.Empty<string>(), "bool"), isArray), isArray ? 3 : 1);
                    }
                    else
                    {
                        return null;
                    }
                }
            }

            var typeNameSection = IdentifierBuilder.Build(tokens);
            if (typeNameSection is not null)
            {
                var currentIndex = typeNameSection.Length;

                // Check if it is an array declarator
                bool isArray = CheckArray(tokens[currentIndex..]);
                if (isArray)
                {
                    currentIndex += 2;
                }

                return new(new(typeNameSection.Section, isArray), currentIndex);
            }

            return null;
        }

        private static bool CheckArray(Token[] tokens)
        {
            if (tokens.Length < 2)
            {
                return false;
            }
            else
            {
                return tokens[0].GetContainer().GetValueOrDefault() == ContainerToken.Index
                && tokens[1].GetContainer().GetValueOrDefault() == ContainerToken.AntiIndex;
            }
        }
    }
}
