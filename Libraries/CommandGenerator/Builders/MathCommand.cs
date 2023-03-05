using Arc.Compiler.Shared.CommandGeneration;
using Arc.Compiler.Shared.CommandGeneration.Math;
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
        public static byte[]? MathCalcAdd()
        {
            return Utils.CombineLeadingCommand((byte)RootCommand.Math, (byte)MathRootCommand.Calculation, (byte)CalculationCommand.Add);
        }

        public static byte[]? MathCalcSubtract()
        {
            return Utils.CombineLeadingCommand((byte)RootCommand.Math, (byte)MathRootCommand.Calculation, (byte)CalculationCommand.Subtract);
        }

        public static byte[]? MathCalcMultiply()
        {
            return Utils.CombineLeadingCommand((byte)RootCommand.Math, (byte)MathRootCommand.Calculation, (byte)CalculationCommand.Multiply);
        }

        public static byte[]? MathCalcDivide()
        {
            return Utils.CombineLeadingCommand((byte)RootCommand.Math, (byte)MathRootCommand.Calculation, (byte)CalculationCommand.Divide);
        }

        public static byte[]? MathCalcModulo()
        {
            return Utils.CombineLeadingCommand((byte)RootCommand.Math, (byte)MathRootCommand.Calculation, (byte)CalculationCommand.Modulo);
        }

        public static byte[]? MathLogicalAnd()
        {
            return Utils.CombineLeadingCommand((byte)RootCommand.Math, (byte)MathRootCommand.Logical, (byte)LogicalCommand.And);
        }

        public static byte[]? MathLogicalOr()
        {
            return Utils.CombineLeadingCommand((byte)RootCommand.Math, (byte)MathRootCommand.Logical, (byte)LogicalCommand.Or);
        }

        public static byte[]? MathLogicalNot()
        {
            return Utils.CombineLeadingCommand((byte)RootCommand.Math, (byte)MathRootCommand.Logical, (byte)LogicalCommand.Not);
        }
    }
}
