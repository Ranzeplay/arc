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
                    IsArray = field.DataDeclarator.DataType.IsArray,
                    MemoryStorageType = field.DataDeclarator.DataType.MemoryStorageType,
                },
                IdentifierName = field.DataDeclarator.Identifier.Name,
            };

            return result;
        }
    }
}
