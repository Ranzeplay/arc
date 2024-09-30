using Arc.Compiler.PackageGenerator.Interfaces;
using Arc.Compiler.PackageGenerator.Models.Builtin;
using Arc.Compiler.PackageGenerator.Models.Intermediate;
using Arc.Compiler.SyntaxAnalyzer.Models.Data.Instant;
using System.Text;

namespace Arc.Compiler.PackageGenerator.Models.Primitives
{
    internal class PushInstantValueInstruction(ArcInstantValue instantValue) : ArcPrimitiveInstructionBase
    {
        public override byte[] Opcode => [0x02];

        public ArcInstantValue Value { get; set; } = instantValue;

        public new ArcGenerationResult Encode<T>(ArcGenerationSource<T> source)
        {
            switch (Value.Type)
            {
                case ArcInstantValue.ValueType.Integer:
                    {
                        var typeSymbol = source.Symbols.First(x => x.Value is ArcBaseType bt && bt.FullName == "int");
                        return new ArcGenerationResult
                        {
                            GeneratedData = Opcode
                                .Concat(BitConverter.GetBytes((long)0))
                                .Concat(BitConverter.GetBytes(Value.IntegerValue!.Value)),
                            RelocationDescriptors =
                            [
                                new()
                            {
                                Id = new Random().Next(),
                                CommandBeginLocation = 1,
                                Type = ArcRelocationType.Symbol,
                                Target = new(typeSymbol.Value)
                            }
                            ]
                        };
                    }
                case ArcInstantValue.ValueType.Decimal:
                    {
                        var typeSymbol = source.Symbols.First(x => x.Value is ArcBaseType bt && bt.FullName == "decimal");
                        return new ArcGenerationResult
                        {
                            GeneratedData = Opcode
                                .Concat(BitConverter.GetBytes((long)0))
                                .Concat(BitConverter.GetBytes(decimal.ToDouble(Value.DecimalValue!.Value))),
                            RelocationDescriptors =
                            [
                                new()
                            {
                                Id = new Random().Next(),
                                CommandBeginLocation = 1,
                                Type = ArcRelocationType.Symbol,
                                Target = new(typeSymbol.Value)
                            }
                            ]
                        };
                    }
                case ArcInstantValue.ValueType.String:
                    {
                        var typeSymbol = source.Symbols.First(x => x.Value is ArcBaseType bt && bt.FullName == "str");
                        return new ArcGenerationResult
                        {
                            GeneratedData = Opcode
                            .Concat(BitConverter.GetBytes((long)0))
                            .Concat(BitConverter.GetBytes(Value.StringValue!.Value.Length))
                            .Concat(Encoding.UTF8.GetBytes(Value.StringValue!.Value)),
                            RelocationDescriptors =
                        [
                            new()
                            {
                                Id = new Random().Next(),
                                CommandBeginLocation = 1,
                                Type = ArcRelocationType.Symbol,
                                Target = new(typeSymbol.Value)
                            }
                        ]
                        };
                    }
                case ArcInstantValue.ValueType.Boolean:
                    {
                        var typeSymbol = source.Symbols.First(x => x.Value is ArcBaseType bt && bt.FullName == "bool");
                        return new ArcGenerationResult
                        {
                            GeneratedData = Opcode
                            .Concat(BitConverter.GetBytes((long)0))
                            .Concat(BitConverter.GetBytes(Value.BooleanValue!.Value)),
                            RelocationDescriptors =
                        [
                            new()
                            {
                                Id = new Random().Next(),
                                CommandBeginLocation = 1,
                                Type = ArcRelocationType.Symbol,
                                Target = new(typeSymbol.Value)
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
