using Arc.Compiler.PackageGenerator.Base;
using Arc.Compiler.PackageGenerator.Models.Generation;
using Arc.Compiler.SyntaxAnalyzer.Models.Data.Instant;
using System.Text;
using Arc.Compiler.PackageGenerator.Models.Relocation;
using Arc.Compiler.PackageGenerator.Interfaces;
using Arc.Compiler.PackageGenerator.Encoders;
using Arc.Compiler.SyntaxAnalyzer.Models.Components;
using Arc.Compiler.PackageGenerator.Models.Descriptors;
using Arc.Compiler.SyntaxAnalyzer.Models.Data;
using Arc.Compiler.SyntaxAnalyzer.Models.Function;
using Arc.Compiler.PackageGenerator.Models.Descriptors.Function;
using Arc.Compiler.PackageGenerator.Models.Scope;
using System;

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
            var id = source.AccessibleConstants.LongCount() + result.AddedConstants.Count;
            result.AddedConstants.Add(new ArcConstant
            {
                Id = id,
                TypeId = typeId ?? -1,
                Value = value.GetRawValue(),
                Encoder = GetEncoderFromInstantValue(value)
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

        public static IEnumerable<byte> SerializeArray(IEnumerable<long> array)
        {
            var result = new List<byte>();
            result.AddRange(BitConverter.GetBytes((long)array.Count()));
            foreach (var item in array)
            {
                result.AddRange(BitConverter.GetBytes(item));
            }
            return result;
        }

        public static int LocateLabelRelativeLocation(IEnumerable<ArcRelocationLabel> labels, bool forwardSearch, ArcRelocationTarget target, long count)
        {
            var label = target.Label;
            var antiLabel = label.GetAntiLabel();
            var sourceLocation = target.Location;

            var list = labels.ToList();
            var directionList = forwardSearch switch
            {
                true => list.Where(l => l.Location >= sourceLocation).OrderBy(l => l.Location),
                false => list.Where(l => l.Location <= sourceLocation).OrderByDescending(l => l.Location)
            };

            var layer = 0;
            for (int i = 0; i < directionList.Count(); i++)
            {
                var l = directionList.ElementAt(i);

                if (l.Type == antiLabel)
                {
                    layer++;
                }
                else if (l.Type == label)
                {
                    layer--;
                    if (layer <= 0)
                    {
                        count--;
                        if (count == 0)
                        {
                            return (forwardSearch ? 1 : -1) * (i + 1);
                        }
                    }
                }
            }


            throw new ArgumentOutOfRangeException(nameof(labels), "Cannot find the corresponding label");
        }

        public static void ReplaceRange<T>(this IList<T> list, int index, IEnumerable<T> collection)
        {
            for (int i = 0; i < collection.Count(); i++)
            {
                list[index + i] = collection.ElementAt(i);
            }
        }

        public static IArcConstantEncoder GetEncoderFromInstantValue(ArcInstantValue value)
        {
            return value.Type switch
            {
                ArcInstantValue.ValueType.Integer => new ArcIntegerEncoder(),
                ArcInstantValue.ValueType.Decimal => new ArcDecimalEncoder(),
                ArcInstantValue.ValueType.String => new ArcStringEncoder(),
                ArcInstantValue.ValueType.Boolean => new ArcBooleanEncoder(),
                _ => throw new NotImplementedException(),
            };
        }

        public static IEnumerable<byte> Encode(this ArcDataDeclarationDescriptor decl, IEnumerable<ArcSymbolBase> symbols)
        {
            // TODO: Use bitmask
            var dataType = symbols.First(x => x is ArcTypeBase && x.Name == decl.Type.FullName);
            return [
                decl.MemoryStorageType == ArcMemoryStorageType.Value ? (byte)0x01 : (byte)0x00,
                decl.IsArray ? (byte)0x01 : (byte)0x00,
                decl.Mutability == ArcMutability.Variable ? (byte)0x01 : (byte)0x00,
                .. BitConverter.GetBytes(dataType.Id),
            ];
        }

        public static long GetFunctionId(ArcGenerationSource source, ArcFunctionCall funcCall)
        {
            if (funcCall.Identifier.Namespace != null && funcCall.Identifier.Namespace.Any())
            {
                var funcDeclarator = source.AccessibleSymbols
                    .OfType<ArcFunctionDescriptor>()
                    .FirstOrDefault(f => f.RawFullName.StartsWith(funcCall.Identifier.AsFunctionIdentifier()))
                    ?? throw new InvalidOperationException("Invalid function declarator");
                return funcDeclarator.Id;
            }
            else
            {
                var funcNode = source.CurrentNode
                    .Root
                    .GetSpecificChild<ArcScopeTreeIndividualFunctionNode>(n => n.SyntaxTree.Declarator.Identifier.Name == funcCall.Identifier.Name, true)
                    ?? throw new InvalidOperationException("Invalid function node");
                return funcNode.Descriptor.Id;
            }
        }
    }
}
