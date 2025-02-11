using Arc.Compiler.PackageGenerator.Base;
using Arc.Compiler.PackageGenerator.Encoders;
using Arc.Compiler.PackageGenerator.Models.Generation;
using Arc.Compiler.SyntaxAnalyzer.Models.Data.Instant;

namespace Arc.Compiler.PackageGenerator.Helpers
{
    internal class ArcConstantHelper
    {
        public static IEnumerable<ArcConstant> GetTotalConstants(ArcGenerationSource source, ArcPartialGenerationResult result)
        {
            return source.AccessibleConstants.Concat(result.AddedConstants);
        }

        public static long GetConstantIdOrCreateConstant(ArcInstantValue value, ref ArcGenerationSource source, ref ArcPartialGenerationResult result)
        {
            var typeId = source.GlobalScopeTree.Symbols.FirstOrDefault(x => x is ArcTypeBase bt && bt.FullName == value.TypeName)?.Id;
            var id = new Random().NextInt64();
            result.AddedConstants.Add(new ArcConstant
            {
                Id = id,
                TypeId = typeId ?? -1,
                IsArray = false,
                Value = value.GetRawValue(),
                Encoder = ArcInstantValueEncoder.GetEncoderFromInstantValue(value)
            });

            return id;
        }
    }
}
