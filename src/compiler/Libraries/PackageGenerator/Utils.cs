using Arc.Compiler.PackageGenerator.Base;
using Arc.Compiler.PackageGenerator.Encoders;
using Arc.Compiler.PackageGenerator.Interfaces;
using Arc.Compiler.PackageGenerator.Models.Builtin;
using Arc.Compiler.PackageGenerator.Models.Descriptors;
using Arc.Compiler.PackageGenerator.Models.Generation;
using Arc.Compiler.PackageGenerator.Models.Relocation;
using Arc.Compiler.PackageGenerator.Models.Scope;
using Arc.Compiler.SyntaxAnalyzer.Models.Components;
using Arc.Compiler.SyntaxAnalyzer.Models.Data;
using Arc.Compiler.SyntaxAnalyzer.Models.Data.DataType;
using Arc.Compiler.SyntaxAnalyzer.Models.Data.Instant;
using Arc.Compiler.SyntaxAnalyzer.Models.Function;
using System.Text;

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

            var typeId = source.GlobalScopeTree.Symbols.FirstOrDefault(x => x is ArcTypeBase bt && bt.FullName == value.TypeName)?.Id;
            var id = source.AccessibleConstants.LongCount() + result.AddedConstants.Count;
            result.AddedConstants.Add(new ArcConstant
            {
                Id = id,
                TypeId = typeId ?? -1,
                IsArray = false,
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
                var funcDeclarator = source.GlobalScopeTree.GetNode<ArcScopeTreeFunctionNodeBase>(funcCall.Identifier.NameArray)
                    ?? throw new InvalidOperationException("Invalid function node");
                return funcDeclarator.Id;
            }
            else
            {
                var funcNode = source.LinkedNamespaces
                    .Select(n => n.GetSpecificChild<ArcScopeTreeFunctionNodeBase>(n => n.Name == funcCall.Identifier.Name))
                    .SkipWhile(n => n == null)
                    .First()
                    ?? throw new InvalidOperationException("Invalid function node");
                return funcNode.Descriptor.Id;
            }
        }

        public static ArcScopeTreeDataTypeNode? GetDataTypeNode(ArcGenerationSource source, ArcDataType dataType)
        {
            if (dataType.DataType == ArcDataType.DataMemberType.Primitive)
            {
                var candidateTypes = source.GlobalScopeTree.FlattenedNodes.OfType<ArcScopeTreeDataTypeNode>();

                return dataType.PrimitiveType switch
                {
                    ArcPrimitiveDataType.Integer => candidateTypes.First(x => x.DataType.Id == ArcPersistentData.IntType.TypeId),
                    ArcPrimitiveDataType.Decimal => candidateTypes.First(x => x.DataType.Id == ArcPersistentData.DecimalType.TypeId),
                    ArcPrimitiveDataType.String => candidateTypes.First(x => x.DataType.Id == ArcPersistentData.StringType.TypeId),
                    ArcPrimitiveDataType.Char => candidateTypes.First(x => x.DataType.Id == ArcPersistentData.CharType.TypeId),
                    ArcPrimitiveDataType.Bool => candidateTypes.First(x => x.DataType.Id == ArcPersistentData.BoolType.TypeId),
                    ArcPrimitiveDataType.None => candidateTypes.First(x => x.DataType.Id == ArcPersistentData.NoneType.TypeId),
                    ArcPrimitiveDataType.Any => candidateTypes.First(x => x.DataType.Id == ArcPersistentData.AnyType.TypeId),
                    _ => throw new NotImplementedException(),
                };
            }
            else
            {
                var typeIdentifier = dataType.DerivativeType!.Identifier;

                if(typeIdentifier.Namespace != null && typeIdentifier.Namespace.Any())
                {
                    return source.GlobalScopeTree.GetNode<ArcScopeTreeDataTypeNode>(typeIdentifier.NameArray);
                }
                else
                {
                    return source.LinkedNamespaces.SelectMany(n => n.GetChildren<ArcScopeTreeDataTypeNode>(c => c.Name == typeIdentifier.Name, true)).First();
                }
            }
        }
    }
}
