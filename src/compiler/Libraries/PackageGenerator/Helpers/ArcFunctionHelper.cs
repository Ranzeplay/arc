using Arc.Compiler.PackageGenerator.Models.Generation;
using Arc.Compiler.PackageGenerator.Models.Logging;
using Arc.Compiler.PackageGenerator.Models.Scope;
using Arc.Compiler.SyntaxAnalyzer.Models.Function;
using Microsoft.Extensions.Logging;

namespace Arc.Compiler.PackageGenerator.Helpers
{
    internal class ArcFunctionHelper
    {
        public static (ulong, IEnumerable<ArcCompilationLogBase>) GetFunctionId(ArcGenerationSource source, ArcFunctionCall funcCall, ArcScopeTreeGroupNode? searchUnderGroup = null)
        {
            if (searchUnderGroup == null)
            {
                if (funcCall.Identifier.Namespace != null && funcCall.Identifier.Namespace.Any())
                {
                    var funcDeclarator = source.GlobalScopeTree
                        .GetNode<ArcScopeTreeFunctionNodeBase>(funcCall.Identifier.NameArray);

                    if (funcDeclarator == null)
                    {
                        return (0, [new ArcSourceLocatableLog(LogLevel.Error, 0, "Function not found", source.Name, funcCall.Context)]);
                    }

                    return (funcDeclarator.Id, []);
                }
                else
                {
                    var funcNode = source.DirectlyAccessibleNodes
                        .OfType<ArcScopeTreeFunctionNodeBase>()
                        .FirstOrDefault(n => n.Name == funcCall.Identifier.Name);

                    if (funcNode == null)
                    {
                        return (0, [new ArcSourceLocatableLog(LogLevel.Error, 0, "Function not found", source.Name, funcCall.Context)]);
                    }

                    return (funcNode.Id, []);
                }
            }
            else
            {
                var funcNode = searchUnderGroup.GetSpecificChild<ArcScopeTreeGroupFunctionNode>(n => n.Name == funcCall.Identifier.Name);

                if (funcNode == null)
                {
                    return (0, [new ArcSourceLocatableLog(LogLevel.Error, 0, "Function not found", source.Name, funcCall.Context)]);
                }

                return (funcNode.Id, []);
            }
        }
    }
}
