using Arc.Compiler.PackageGenerator.Base;
using Arc.Compiler.PackageGenerator.Models.Intermediate;

namespace Arc.Compiler.PackageGenerator.Models.PrimitiveInstructions
{
    internal class ArcSaveDataFromStackInstruction(ArcStackDataOperationDescriptor locator) : ArcStackDataInstructionBase(locator)
    {
        public override byte[] Opcode => [0x38];
    }
}
