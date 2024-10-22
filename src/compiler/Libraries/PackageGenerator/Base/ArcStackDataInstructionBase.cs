using Arc.Compiler.PackageGenerator.Models.Generation;
using Arc.Compiler.PackageGenerator.Models.Intermediate;

namespace Arc.Compiler.PackageGenerator.Base
{
    internal abstract class ArcStackDataInstructionBase(ArcDataSourceType dataSourceType, long locationId, long fieldId) : ArcPrimitiveInstructionBase
    {
        public ArcDataSourceType DataSourceType { get; set; } = dataSourceType;

        public long LocationId { get; set; } = locationId;

        public long FieldId { get; set; } = fieldId;

        public new ArcPartialGenerationResult Encode(ArcGenerationSource source)
        {
            return new ArcPartialGenerationResult
            {
                GeneratedData = Opcode.Concat([(byte)DataSourceType, (byte)LocationId, (byte)FieldId]).ToArray(),
            };
        }
    }
}
