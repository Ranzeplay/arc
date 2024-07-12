using Arc.Compiler.Shared.LexicalAnalysis;

namespace Arc.Compiler.Shared.Parsing.Components.Expression
{

    using Relation = RelationOperatorType;

    internal class ExpressionUtils
    {
        public static Relation ComparePriority(OperatorToken a, OperatorToken b)
        {
            if (a.Type != b.Type)
            {
                if (TokenConstants.OperatorPriority[a.Type] < TokenConstants.OperatorPriority[b.Type])
                {
                    return Relation.Less;
                }
                else if (TokenConstants.OperatorPriority[a.Type] > TokenConstants.OperatorPriority[b.Type])
                {
                    return Relation.Greater;
                }
                else
                {
                    return Relation.Equal;
                }
            }
            else
            {
                if (a.Type == OperatorTokenType.Calculation)
                {
                    var aCalc = a.CalculationOperator!;
                    var bCalc = b.CalculationOperator!;

                    if (TokenConstants.CalculationOperatorPriority[aCalc] > TokenConstants.CalculationOperatorPriority[bCalc])
                    {
                        return Relation.Greater;
                    }
                    else if (TokenConstants.CalculationOperatorPriority[aCalc] < TokenConstants.CalculationOperatorPriority[bCalc])
                    {
                        return Relation.Less;
                    }
                    else
                    {
                        return Relation.Equal;
                    }
                }
                else
                {
                    return Relation.Equal;
                }
            }
        }
    }
}
