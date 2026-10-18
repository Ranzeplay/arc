using Arc.Compiler.PackageGenerator.Ginkgo.Scope;
using Arc.Compiler.PackageGenerator.Models.Intermediate;
using Arc.Compiler.PackageGenerator.Models.Descriptors;
using Arc.Compiler.PackageGenerator.Models.Descriptors.Function;
using Arc.Compiler.SyntaxAnalyzer.Models.Components;
using Legacy = Arc.Compiler.PackageGenerator.Models.Scope;

namespace Arc.Compiler.PackageGenerator.Ginkgo.Transformation;

/// <summary>Reuses resolved declarations while keeping Ginkgo symbols separate from legacy bytecode.</summary>
public static class ArcGinkgoScopeAdapter
{
    public static ArcGinkgoUnitGenerationContext CreateContext(ArcCompilationUnitStructure structure)
    {
        var mapped = new Dictionary<Legacy.ArcScopeTreeNodeBase, ArcScopeTreeNodeBase>(ReferenceEqualityComparer.Instance);
        var tree = new ArcScopeTree();
        mapped.Add(structure.GlobalScopeTree.Root, tree.Root);
        CopyChildren(structure.GlobalScopeTree.Root, tree.Root);

        foreach (var (source, target) in mapped)
        {
            if (source is Legacy.ArcScopeTreeDataTypeNode oldType && target is ArcScopeTreeDataTypeNode type)
            {
                if (oldType.ComplexTypeGroup is not null && mapped.TryGetValue(oldType.ComplexTypeGroup, out var group))
                    type.ComplexTypeGroup = (ArcScopeTreeGroupNode)group;
                if (oldType.EnumType is not null && mapped.TryGetValue(oldType.EnumType, out var enumeration))
                    type.EnumType = (ArcScopeTreeEnumNode)enumeration;
                // Primitive type IDs are part of the existing language contract.
                if (oldType.IsInternal) type.Id = checked((uint)oldType.Id);
            }
            if (source is Legacy.ArcScopeTreeFunctionNodeBase oldFunction && target is ArcScopeTreeFunctionNodeBase function)
            {
                function.Parameters = oldFunction.Parameters.Select(parameter => new ArcParameterDescriptor
                {
                    RawFullName = parameter.RawFullName,
                    DataType = CopyDeclaration(parameter.DataType, parameter.DataType.SyntaxTree)
                }).ToArray();
                function.ReturnValueType = oldFunction.ReturnValueType;
                function.Accessibility = oldFunction.Accessibility;
                function.Annotations = oldFunction.Annotations;
            }
        }
        // FlattenedNodes deduplicates IDs, so inspect the mapped nodes instead.
        if (mapped.Values.GroupBy(n => n.Id).Any(g => g.Count() > 1))
            throw new InvalidOperationException("Ginkgo symbol ID collision");
        return new ArcGinkgoUnitGenerationContext
        {
            ScopeTree = tree, CompilationUnit = structure.CompilationUnit,
            LinkedNamespaces = structure.LinkedNamespaces.Select(n => tree.GetNamespaceByName(
                string.Join("::", NamespacePath(n)))).OfType<ArcScopeTreeNamespaceNode>()
        };

        static IEnumerable<string> NamespacePath(Legacy.ArcScopeTreeNamespaceNode node)
            => node.GetAncestors().OfType<Legacy.ArcScopeTreeNamespaceNode>().Reverse().Select(n => n.Name);

        static ArcDataDeclarationDescriptor CopyDeclaration(ArcDataDeclarationDescriptor declaration,
            ArcDataDeclarator? syntax) => new()
        {
            Type = declaration.Type,
            Dimension = declaration.Dimension,
            AllowNone = declaration.AllowNone,
            Mutability = syntax?.Mutability ?? declaration.Mutability,
            SyntaxTree = syntax!
        };

        void CopyChildren(Legacy.ArcScopeTreeNodeBase parent, ArcScopeTreeNodeBase destination)
        {
            foreach (var child in parent.Children)
            {
                ArcScopeTreeNodeBase? copy = child switch
                {
                    Legacy.ArcScopeTreeNamespaceNode n => new ArcScopeTreeNamespaceNode(n.Name),
                    Legacy.ArcScopeTreeDataTypeNode n => new ArcScopeTreeDataTypeNode(n.ArcDataTypeType, n.DataType, n.ShortName)
                        { IsInternal = n.IsInternal },
                    Legacy.ArcScopeTreeGroupNode n => new ArcScopeTreeGroupNode
                        { SyntaxTree = n.SyntaxTree, ShortName = n.ShortName, Accessibility = n.Accessibility, Derivations = n.Derivations },
                    Legacy.ArcScopeTreeIndividualFunctionNode n => new ArcScopeTreeIndividualFunctionNode { SyntaxTree = n.SyntaxTree },
                    Legacy.ArcScopeTreeGroupFunctionNode n => new ArcScopeTreeGroupFunctionNode { SyntaxTree = n.SyntaxTree },
                    Legacy.ArcScopeTreeLifecycleFunctionNode n => new ArcScopeTreeLifecycleFunctionNode { SyntaxTree = n.SyntaxTree },
                    Legacy.ArcScopeTreeGroupFieldNode n => new ArcScopeTreeGroupFieldNode
                        { DataType = CopyDeclaration(n.DataType,
                            (parent as Legacy.ArcScopeTreeGroupNode)?.SyntaxTree.Fields
                                .FirstOrDefault(f => f.DataDeclarator.Identifier.Name == n.IdentifierName)?.DataDeclarator),
                            Accessibility = n.Accessibility, IdentifierName = n.IdentifierName, Annotations = n.Annotations },
                    Legacy.ArcScopeTreeEnumNode n => new ArcScopeTreeEnumNode { SyntaxTree = n.SyntaxTree, Annotations = n.Annotations },
                    Legacy.ArcScopeTreeEnumMemberNode n => new ArcScopeTreeEnumMemberNode { SyntaxTree = n.SyntaxTree, Annotations = n.Annotations },
                    Legacy.ArcScopeTreeGenericTypeNode n => new ArcScopeTreeGenericTypeNode
                        { Identifier = n.Identifier, ConstraintType = n.ConstraintType },
                    _ => null
                };
                if (copy is null) continue;
                destination.AddChild(copy);
                mapped.Add(child, copy);
                CopyChildren(child, copy);
            }
        }
    }
}
