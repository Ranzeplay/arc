using Arc.Compiler.CommandGenerator;
using Arc.Compiler.Shared.CommandGeneration.Mappings;
using Arc.Compiler.Shared.CommandGeneration.Mappings.Math;
using Arc.Compiler.Shared.LexicalAnalysis;

namespace Arc.Compiler.CommandGenerator.Builders
{
    internal class MathCommand
    {
        public static byte[]? FromOperator(OperatorToken op)
        {
            switch (op.Type)
            {
                case OperatorTokenType.Calculation:
                    {
                        switch (op.CalculationOperator)
                        {
                            case CalculationOperatorType.Addition:
                                {
                                    return MathCalcAdd();
                                }
                            case CalculationOperatorType.Subtraction:
                                {
                                    return MathCalcSubtract();
                                }
                            case CalculationOperatorType.Division:
                                {
                                    return MathCalcDivide();
                                }
                            case CalculationOperatorType.Multiply:
                                {
                                    return MathCalcMultiply();
                                }
                            case CalculationOperatorType.Modulo:
                                {
                                    return MathCalcModulo();
                                }
                        }

                        break;
                    }
                case OperatorTokenType.Logical:
                    {
                        switch (op.LogicalOperator)
                        {
                            case LogicalOperatorType.And:
                                {
                                    return MathLogicalAnd();
                                }
                            case LogicalOperatorType.Or:
                                {
                                    return MathLogicalOr();
                                }
                            case LogicalOperatorType.Not:
                                {
                                    return MathLogicalNot();
                                }
                        }

                        break;
                    }
                case OperatorTokenType.Relation:
                    {
                        switch (op.RelationOperator)
                        {
                            case RelationOperatorType.Greater:
                                return MathRelationGreater();
                            case RelationOperatorType.GreaterOrEqual:
                                return MathRelationGreaterOrEqual();
                            case RelationOperatorType.Less:
                                return MathRelationLess();
                            case RelationOperatorType.LessOrEqual:
                                return MathRelationLessOrEqual();
                            case RelationOperatorType.NotEqual:
                                return MathRelationNotEqual();
                            case RelationOperatorType.Equal:
                                return MathRelationEqual();
                        }

                        break;
                    }
            }

            return null;
        }

        private static byte[] MathCalcAdd()
        {
            return Utils.CombineLeadingCommand((byte)RootCommand.Math, (byte)MathRootCommand.Calculation, (byte)CalculationCommand.Add);
        }

        private static byte[] MathCalcSubtract()
        {
            return Utils.CombineLeadingCommand((byte)RootCommand.Math, (byte)MathRootCommand.Calculation, (byte)CalculationCommand.Subtract);
        }

        private static byte[] MathCalcMultiply()
        {
            return Utils.CombineLeadingCommand((byte)RootCommand.Math, (byte)MathRootCommand.Calculation, (byte)CalculationCommand.Multiply);
        }

        private static byte[] MathCalcDivide()
        {
            return Utils.CombineLeadingCommand((byte)RootCommand.Math, (byte)MathRootCommand.Calculation, (byte)CalculationCommand.Divide);
        }

        private static byte[] MathCalcModulo()
        {
            return Utils.CombineLeadingCommand((byte)RootCommand.Math, (byte)MathRootCommand.Calculation, (byte)CalculationCommand.Modulo);
        }

        private static byte[] MathLogicalAnd()
        {
            return Utils.CombineLeadingCommand((byte)RootCommand.Math, (byte)MathRootCommand.Logical, (byte)LogicalCommand.And);
        }

        private static byte[] MathLogicalOr()
        {
            return Utils.CombineLeadingCommand((byte)RootCommand.Math, (byte)MathRootCommand.Logical, (byte)LogicalCommand.Or);
        }

        private static byte[] MathLogicalNot()
        {
            return Utils.CombineLeadingCommand((byte)RootCommand.Math, (byte)MathRootCommand.Logical, (byte)LogicalCommand.Not);
        }

        private static byte[] MathRelationGreater()
        {
            return Utils.CombineLeadingCommand((byte)RootCommand.Math, (byte)MathRootCommand.Relation, (byte)RelationCommand.Greater);
        }

        private static byte[] MathRelationGreaterOrEqual()
        {
            return Utils.CombineLeadingCommand((byte)RootCommand.Math, (byte)MathRootCommand.Relation, (byte)RelationCommand.GreaterOrEqual);
        }

        private static byte[] MathRelationLess()
        {
            return Utils.CombineLeadingCommand((byte)RootCommand.Math, (byte)MathRootCommand.Relation, (byte)RelationCommand.Less);
        }

        private static byte[] MathRelationLessOrEqual()
        {
            return Utils.CombineLeadingCommand((byte)RootCommand.Math, (byte)MathRootCommand.Relation, (byte)RelationCommand.LessOrEqual);
        }

        private static byte[] MathRelationNotEqual()
        {
            return Utils.CombineLeadingCommand((byte)RootCommand.Math, (byte)MathRootCommand.Relation, (byte)RelationCommand.NotEqual);
        }

        private static byte[] MathRelationEqual()
        {
            return Utils.CombineLeadingCommand((byte)RootCommand.Math, (byte)MathRootCommand.Relation, (byte)RelationCommand.Equal);
        }
    }
}
