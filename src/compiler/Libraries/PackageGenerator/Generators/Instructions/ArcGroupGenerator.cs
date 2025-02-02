using Arc.Compiler.PackageGenerator.Models.Builtin;
using Arc.Compiler.PackageGenerator.Models.Descriptors;
using Arc.Compiler.PackageGenerator.Models.Descriptors.Group;
using Arc.Compiler.PackageGenerator.Models.Generation;
using Arc.Compiler.PackageGenerator.Models.Intermediate;
using Arc.Compiler.PackageGenerator.Models.Scope;
using Arc.Compiler.SyntaxAnalyzer.Models.Group;

namespace Arc.Compiler.PackageGenerator.Generators.Instructions
{
    internal class ArcGroupGenerator
    {
        public static (ArcGroupDescriptor, ArcScopeTreeGroupNode) GenerateGroupDescriptorSkelecton(ArcGenerationSource source, ArcGroup group)
        {
            source.ParentSignature.Locators.Add(group);
            var descriptor = new ArcGroupDescriptor() { Name = source.ParentSignature.GetSignature() };

            var derivativeTypeDescriptor = new ArcDerivativeType(descriptor) { Name = descriptor.Name };
            var typeNode = new ArcScopeTreeDataTypeNode(derivativeTypeDescriptor);

            var functionNodes = new List<ArcScopeTreeGroupFunctionNode>();
            foreach (var fn in group.Functions)
            {
                var fnDescriptor = ArcFunctionGenerator.GenerateDescriptor(source, fn.Declarator);
                descriptor.Functions.Add(fnDescriptor);
                functionNodes.Add(new ArcScopeTreeGroupFunctionNode(fnDescriptor) { SyntaxTree = fn });
                // Remove the last element since after executing the previous statement, there will be a new function in the parent signature
                source.ParentSignature.Locators = source.ParentSignature.Locators.Take(source.ParentSignature.Locators.Count - 1).ToList();
            }

            var fieldNodes = new List<ArcScopeTreeGroupFieldNode>();
            foreach (var field in group.Fields)
            {
                var fieldDescriptor = GenerateFieldDescriptor(source, field);
                descriptor.Fields.Add(fieldDescriptor);
                fieldNodes.Add(new ArcScopeTreeGroupFieldNode(fieldDescriptor));
            }

            var node = new ArcScopeTreeGroupNode(descriptor);
            node.AddChildren(functionNodes)
                .AddChildren(fieldNodes)
                .AddChild(typeNode);

            return (descriptor, node);
        }

        public static ArcGroupFieldDescriptor GenerateFieldDescriptor(ArcGenerationSource source, ArcGroupField field)
        {
            source.ParentSignature.Locators.Add(field);
            var result = new ArcGroupFieldDescriptor()
            {
                Name = source.ParentSignature.GetSignature(),
                DataType = new ArcDataDeclarationDescriptor
                {
                    Type = Utils.GetDataTypeNode(source, field.DataDeclarator.DataType).DataType,
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
