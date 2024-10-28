using Arc.Compiler.PackageGenerator.Models.Builtin;
using Arc.Compiler.PackageGenerator.Models.Generation;
using Arc.Compiler.SyntaxAnalyzer.Models.Data.Instant;

namespace Arc.Compiler.PackageGenerator
{
    internal static class Utils
    {
        public static IEnumerable<ArcConstant> GetTotalConstants(ArcGenerationSource source, ArcPartialGenerationResult result)
        {
            return source.AccessibleConstants.Concat(result.AddedConstants);
        }

        public static long GetConstantIdOrCreateConstant(ArcInstantValue value, ref ArcGenerationSource source, ref ArcPartialGenerationResult result)
        {
            var existingConstant = GetTotalConstants(source, result).FirstOrDefault(c => c.Value.Equals(value));
            if (existingConstant != null)
            {
                return existingConstant.Id;
            }
            else
            {
                var typeId = ArcPersistentData.BaseTypes.FirstOrDefault(t => t.Name.Equals(value.Type.ToString())).Id;
                var id = source.AccessibleConstants.LongCount() + result.AddedConstants.LongCount();
                result.AddedConstants = result.AddedConstants.Append(new ArcConstant
                {
                    Id = id,
                    TypeId = typeId,
                    Value = value
                });

                return id;
            }
        }
    }
}
