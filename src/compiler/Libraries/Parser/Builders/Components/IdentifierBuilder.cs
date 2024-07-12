using Arc.Compiler.Shared.LexicalAnalysis;
using Arc.Compiler.Shared.Parsing.Components;
using Arc.Compiler.Shared.Utils;

namespace Arc.Compiler.Parser.Builders.Components
{
    internal class IdentifierBuilder
    {
        public static SectionBuildResult<Identifier>? Build(Token[] tokens)
        {
            int index;
            var scope = new List<string>();
            // Tokens at odd(1, 3, 5, 7...) indexes are Operator::Scope
            // Tokens at even(0, 2, 4, 6...) indexes are Identifiers
            for (index = 0; index < tokens.Length; index++)
            {
                if (index % 2 == 0)
                {
                    var current = tokens[index].GetIdentifier();
                    if (current is not null)
                    {
                        scope.Add(current);
                    }
                    else
                    {
                        break;
                    }
                }
                else
                {
                    if (tokens[index].GetOperator()?.Type != OperatorTokenType.Scope)
                    {
                        break;
                    }
                }
            }

            // Name and scope are in the Scope array for now.
            // If Scope is empty, then they are even no valid identifier.
            if (scope.Count > 0)
            {
                return new(new(scope.SkipLast(1).ToArray(), scope.Last()), index);
            }
            else
            {
                return null;
            }
        }
    }
}
