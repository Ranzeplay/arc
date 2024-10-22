using Arc.Compiler.PackageGenerator.Base;
using Arc.Compiler.PackageGenerator.Models.Intermediate;

namespace Arc.Compiler.PackageGenerator.Models.PrimitiveInstructions
{
    internal class LoadDataToStackInstruction(ArcDataSourceType dataSourceType, long locationId, long fieldId) : ArcStackDataInstructionBase(dataSourceType, locationId, fieldId)
    {
        public override byte[] Opcode => [0x37];
    }
}
