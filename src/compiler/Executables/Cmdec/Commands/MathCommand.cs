using Arc.Cmdec.Models;
using Arc.Compiler.Shared.CommandGeneration.Mappings.Math;

namespace Arc.Cmdec.Commands
{
    internal class MathCommand
    {
        public static DecodeResult? SelectCalculation(long location, IEnumerable<byte> commands)
        {
            return commands.ElementAt(1) switch
            {
                (byte)CalculationCommand.Add => CalcAdd(location, commands),
                (byte)CalculationCommand.Subtract => CalcSub(location, commands),
                (byte)CalculationCommand.Multiply => CalcMul(location, commands),
                (byte)CalculationCommand.Divide => CalcDiv(location, commands),
                (byte)CalculationCommand.Modulo => CalcMod(location, commands),
                (byte)CalculationCommand.Inverse => CalcInv(location, commands),
                _ => null,
            };
        }

        public static DecodeResult? SelectLogical(long location, IEnumerable<byte> commands)
        {
            return commands.ElementAt(1) switch
            {
                (byte)LogicalCommand.And => LogicalAnd(location, commands),
                (byte)LogicalCommand.Or => LogicalOr(location, commands),
                (byte)LogicalCommand.Not => LogicalNot(location, commands),
                _ => null,
            };
        }

        public static DecodeResult? SelectRelation(long location, IEnumerable<byte> commands)
        {
            return commands.ElementAt(1) switch
            {
                (byte)RelationCommand.Greater => RelationGreater(location, commands),
                (byte)RelationCommand.GreaterOrEqual => RelationGreaterEqual(location, commands),
                (byte)RelationCommand.Less => RelationLess(location, commands),
                (byte)RelationCommand.LessOrEqual => RelationLessEqual(location, commands),
                (byte)RelationCommand.Equal => RelationEqual(location, commands),
                (byte)RelationCommand.NotEqual => RelationNotEqual(location, commands),
                _ => null,
            };
        }

        public static DecodeResult CalcAdd(long location, IEnumerable<byte> commands)
        {
            return new(2, new(location, commands.Take(2).ToArray(), "Perform ADD operation on stack top"));
        }

        public static DecodeResult CalcSub(long location, IEnumerable<byte> commands)
        {
            return new(2, new(location, commands.Take(2).ToArray(), "Perform SUB operation on stack top"));
        }

        public static DecodeResult CalcMul(long location, IEnumerable<byte> commands)
        {
            return new(2, new(location, commands.Take(2).ToArray(), "Perform MUL operation on stack top"));
        }

        public static DecodeResult CalcDiv(long location, IEnumerable<byte> commands)
        {
            return new(2, new(location, commands.Take(2).ToArray(), "Perform DIV operation on stack top"));
        }

        public static DecodeResult CalcMod(long location, IEnumerable<byte> commands)
        {
            return new(2, new(location, commands.Take(2).ToArray(), "Perform MOD operation on stack top"));
        }

        public static DecodeResult CalcInv(long location, IEnumerable<byte> commands)
        {
            return new(2, new(location, commands.Take(2).ToArray(), "Perform INV operation on stack top"));
        }

        public static DecodeResult LogicalAnd(long location, IEnumerable<byte> commands)
        {
            return new(2, new(location, commands.Take(2).ToArray(), "Perform AND operation on stack top"));
        }

        public static DecodeResult LogicalOr(long location, IEnumerable<byte> commands)
        {
            return new(2, new(location, commands.Take(2).ToArray(), "Perform OR operation on stack top"));
        }

        public static DecodeResult LogicalNot(long location, IEnumerable<byte> commands)
        {
            return new(2, new(location, commands.Take(2).ToArray(), "Perform NOT operation on stack top"));
        }

        public static DecodeResult RelationGreater(long location, IEnumerable<byte> commands)
        {
            return new(2, new(location, commands.Take(2).ToArray(), "Perform GREATER operation on stack top"));
        }

        public static DecodeResult RelationGreaterEqual(long location, IEnumerable<byte> commands)
        {
            return new(2, new(location, commands.Take(2).ToArray(), "Perform GREATER_EQUAL operation on stack top"));
        }

        public static DecodeResult RelationLess(long location, IEnumerable<byte> commands)
        {
            return new(2, new(location, commands.Take(2).ToArray(), "Perform LESS operation on stack top"));
        }

        public static DecodeResult RelationLessEqual(long location, IEnumerable<byte> commands)
        {
            return new(2, new(location, commands.Take(2).ToArray(), "Perform LESS_EQUAL operation on stack top"));
        }

        public static DecodeResult RelationEqual(long location, IEnumerable<byte> commands)
        {
            return new(2, new(location, commands.Take(2).ToArray(), "Perform EQUAL operation on stack top"));
        }

        public static DecodeResult RelationNotEqual(long location, IEnumerable<byte> commands)
        {
            return new(2, new(location, commands.Take(2).ToArray(), "Perform NOT_EQUAL operation on stack top"));
        }
    }
}
