using Arc.Compiler.SyntaxAnalyzer.Models.Components;
using Arc.Compiler.SyntaxAnalyzer.Models.Data;

namespace Arc.Compiler.SyntaxAnalyzer.Models.Expression
{
    public class ArcExpressionTerm
    {
        public enum ExpressionTermType
        {
            DataValue,
            Operator,
        }

        public ExpressionTermType Type { get; set; }

        public ArcDataValue? DataValue { get; set; }

        public ArcOperator? Operator { get; set; }

        public bool IsOperator => Type == ExpressionTermType.Operator;

        public ArcExpressionTerm(ArcDataValue value)
        {
            Type = ExpressionTermType.DataValue;
            DataValue = value;
        }

        public ArcExpressionTerm(ArcOperator op)
        {
            Type = ExpressionTermType.Operator;
            Operator = op;
        }
    }
}
