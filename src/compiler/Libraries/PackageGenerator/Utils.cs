using Arc.Compiler.PackageGenerator.Base;
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
            // TODO: Fix duplicated entry (existingConstant is always null)
            var existingConstant = GetTotalConstants(source, result).FirstOrDefault(c => c.Value.Equals(value));
            if (existingConstant != null)
            {
                return existingConstant.Id;
            }
            else
            {
                var typeId = source.AccessibleSymbols.FirstOrDefault(x => x is ArcTypeBase bt && bt.FullName == value.TypeName)?.Id;
                var id = source.AccessibleConstants.LongCount() + result.AddedConstants.LongCount();
                result.AddedConstants = result.AddedConstants.Append(new ArcConstant
                {
                    Id = id,
                    TypeId = typeId ?? -1,
                    Value = value
                });

                return id;
            }
        }
    }
}
