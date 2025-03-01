using Arc.Compiler.PackageGenerator.Encoders;
using Arc.Compiler.PackageGenerator.Models.Generation;
using Arc.Compiler.PackageGenerator.Models.Scope;
using Arc.Compiler.SyntaxAnalyzer.Models.Data.Instant;

namespace Arc.Compiler.PackageGenerator.Helpers
{
    internal class ArcConstantHelper
    {
        public static IEnumerable<ArcConstant> GetTotalConstants(ArcGenerationSource source, ArcPartialGenerationResult result)
        {
            return source.AccessibleConstants.Concat(result.AddedConstants);
        }

        public static ulong GetConstantIdOrCreateConstant(ArcInstantValue value, ref ArcGenerationSource source, ref ArcPartialGenerationResult result)
        {
            var typeId = source.GlobalScopeTree
                .GetNodes<ArcScopeTreeDataTypeNode>()
                .First(x => x.ShortName == value.TypeName).Id;
            var id = (ulong)new Random().NextInt64();
            result.AddedConstants.Add(new ArcConstant
            {
                Id = id,
                TypeId = typeId,
                IsArray = false,
                Value = value.GetRawValue(),
                Encoder = ArcInstantValueEncoder.GetEncoderFromInstantValue(value)
            });

            return id;
        }
    }
}
