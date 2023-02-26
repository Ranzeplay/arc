using Arc.Compiler.Shared.Compilation;
using Arc.Compiler.Shared.LexicalAnalysis;
using Arc.Compiler.Shared.Utils;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Arc.Compiler.Lexer.Rules
{
    internal class Operator
    {
        public static SectionBuildResult<Token>? Build(SourceFile source, int baseIndex)
        {
            // Try build calculation operator
            var calcResult = TokenConstants.CalculationOperatorMappings
                .FirstOrDefault(k => source.Content[baseIndex..].StartsWith(k.Value), new(CalculationOperatorType.Invalid, string.Empty));

            if (calcResult.Key != CalculationOperatorType.Invalid)
            {
                return new(new Token(new OperatorToken(calcResult.Key), new TokenPosition(source, baseIndex, calcResult.Value.Length)), calcResult.Value.Length);
            }

            // Try build relation operator
            var relationResult = TokenConstants.RelationOperatorMappings
                .FirstOrDefault(k => source.Content[baseIndex..].StartsWith(k.Value), new(RelationOperatorType.Invalid, string.Empty));

            if (relationResult.Key != RelationOperatorType.Invalid)
            {
                return new(new Token(new OperatorToken(relationResult.Key), new TokenPosition(source, baseIndex, relationResult.Value.Length)), relationResult.Value.Length);
            }

            // Try build logical operator
            var logicalResult = TokenConstants.LogicalOperatorMappings
                .FirstOrDefault(k => source.Content[baseIndex..].StartsWith(k.Value), new(LogicalOperatorType.Invalid, string.Empty));

            if (logicalResult.Key != LogicalOperatorType.Invalid)
            {
                return new(new Token(new OperatorToken(logicalResult.Key), new TokenPosition(source, baseIndex, logicalResult.Value.Length)), logicalResult.Value.Length);
            }

            // Maybe it is one of the root operators
            var rootResult = TokenConstants.RootOperatorMappings
                .FirstOrDefault(k => source.Content[baseIndex..].StartsWith(k.Value), new(OperatorTokenType.Invalid, string.Empty));

            if (rootResult.Key != OperatorTokenType.Invalid)
            {
                return new(new Token(new OperatorToken(rootResult.Key), new TokenPosition(source, baseIndex, rootResult.Value.Length)), rootResult.Value.Length);
            }

            // It doesn't belongs to anyone
            return null;
        }
    }
}
