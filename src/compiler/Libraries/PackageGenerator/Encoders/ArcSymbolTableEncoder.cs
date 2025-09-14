using Arc.Compiler.PackageGenerator.Interfaces;
using Arc.Compiler.PackageGenerator.Models;
using Arc.Compiler.PackageGenerator.Models.Scope;
using Microsoft.Extensions.Logging;

namespace Arc.Compiler.PackageGenerator.Encoders
{
    internal static class ArcSymbolTableEncoder
    {
        public static IEnumerable<byte> Encode(ArcGeneratorContext context)
        {
            var result = new List<byte>();

            var validSymbolNodes = context.GlobalScopeTree.FlattenedNodes
                .OfType<IArcEncodableScopeTreeNode>()
                .OfType<ArcScopeTreeNodeBase>()
                .ToDictionary(x => x.Id, x => (IArcEncodableScopeTreeNode)x);
            result.AddRange(BitConverter.GetBytes((long)validSymbolNodes.Count));
            foreach (var node in validSymbolNodes)
            {
                var iterResult = new List<byte>();
                iterResult.AddRange(BitConverter.GetBytes(node.Key));
                iterResult.AddRange(node.Value.Encode(context.GlobalScopeTree));

                // Print iterResult in hex
                context.Logger.LogTrace("Symbol: {}", BitConverter.ToString([.. iterResult]).Replace("-", " "));

                result.AddRange(iterResult);
            }

            context.Logger.LogInformation("Generated {} symbols into symbol table", validSymbolNodes.Count);

            return result;
        }
    }
}
