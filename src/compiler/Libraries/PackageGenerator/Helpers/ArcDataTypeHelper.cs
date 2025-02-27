using Arc.Compiler.PackageGenerator.Base;
using Arc.Compiler.PackageGenerator.Models.Builtin;
using Arc.Compiler.PackageGenerator.Models.Generation;
using Arc.Compiler.PackageGenerator.Models.Intermediate;
using Arc.Compiler.PackageGenerator.Models.Scope;
using Arc.Compiler.SyntaxAnalyzer.Models.Data.DataType;

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
                var typeIdentifier = dataType.ComplexType!.Identifier;

                if (typeIdentifier.Namespace != null && typeIdentifier.Namespace.Any())
                {
                    return source.GlobalScopeTree.GetNode<ArcScopeTreeDataTypeNode>(typeIdentifier.NameArray);
                }
                else
                {
                    return source.DirectlyAccessibleNodes
                        .OfType<ArcScopeTreeDataTypeNode>()
                        .First(c => c.ShortName == typeIdentifier.Name);
                }
            }
        }

        public static ArcScopeTreeDataTypeNode? GetDataTypeNode(ArcGenerationSource source, ArcTypeBase dataType)
        {
            return source.GlobalScopeTree.FlattenedNodes.OfType<ArcScopeTreeDataTypeNode>().First(x => x.DataType.Id == dataType.Id);
        }

        public static ArcScopeTreeGroupNode? GetDataTypeGroupNode(ArcGenerationSource source, ArcDataType dataType)
        {
            var typeNode = GetDataTypeNode(source, dataType);
            return source.GlobalScopeTree
                .FlattenedNodes
                .OfType<ArcScopeTreeGroupNode>()
                .FirstOrDefault(n => n.Id == typeNode.DataType.TypeId);
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
