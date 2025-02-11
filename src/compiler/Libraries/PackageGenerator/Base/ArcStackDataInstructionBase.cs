using Arc.Compiler.PackageGenerator.Encoders;
using Arc.Compiler.PackageGenerator.Models.Generation;
using Arc.Compiler.PackageGenerator.Models.Intermediate;

namespace Arc.Compiler.PackageGenerator.Base
{
    internal abstract class ArcStackDataInstructionBase(ArcDataLocator locator) : ArcPrimitiveInstructionBase
    {
        public ArcDataLocator Locator { get; set; } = locator;

        public new ArcPartialGenerationResult Encode(ArcGenerationSource source)
        {
            return new ArcPartialGenerationResult
            {
                GeneratedData = [
                    ..Opcode,
                    (byte)Locator.Source,
                    ..BitConverter.GetBytes(Locator.LocationId),
                    ..ArcArrayEncoder.SerializeArray(Locator.FieldChain.Select(f => f == null ? 0 : f.Id)),
                    ..Locator.Addend
                    ],
            };
        }
    }
}
