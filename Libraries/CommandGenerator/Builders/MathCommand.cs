using Arc.Compiler.Shared.CommandGeneration;
using Arc.Compiler.Shared.CommandGeneration.Math;
using Arc.Compiler.Shared.LexicalAnalysis;
using Arc.Compiler.Shared.Parsing.AST;
using Arc.CompilerCommandGenerator.Models;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Arc.CompilerCommandGenerator.Builders
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
                case OperatorTokenType.Relation:
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
    }
}
