using Arc.Compiler.PackageGenerator.Base;
using Arc.Compiler.PackageGenerator.Models.Builtin;
using Arc.Compiler.PackageGenerator.Models.Generation;
using Arc.Compiler.PackageGenerator.Models.Relocation;
using Arc.Compiler.SyntaxAnalyzer.Models.Data.Instant;
using System.Text;

namespace Arc.Compiler.PackageGenerator.Models.PrimitiveInstructions
{
    internal class ArcPushInstantValueInstruction(ArcInstantValue instantValue) : ArcPrimitiveInstructionBase
    {
        public override byte[] Opcode => [0x02];

        public ArcInstantValue Value { get; set; } = instantValue;

        public new ArcPartialGenerationResult Encode(ArcGenerationSource source)
        {
            switch (Value.Type)
            {
                case ArcInstantValue.ValueType.Integer:
                    {
                        var typeSymbol = source.AccessibleSymbols.First(x => x is ArcBaseType bt && bt.FullName == "int");
                        return new ArcPartialGenerationResult
                        {
                            GeneratedData = [.. Opcode, .. BitConverter.GetBytes((long)0), .. BitConverter.GetBytes(Value.IntegerValue!.Value)],
                            RelocationTargets = [
                                new()
                                {
                                    Location = 1,
                                    TargetType = ArcRelocationTargetType.Symbol,
                                    Symbol = typeSymbol
                                }
                            ]
                        };
                    }
                case ArcInstantValue.ValueType.Decimal:
                    {
                        var typeSymbol = source.AccessibleSymbols.First(x => x is ArcBaseType bt && bt.FullName == "decimal");
                        return new ArcPartialGenerationResult
                        {
                            GeneratedData = [.. Opcode, .. BitConverter.GetBytes((long)0), .. BitConverter.GetBytes(decimal.ToDouble(Value.DecimalValue!.Value))],
                            RelocationTargets = [
                                new()
                                {
                                    Location = 1,
                                    TargetType = ArcRelocationTargetType.Symbol,
                                    Symbol = typeSymbol
                                }
                            ]
                        };
                    }
                case ArcInstantValue.ValueType.String:
                    {
                        var typeSymbol = source.AccessibleSymbols.First(x => x is ArcBaseType bt && bt.FullName == "str");
                        return new ArcPartialGenerationResult
                        {
                            GeneratedData = Opcode
                            .Concat(BitConverter.GetBytes((long)0))
                            .Concat(BitConverter.GetBytes(Value.StringValue!.Value.Length))
                            .Concat(Encoding.UTF8.GetBytes(Value.StringValue!.Value)).ToList(),
                            RelocationTargets = [
                                new()
                                {
                                    Location = 1,
                                    TargetType = ArcRelocationTargetType.Symbol,
                                    Symbol = typeSymbol
                                }
                            ]
                        };
                    }
                case ArcInstantValue.ValueType.Boolean:
                    {
                        var typeSymbol = source.AccessibleSymbols.First(x => x is ArcBaseType bt && bt.FullName == "bool");
                        return new ArcPartialGenerationResult
                        {
                            GeneratedData = Opcode
                            .Concat(BitConverter.GetBytes((long)0))
                            .Concat(BitConverter.GetBytes(Value.BooleanValue!.Value)).ToList(),
                            RelocationTargets = [
                                new()
                                {
                                    Location = 1,
                                    TargetType = ArcRelocationTargetType.Symbol,
                                    Symbol = typeSymbol
                                }
                            ]
                        };
                    }
                default:
                    throw new NotImplementedException();
            }
        }
    }
}
