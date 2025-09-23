using Arc.Compiler.PackageGenerator.Helpers;
using Arc.Compiler.PackageGenerator.Models.Builtin;
using Arc.Compiler.PackageGenerator.Models.Descriptors;
using Arc.Compiler.PackageGenerator.Models.Generation;
using Arc.Compiler.PackageGenerator.Models.Logging;
using Arc.Compiler.PackageGenerator.Models.Scope;
using Arc.Compiler.SyntaxAnalyzer.Models.Group;
using Microsoft.Extensions.Logging;

namespace Arc.Compiler.PackageGenerator.Generators.Instructions
{
    internal class ArcGroupGenerator
    {
        public static (ArcScopeTreeGroupFieldNode, IEnumerable<ArcCompilationLogBase>) GenerateFieldDescriptor(ArcGenerationSource source, ArcGroupField field)
        {
            source.ParentSignature.Locators.Add(field);

            var fieldTypeProxy = ArcDataTypeHelper.GetDataType(source, field.DataDeclarator.DataType);
            var fieldTypeNode = ArcDataTypeHelper.GetDataTypeNode(source, fieldTypeProxy!.ResolvedType);

            var result = new ArcScopeTreeGroupFieldNode()
            {
                DataType = new ArcDataDeclarationDescriptor
                {
                    Type = fieldTypeNode?.DataType ?? ArcBaseType.Placeholder(),
                    AllowNone = false,
                    Dimension = field.DataDeclarator.DataType.Dimension,
                },
                IdentifierName = field.DataDeclarator.Identifier.Name,
                Annotations = field.Annotations
                    .ToDictionary(
                        a => ArcAnnotationHelper.FindAnnotationNode(source, a),
                        a => a.CallArguments.Select(ca => ca.Expression)
                    ),
                Accessibility = field.Accessibility,
            };

            if (fieldTypeProxy == null)
            {
                return (result, [new ArcSourceLocatableLog(LogLevel.Error, 0, $"Data type '{field.DataDeclarator.DataType}' not found", source.Name, field.DataDeclarator.DataType.Context)]);
            }
            else
            {
                return (result, []);
            }
        }
    }
}
