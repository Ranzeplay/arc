using Arc.Compiler.PackageGenerator.Helpers;
using Arc.Compiler.PackageGenerator.Models.Descriptors;
using Arc.Compiler.PackageGenerator.Models.Descriptors.Group;
using Arc.Compiler.PackageGenerator.Models.Generation;
using Arc.Compiler.SyntaxAnalyzer.Models.Group;

namespace Arc.Compiler.PackageGenerator.Generators.Instructions
{
    internal class ArcGroupGenerator
    {
        public static ArcGroupFieldDescriptor GenerateFieldDescriptor(ArcGenerationSource source, ArcGroupField field)
        {
            source.ParentSignature.Locators.Add(field);
            var result = new ArcGroupFieldDescriptor()
            {
                Name = source.ParentSignature.GetSignature(),
                DataType = new ArcDataDeclarationDescriptor
                {
                    Type = ArcDataTypeHelper.GetDataTypeNode(source, field.DataDeclarator.DataType).DataType,
                    AllowNone = false,
                    Dimension = field.DataDeclarator.DataType.Dimension,
                    MemoryStorageType = field.DataDeclarator.DataType.MemoryStorageType,
                },
                IdentifierName = field.DataDeclarator.Identifier.Name,
                Annotations = field.Annotations
                    .ToDictionary(
                        a => ArcAnnotationHelper.FindAnnotationNode(source, a).Descriptor,
                        a => a.CallArguments.Select(ca => ca.Expression)
                    ),
            };

            return result;
        }
    }
}
