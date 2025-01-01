using Arc.Compiler.PackageGenerator.Base;
using Arc.Compiler.PackageGenerator.Models.Generation;
using Arc.Compiler.SyntaxAnalyzer.Models.Data.Instant;
using System.Text;
using Arc.Compiler.PackageGenerator.Models.Relocation;

namespace Arc.Compiler.PackageGenerator
{
    // ReSharper disable once InconsistentNaming
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

            var typeId = source.AccessibleSymbols.FirstOrDefault(x => x is ArcTypeBase bt && bt.FullName == value.TypeName)?.Id;
            var id = source.AccessibleConstants.LongCount() + result.AddedConstants.LongCount();
            result.AddedConstants.Add(new ArcConstant
            {
                Id = id,
                TypeId = typeId ?? -1,
                Value = value
            });

            return id;
        }

        public static IEnumerable<byte> SerializeString(string s)
        {
            var result = new List<byte>();
            result.AddRange(BitConverter.GetBytes((long)s.Length));
            result.AddRange(Encoding.UTF8.GetBytes(s));
            return result;
        }

        public static int TraceRelocationLabel(IEnumerable<ArcRelocationLabel> labels, long count)
        {
            var firstLabel = labels.First();
            var antiLabel = firstLabel.Type.GetAntiLabel();

            var distance = 0;
            var layer = 0;
            foreach (var label in labels)
            {
                if (label.Type == firstLabel.Type)
                {
                    layer++;
                }
                else if (label.Type == antiLabel)
                {
                    layer--;
                    if (layer == 0)
                    {
                        count--;
                        if (count == 0)
                        {
                            return distance;
                        }
                    }
                }

                distance++;
            }
            
            throw new ArgumentOutOfRangeException(nameof(labels), "Cannot find the corresponding label");
        }
    }
}
