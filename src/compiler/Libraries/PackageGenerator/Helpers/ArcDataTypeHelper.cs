using Arc.Compiler.PackageGenerator.Base;
using Arc.Compiler.PackageGenerator.Models.Builtin;
using Arc.Compiler.PackageGenerator.Models.Generation;
using Arc.Compiler.PackageGenerator.Models.Intermediate;
using Arc.Compiler.PackageGenerator.Models.Scope;
using Arc.Compiler.SyntaxAnalyzer.Models.Data.DataType;
using Arc.Compiler.SyntaxAnalyzer.Models.Identifier;

namespace Arc.Compiler.PackageGenerator.Helpers
{
    internal class ArcDataTypeHelper
    {
        public static ArcScopeTreeDataTypeNode? GetDataTypeNode(ArcGenerationSource source, ArcDataType dataType)
        {
            if (dataType.DataType == ArcDataType.DataMemberType.Primitive)
            {
                var candidateTypes = source.DirectlyAccessibleNodes.OfType<ArcScopeTreeDataTypeNode>();

                return dataType.PrimitiveType switch
                {
                    ArcPrimitiveDataType.Integer => candidateTypes.First(x => x.DataType.TypeId == ArcPersistentData.IntType.TypeId),
                    ArcPrimitiveDataType.Decimal => candidateTypes.First(x => x.DataType.TypeId == ArcPersistentData.DecimalType.TypeId),
                    ArcPrimitiveDataType.String => candidateTypes.First(x => x.DataType.TypeId == ArcPersistentData.StringType.TypeId),
                    ArcPrimitiveDataType.Char => candidateTypes.First(x => x.DataType.TypeId == ArcPersistentData.CharType.TypeId),
                    ArcPrimitiveDataType.Bool => candidateTypes.First(x => x.DataType.TypeId == ArcPersistentData.BoolType.TypeId),
                    ArcPrimitiveDataType.Byte => candidateTypes.First(x => x.DataType.TypeId == ArcPersistentData.ByteType.TypeId),
                    ArcPrimitiveDataType.None => candidateTypes.First(x => x.DataType.TypeId == ArcPersistentData.NoneType.TypeId),
                    ArcPrimitiveDataType.Any => candidateTypes.First(x => x.DataType.TypeId == ArcPersistentData.AnyType.TypeId),
                    _ => throw new NotImplementedException(),
                };
            }
            else
            {
                return GetComplexTypeNode(source, dataType.ComplexType!.Identifier);
            }
        }

        public static ArcScopeTreeDataTypeNode? GetComplexTypeNode(ArcGenerationSource source, ArcFlexibleIdentifier identifier)
        {
            if (identifier.Namespace != null && identifier.Namespace.Any())
            {
                return source.GlobalScopeTree.GetNode<ArcScopeTreeDataTypeNode>(identifier.NameArray);
            }
            else
            {
                return source.DirectlyAccessibleNodes
                    .OfType<ArcScopeTreeDataTypeNode>()
                    .First(c => c.ShortName == identifier.Name);
            }
        }

        public static ArcScopeTreeDataTypeNode? GetDataTypeNode(ArcGenerationSource source, ArcTypeBase dataType)
        {
            return source.GlobalScopeTree.FlattenedNodes.OfType<ArcScopeTreeDataTypeNode>().First(x => x.DataType.TypeId == dataType.TypeId);
        }

        public static ArcScopeTreeGroupNode? GetDataTypeGroupNode(ArcGenerationSource source, ArcDataType dataType)
        {
            var typeNode = GetDataTypeNode(source, dataType);
            return source.GlobalScopeTree
                .FlattenedNodes
                .OfType<ArcScopeTreeGroupNode>()
                .FirstOrDefault(n => n.Id == typeNode?.DataType.TypeId);
        }

        public static ArcScopeTreeGroupNode? GetDataTypeGroupNode(ArcGenerationSource source, ArcComplexType dataType)
        {
            return source.GlobalScopeTree
                .FlattenedNodes
                .OfType<ArcScopeTreeGroupNode>()
                .FirstOrDefault(n => n.Id == dataType.GroupId);
        }
    }
}
