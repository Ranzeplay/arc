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
                    SyntaxTree = DataDeclarator,
                },
                SlotId = (uint)source.LocalDataSlots.Count,
            };

            var specializedGenericTypeId = DataDeclarator.DataType.SpecializedGenericTypes.Select(t => ArcDataTypeHelper.GetDataType(source, t)?.ResolvedType.TypeId ?? 0);

            return new ArcPartialGenerationResult
            {
                // TODO: use bitmask
                GeneratedData =
                [
                    .. Opcode,
                    .. BitConverter.GetBytes(DataDeclarator.DataType.Dimension),
                    .. BitConverter.GetBytes(dataTypeNode?.ResolvedType.TypeId ?? 0),
                    
                    .. BitConverter.GetBytes(specializedGenericTypeId.LongCount()),
                    .. specializedGenericTypeId.SelectMany(BitConverter.GetBytes),
                ],
                DataSlots = [slot],
                TotalGeneratedDataSlotCount = 1,
                Logs = errorLog == null ? [] : [errorLog],
            };
        }
    }
}
