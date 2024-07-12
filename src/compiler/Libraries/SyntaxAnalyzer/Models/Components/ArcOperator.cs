using Arc.Compiler.SyntaxAnalyzer.Models.Expression;

namespace Arc.Compiler.SyntaxAnalyzer.Models.Components
{
    internal class ArcOperator : ArcExpressionTermBase
    {
        public ArcOperatorType OperatorType { get; set; }

        public enum ArcOperatorType
        {
            Plus,
            Minus,
            Multiply,
            Divide,
            Modulo,
            And,
            Or,
            Not,
            ValueEqual,
            ReferenceEqual,
            ValueNotEqual,
            ReferenceNotEqual,
            GreaterThan,
            LessThan,
            GreaterThanOrEqual,
            LessThanOrEqual,
            PriorityUp,
            PriorityDown
        }
    }
}
