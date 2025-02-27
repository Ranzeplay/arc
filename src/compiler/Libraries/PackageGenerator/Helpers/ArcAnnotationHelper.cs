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
                    .First(c => c.TargetGroup.ShortName == annotationIdentifier.Name);
            }
        }

        public static IEnumerable<ArcScopeTreeNodeBase> GenerateEmptyAnnotation(string name, long groupId, long annotationId)
        {
            var groupNode = new ArcScopeTreeGroupNode
            {
                ShortName = name,
                Id = groupId,
            };
            var annotationNode = new ArcScopeTreeAnnotationNode
            {
                TargetGroup = groupNode,
                Id = annotationId
            };

            return [groupNode, annotationNode];
        }
    }
}
