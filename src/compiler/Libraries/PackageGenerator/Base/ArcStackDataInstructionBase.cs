using Arc.Compiler.PackageGenerator.Models.Generation;
using Arc.Compiler.PackageGenerator.Models.Intermediate;

namespace Arc.Compiler.PackageGenerator.Base
{
    internal abstract class ArcStackDataInstructionBase(ArcStackDataOperationDescriptor locator) : ArcPrimitiveInstructionBase
    {
        public ArcStackDataOperationDescriptor OperationDescriptor { get; set; } = locator;

        public new ArcPartialGenerationResult Encode(ArcGenerationSource source)
        {
            return new ArcPartialGenerationResult
            {
                GeneratedData = [
                    ..Opcode,
                    (byte)OperationDescriptor.Source,
                    (byte)OperationDescriptor.StorageType,
                    ..BitConverter.GetBytes(OperationDescriptor.LocationId),
                    (byte)(OperationDescriptor.Overwrite ? 0x01 : 0x00),
                    ],
            };
        }
    }
}
