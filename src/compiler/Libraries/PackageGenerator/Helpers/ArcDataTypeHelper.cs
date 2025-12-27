using Arc.Compiler.PackageGenerator.Base;
using Arc.Compiler.PackageGenerator.Interfaces;
using Arc.Compiler.PackageGenerator.Models.Builtin;
using Arc.Compiler.PackageGenerator.Models.Generation;
using Arc.Compiler.PackageGenerator.Models.Scope;
using Arc.Compiler.SyntaxAnalyzer.Models.Data.DataType;
using Arc.Compiler.SyntaxAnalyzer.Models.Identifier;
using System.Diagnostics;

namespace Arc.Compiler.PackageGenerator.Helpers
{
    internal class ArcDataTypeHelper
    {
        public static IArcDataTypeProxy? GetDataType(ArcGenerationSource source, ArcDataType dataType)
        {
            if (dataType.DataType == ArcDataType.DataMemberType.Primitive)
            {
                var candidateTypes = source.DirectlyAccessibleTypes
                    .OfType<ArcScopeTreeDataTypeNode>()
                    .Where(x => x.DataType is ArcBaseType);

                return dataType.PrimitiveType switch
                {
                    ArcPrimitiveDataType.Integer => candidateTypes.First(x =>
                        x.DataType.TypeId == ArcPersistentData.IntType.TypeId),
                    ArcPrimitiveDataType.Decimal => candidateTypes.First(x =>
                        x.DataType.TypeId == ArcPersistentData.DecimalType.TypeId),
                    ArcPrimitiveDataType.String => candidateTypes.First(x =>
                        x.DataType.TypeId == ArcPersistentData.StringType.TypeId),
                    ArcPrimitiveDataType.Char => candidateTypes.First(x =>
                        x.DataType.TypeId == ArcPersistentData.CharType.TypeId),
                    ArcPrimitiveDataType.Bool => candidateTypes.First(x =>
                        x.DataType.TypeId == ArcPersistentData.BoolType.TypeId),
                    ArcPrimitiveDataType.Byte => candidateTypes.First(x =>
                        x.DataType.TypeId == ArcPersistentData.ByteType.TypeId),
                    ArcPrimitiveDataType.None => candidateTypes.First(x =>
                        x.DataType.TypeId == ArcPersistentData.NoneType.TypeId),
                    ArcPrimitiveDataType.Any => candidateTypes.First(x =>
                        x.DataType.TypeId == ArcPersistentData.AnyType.TypeId),
                    ArcPrimitiveDataType.Function => candidateTypes.First(x =>
                        x.DataType.TypeId == ArcPersistentData.FunctionType.TypeId),
                    ArcPrimitiveDataType.Infer => throw new NotImplementedException(),
                    _ => throw new UnreachableException(),
                };
            }
            else
            {
                return GetComplexType(source, dataType.ComplexType!.Identifier);
            }
        }

        public static IArcDataTypeProxy? GetComplexType(ArcGenerationSource source, ArcFlexibleIdentifier identifier)
        {
            if (identifier.Namespace != null && identifier.Namespace.Any())
            {
                return source.GlobalScopeTree.GetNode<ArcScopeTreeDataTypeNode>(identifier.NameArray);
            }
            else
            {
                return source.DirectlyAccessibleTypes.FirstOrDefault(c => c.ShortName.Equals(identifier.Name)) ??
                       source.GenericTypes.FirstOrDefault(c => c.ShortName.Equals(identifier.Name));
            }
        }

        public static ArcScopeTreeDataTypeNode? GetDataTypeNode(ArcGenerationSource source, ArcTypeBase dataType)
        {
            return source.GlobalScopeTree.FlattenedNodes
                .OfType<ArcScopeTreeDataTypeNode>()
                .FirstOrDefault(x => x.DataType.TypeId == dataType.TypeId);
        }
    }
}
