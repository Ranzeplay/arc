using Arc.Compiler.PackageGenerator.Base;
using Arc.Compiler.PackageGenerator.Models.Generation;
using Arc.Compiler.PackageGenerator.Models.Intermediate;
using Arc.Compiler.PackageGenerator.Models.Relocation;
using Arc.Compiler.SyntaxAnalyzer.Models.Components;

namespace Arc.Compiler.PackageGenerator.Models.PrimitiveInstructions
{
    internal class ArcDeclarationInstruction(ArcDataDeclarator dataDecl) : ArcPrimitiveInstructionBase
    {
        public override byte[] Opcode => [0x01];

        public ArcDataDeclarator DataDeclarator { get; } = dataDecl;

        public new ArcPartialGenerationResult Encode(ArcGenerationSource source)
        {
            var slot = new ArcDataSlot
            {
                Name = "Local data slot",
                Declarator = DataDeclarator,
                SlotId = source.LocalDataSlots.Count(),
            };

            var dataType = Utils.GetDataTypeNode(source, DataDeclarator.DataType).DataType;

            return new ArcPartialGenerationResult
            {
                // TODO: use bitmask
                GeneratedData =
                [
                    .. Opcode,
                    DataDeclarator.DataType.MemoryStorageType == ArcMemoryStorageType.Value ? (byte)0x01 : (byte)0x00,
                    DataDeclarator.DataType.IsArray ? (byte)0x01 : (byte)0x00,
                    .. BitConverter.GetBytes((long)0),
                ],
                RelocationTargets = [
                    new() {
                        Location = 3,
                        TargetType = ArcRelocationTargetType.Symbol,
                        Symbol = dataType
                    }
                ],
                DataSlots = [slot]
            };
        }
    }
}
