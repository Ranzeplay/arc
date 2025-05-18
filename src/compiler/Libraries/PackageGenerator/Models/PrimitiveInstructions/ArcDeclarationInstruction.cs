using Arc.Compiler.PackageGenerator.Base;
using Arc.Compiler.PackageGenerator.Helpers;
using Arc.Compiler.PackageGenerator.Models.Builtin;
using Arc.Compiler.PackageGenerator.Models.Descriptors;
using Arc.Compiler.PackageGenerator.Models.Generation;
using Arc.Compiler.PackageGenerator.Models.Intermediate;
using Arc.Compiler.PackageGenerator.Models.Logging;
using Arc.Compiler.SyntaxAnalyzer.Models.Components;
using Microsoft.Extensions.Logging;

namespace Arc.Compiler.PackageGenerator.Models.PrimitiveInstructions
{
    internal class ArcDeclarationInstruction(ArcDataDeclarator dataDecl) : ArcPrimitiveInstructionBase
    {
        public override byte[] Opcode => [0x01];

        public ArcDataDeclarator DataDeclarator { get; } = dataDecl;

        public override ArcPartialGenerationResult Encode(ArcGenerationSource source)
        {
            var dataTypeProxy = ArcDataTypeHelper.GetDataType(source, DataDeclarator.DataType);

            ArcSourceLocatableLog? errorLog = null;
            if (dataTypeProxy == null)
            {
                errorLog = new ArcSourceLocatableLog(LogLevel.Error, 0, "Data type not found", source.Name, DataDeclarator.Context);
            }

            var dataTypeNode = ArcDataTypeHelper.GetDataTypeNode(source, dataTypeProxy?.ResolvedType!);

            var slot = new ArcDataSlot
            {
                Name = DataDeclarator.Identifier.Name,
                DeclarationDescriptor = new ArcDataDeclarationDescriptor()
                {
                    Type = dataTypeProxy?.ResolvedType ?? ArcBaseType.Placeholder(),
                    AllowNone = false,
                    Dimension = DataDeclarator.DataType.Dimension,
                    MemoryStorageType = DataDeclarator.DataType.MemoryStorageType,
                    SyntaxTree = DataDeclarator,
                },
                SlotId = (uint)source.LocalDataSlots.Count,
            };

            return new ArcPartialGenerationResult
            {
                // TODO: use bitmask
                GeneratedData =
                [
                    .. Opcode,
                    DataDeclarator.DataType.MemoryStorageType == ArcMemoryStorageType.Value ? (byte)0x01 : (byte)0x00,
                    .. BitConverter.GetBytes(DataDeclarator.DataType.Dimension),
                    .. BitConverter.GetBytes(dataTypeNode?.ResolvedType.TypeId ?? 0),
                ],
                DataSlots = [slot],
                TotalGeneratedDataSlotCount = 1,
                Logs = errorLog == null ? [] : [errorLog],
            };
        }
    }
}
