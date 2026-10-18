namespace Arc.Compiler.SyntaxAnalyzer.Models.Components;

public static class ArcOperatorExtensions
{
    extension(ArcOperator op)
    {
        public bool IsUnary()
        {
            switch (op)
            {
                case ArcOperator.Plus:
                case ArcOperator.Minus:
                case ArcOperator.Multiply:
                case ArcOperator.Divide:
                case ArcOperator.Modulus:
                case ArcOperator.BitwiseAnd:
                case ArcOperator.BitwiseOr:
                case ArcOperator.BitwiseXor:
                case ArcOperator.LogicalAnd:
                case ArcOperator.LogicalOr:
                case ArcOperator.ObjectEquals:
                case ArcOperator.ReferenceEquals:
                case ArcOperator.ObjectNotEquals:
                case ArcOperator.ReferenceNotEquals:
                case ArcOperator.LessThan:
                case ArcOperator.LessThanOrEqual:
                case ArcOperator.GreaterThan:
                case ArcOperator.GreaterThanOrEqual:
                case ArcOperator.ShiftLeft:
                case ArcOperator.ShiftRight:
                case ArcOperator.IncreaseBy:
                case ArcOperator.DecreaseBy:
                case ArcOperator.MultiplyBy:
                case ArcOperator.DivideBy:
                case ArcOperator.ModulusBy:
                    return false;
                case ArcOperator.SelfIncrement:
                case ArcOperator.SelfDecrement:
                case ArcOperator.BitwiseNot:
                case ArcOperator.LogicalNot:
                    return true;
            
                default:
                    throw new ArgumentOutOfRangeException(nameof(op), op, null);
            }
        }

        public bool IsBinary()
        {
            switch (op)
            {
                case ArcOperator.Plus:
                case ArcOperator.Minus:
                case ArcOperator.Multiply:
                case ArcOperator.Divide:
                case ArcOperator.Modulus:
                case ArcOperator.BitwiseAnd:
                case ArcOperator.BitwiseOr:
                case ArcOperator.BitwiseXor:
                case ArcOperator.LogicalAnd:
                case ArcOperator.LogicalOr:
                case ArcOperator.ObjectEquals:
                case ArcOperator.ReferenceEquals:
                case ArcOperator.ObjectNotEquals:
                case ArcOperator.ReferenceNotEquals:
                case ArcOperator.LessThan:
                case ArcOperator.LessThanOrEqual:
                case ArcOperator.GreaterThan:
                case ArcOperator.GreaterThanOrEqual:
                case ArcOperator.ShiftLeft:
                case ArcOperator.ShiftRight:
                    return true;
                case ArcOperator.SelfIncrement:
                case ArcOperator.SelfDecrement:
                case ArcOperator.BitwiseNot:
                case ArcOperator.LogicalNot:
                case ArcOperator.IncreaseBy:
                case ArcOperator.DecreaseBy:
                case ArcOperator.MultiplyBy:
                case ArcOperator.DivideBy:
                case ArcOperator.ModulusBy:
                    return false;
                default:
                    throw new ArgumentOutOfRangeException(nameof(op), op, null);
            }
        }

        public bool IsComparison()
        {
            return op is ArcOperator.ObjectEquals or ArcOperator.ReferenceEquals or
                ArcOperator.ObjectNotEquals or ArcOperator.ReferenceNotEquals or ArcOperator.LessThan or
                ArcOperator.LessThanOrEqual or ArcOperator.GreaterThan or ArcOperator.GreaterThanOrEqual;
        }
    }
}