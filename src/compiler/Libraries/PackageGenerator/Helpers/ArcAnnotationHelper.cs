using Arc.Compiler.PackageGenerator.Models.Descriptors;
using Arc.Compiler.PackageGenerator.Models.Descriptors.Group;
using Arc.Compiler.PackageGenerator.Models.Generation;
using Arc.Compiler.PackageGenerator.Models.Scope;
using Arc.Compiler.SyntaxAnalyzer.Models.Components;

namespace Arc.Compiler.PackageGenerator.Helpers
{
    class ArcAnnotationHelper
    {
        public static ArcScopeTreeAnnotationNode FindAnnotationNode(ArcGenerationSource source, ArcAnnotation annotation)
        {
            var annotationIdentifier = annotation.Identifier;

            if (annotationIdentifier.Namespace != null && annotationIdentifier.Namespace.Any())
            {
                return source.GlobalScopeTree
                    .GetNode<ArcScopeTreeAnnotationNode>(annotationIdentifier.NameArray);
            }
            else
            {
                return source.DirectlyAccessibleNodes
                    .OfType<ArcScopeTreeAnnotationNode>()
                    .First(c => c.Descriptor.TargetGroup.ShortName == annotationIdentifier.Name);
            }
        }

        public static IEnumerable<ArcScopeTreeNodeBase> GenerateEmptyAnnotation(string name, long groupId, long annotationId)
        {
            var groupDescriptor = new ArcGroupDescriptor
            {
                Name = name,
                Id = groupId,
                ShortName = name
            };
            var annotationDescriptor = new ArcAnnotationDescriptor
            {
                Name = name,
                Id = annotationId,
                TargetGroup = groupDescriptor
            };

            var groupNode = new ArcScopeTreeGroupNode(groupDescriptor);
            var annotationNode = new ArcScopeTreeAnnotationNode(annotationDescriptor);

            return [groupNode, annotationNode];
        }
    }
}
