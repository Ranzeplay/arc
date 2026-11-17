using Arc.Compiler.PackageGenerator.Base;
using Arc.Compiler.PackageGenerator.Models.Generation;
using Arc.Compiler.PackageGenerator.Models.Relocation;
using Arc.Compiler.PackageGenerator.Models.Scope;
using Arc.Compiler.SyntaxAnalyzer.Models.Data.Instant;
using System.Text;

namespace Arc.Compiler.PackageGenerator.Models.PrimitiveInstructions
{
    internal class ArcPushInstantValueInstruction(ArcInstantValue instantValue) : ArcPrimitiveInstructionBase
    {
        public override byte[] Opcode => [0x02];

        public ArcInstantValue Value { get; set; } = instantValue;

        public override ArcPartialGenerationResult Encode(ArcGenerationSource source)
        {
            switch (Value)
            {
                case ArcInstantIntegerValue value:
                    {
                        var typeSymbol = source.GlobalScopeTree.GetNode<ArcScopeTreeDataTypeNode>(["Arc", "Base", "int"]);
                        return new ArcPartialGenerationResult
                        {
                            GeneratedData = [.. Opcode, .. BitConverter.GetBytes((long)0), .. BitConverter.GetBytes(value.Value)],
                            RelocationTargets = [
                                new()
                                {
                                    Location = 1,
                                    TargetType = ArcRelocationTargetType.Symbol,
                                    Symbol = typeSymbol,
                                    Layer = Guid.Empty
                                }
                            ]
                        };
                    }
                case ArcInstantDecimalValue value:
                    {
                        var typeSymbol = source.GlobalScopeTree.GetNode<ArcScopeTreeDataTypeNode>(["Arc", "Base", "decimal"]);
                        return new ArcPartialGenerationResult
                        {
                            GeneratedData = [.. Opcode, .. BitConverter.GetBytes((long)0), .. BitConverter.GetBytes(decimal.ToDouble(value.Value))],
                            RelocationTargets = [
                                new()
                                {
                                    Location = 1,
                                    TargetType = ArcRelocationTargetType.Symbol,
                                    Symbol = typeSymbol,
                                    Layer = Guid.Empty
                                }
                            ]
                        };
                    }
                case ArcInstantStringValue value:
                    {
                        var typeSymbol = source.GlobalScopeTree.GetNode<ArcScopeTreeDataTypeNode>(["Arc", "Base", "str"]);
                        return new ArcPartialGenerationResult
                        {
                            GeneratedData = Opcode
                            .Concat(BitConverter.GetBytes((long)0))
                            .Concat(BitConverter.GetBytes(value.Value.Length))
                            .Concat(Encoding.UTF8.GetBytes(value.Value)).ToList(),
                            RelocationTargets = [
                                new()
                                {
                                    Location = 1,
                                    TargetType = ArcRelocationTargetType.Symbol,
                                    Symbol = typeSymbol,
                                    Layer = Guid.Empty
                                }
                            ]
                        };
                    }
                case ArcBooleanValue value:
                    {
                        var typeSymbol = source.GlobalScopeTree.GetNode<ArcScopeTreeDataTypeNode>(["Arc", "Base", "bool"]);
                        return new ArcPartialGenerationResult
                        {
                            GeneratedData = Opcode
                            .Concat(BitConverter.GetBytes((long)0))
                            .Concat(BitConverter.GetBytes(value.Value)).ToList(),
                            RelocationTargets = [
                                new()
                                {
                                    Location = 1,
                                    TargetType = ArcRelocationTargetType.Symbol,
                                    Symbol = typeSymbol,
                                    Layer = Guid.Empty
                                }
                            ]
                        };
                    }
                case ArcNoneValue or ArcAnyValue:
                    throw new NotSupportedException("The stack backend cannot push none or any literals directly");
                case null:
                    throw new InvalidOperationException("Uninitialized instant value");
            }
        }
    }
}
