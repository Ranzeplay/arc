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
                    ..Utils.SerializeArray(Locator.FieldChain.Select(f => f.Id)),
                    ..Locator.Addend
                    ],
            };
        }
    }
}
