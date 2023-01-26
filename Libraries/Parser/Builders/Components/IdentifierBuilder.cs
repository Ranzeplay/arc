using Arc.Compiler.Shared.LexicalAnalysis;
using Arc.Compiler.Shared.Parsing.Components;
using Arc.Compiler.Shared.Utils;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Arc.Compiler.Parser.Builders.Components
{
    public class IdentifierBuilder
    {
        public static SectionBuildResult<Identifier>? Build(Token[] tokens)
        {
            int index;
            var scope = new List<string>();
            // Tokens at odd(1, 3, 5, 7...) indexes are Operator::Scope
            // Tokens at even(0, 2, 4, 6...) indexes are Identifiers
            for(index = 0; index < tokens.Length; index++)
            {
                if(index % 2 == 0)
                {
                    var current = tokens[index].GetIdentifier();
                    if (current is not null)
                    {
                        scope.Add(current);
                    } else
                    {
                        break;
                    }
                }
                else
                {
                    bool match = false;
                    var op = tokens[index].GetOperator();
                    if(op is not null)
                    {
                        if(op.Type == OperatorTokenType.Scope)
                        {
                            match = true;
                        }
                    }

                    if (!match)
                    {
                        break;
                    }
                }
            }

            if(scope.Count > 0)
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
