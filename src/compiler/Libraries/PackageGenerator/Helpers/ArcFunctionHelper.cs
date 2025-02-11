using Arc.Compiler.PackageGenerator.Models.Generation;
using Arc.Compiler.PackageGenerator.Models.Scope;
using Arc.Compiler.SyntaxAnalyzer.Models.Function;

namespace Arc.Compiler.PackageGenerator.Helpers
{
    internal class ArcFunctionHelper
    {
        public static long GetFunctionId(ArcGenerationSource source, ArcFunctionCall funcCall, ArcScopeTreeGroupNode? searchUnderGroup = null)
        {
            if (searchUnderGroup == null)
            {
                if (funcCall.Identifier.Namespace != null && funcCall.Identifier.Namespace.Any())
                {
                    var funcDeclarator = source.GlobalScopeTree.GetNode<ArcScopeTreeFunctionNodeBase>(funcCall.Identifier.NameArray)
                        ?? throw new InvalidOperationException("Invalid function node");
                    return funcDeclarator.Id;
                }
                else
                {
                    var funcNode = source.DirectlyAccessibleNodes
                        .OfType<ArcScopeTreeFunctionNodeBase>()
                        .FirstOrDefault(n => n.Name == funcCall.Identifier.Name)
                        ?? throw new InvalidOperationException("Invalid function node");
                    return funcNode.Descriptor.Id;
                }
            }
            else
            {
                var funcNode = searchUnderGroup.GetSpecificChild<ArcScopeTreeGroupFunctionNode>(n => n.Name == funcCall.Identifier.Name);
                return funcNode.Descriptor.Id;
            }
        }
    }
}
