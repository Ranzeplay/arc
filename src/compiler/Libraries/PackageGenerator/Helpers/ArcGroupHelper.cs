using Arc.Compiler.PackageGenerator.Models.Generation;
using Arc.Compiler.PackageGenerator.Models.Scope;
using Arc.Compiler.SyntaxAnalyzer.Models.Function;

namespace Arc.Compiler.PackageGenerator.Helpers;

public static class ArcGroupHelper
{
    public static ArcScopeTreeGroupFieldNode? ResolveField(ArcScopeTreeGroupNode group, string fieldName,
        ArcGenerationSource source)
    {
        // Search from current group first
        var field = group.Fields.FirstOrDefault(f => f.Name == fieldName);
        if (field != null)
        {
            return field;
        }

        // Search from base groups if not found
        return group.Derivations
            .Select(d => d.Target)
            .OfType<ArcScopeTreeDataTypeNode>()
            .Where(n => n.ComplexTypeGroup != null)
            .Select(n => n.ComplexTypeGroup!)
            .SelectMany(n => n.Fields)
            .FirstOrDefault(f => f.Name == fieldName);
    }
    
    public static ArcScopeTreeGroupFunctionNode? ResolveSelfFunction(ArcScopeTreeGroupNode group, ArcFunctionCall call, ArcGenerationSource source)
    {
        // Search from current group first
        var fn = group.Functions
            .FirstOrDefault(f => f.IsSelfFunction &&
                                 f.Name == call.Identifier.Name &&
                                 f.Parameters.Count() == call.Arguments.Count() + 1);
        if (fn != null)
        {
            return fn;
        }

        // Search from base groups if not found
        return group.Derivations
            .Select(d => d.Target)
            .Where(l => source.GlobalScopeTree.GetNodeById(l.ProxyTypeId) != null)
            .Select(l => source.GlobalScopeTree.FlattenedNodes.First(n => n.Id == l.ProxyTypeId))
            .OfType<ArcScopeTreeGroupNode>()
            .SelectMany(n => n.Functions)
            .FirstOrDefault(f => f.IsSelfFunction &&
                                 f.Name == call.Identifier.Name &&
                                 f.Parameters.Count() == call.Arguments.Count() + 1);
    }
}