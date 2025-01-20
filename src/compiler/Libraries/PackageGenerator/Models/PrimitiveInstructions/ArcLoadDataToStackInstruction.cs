using Arc.Compiler.PackageGenerator.Base;
using Arc.Compiler.PackageGenerator.Models.Intermediate;

namespace Arc.Compiler.PackageGenerator.Models.PrimitiveInstructions
{
    internal class ArcLoadDataToStackInstruction(ArcDataLocator locator) : ArcStackDataInstructionBase(locator)
    {
        public override byte[] Opcode => [0x37];
    }
}
