using Arc.Compiler.PackageGenerator.Generators.Instructions;
using Arc.Compiler.PackageGenerator.Models;
using Arc.Compiler.PackageGenerator.Models.Builtin;
using Arc.Compiler.PackageGenerator.Models.Descriptors.Group;
using Arc.Compiler.PackageGenerator.Models.Generation;
using Arc.Compiler.PackageGenerator.Models.Intermediate;
using Arc.Compiler.PackageGenerator.Models.Scope;
using Arc.Compiler.SyntaxAnalyzer.Models;

namespace Arc.Compiler.PackageGenerator.Generators
{
    internal class LayeredScopeTreeGenerator
    {
        public static ArcScopeTree GeneratePrimitiveGroups(ArcCompilationUnit unit)
        {
            var tree = new ArcScopeTree();
            var current = tree.Root;

            // Split namespace
            var ns = unit.Namespace;
            foreach (var part in ns.Identifier.Namespace ?? [])
            {
                var namespaceScope = new ArcScopeTreeNamespaceNode(part);
                current = current.AddChild(namespaceScope);
            }

            var nsSignature = current.Signature;
            foreach (var group in ns.Groups)
            {
                var descriptor = new ArcGroupDescriptor() { Name = nsSignature + "+" + group.GetSignature() };

                var complexTypeDescriptor = new ArcComplexType(descriptor) { Name = descriptor.Name };
                var typeNode = new ArcScopeTreeDataTypeNode(complexTypeDescriptor);
                var typeNode = new ArcScopeTreeDataTypeNode(complexTypeDescriptor, group.Identifier.Name);

                var node = new ArcScopeTreeGroupNode(descriptor) { SyntaxTree = group };
                node.AddChild(typeNode);
                current.AddChild(typeNode);

                current = current.AddChild(node);
                current.AddChild(node);
            }

            return tree;
        }

        public static ArcScopeTree GenerateIndividualFunctions(ArcGenerationSource source, ArcScopeTree mainTree, ArcCompilationUnit unit)
        {
            var namespaceNode = mainTree.GetNamespace(unit.Namespace.Identifier.Namespace);
            foreach (var fn in unit.Namespace.Functions)
            {
                var descriptor = ArcFunctionGenerator.GenerateDescriptor(source, fn.Declarator);
                source.ParentSignature.Locators = source.ParentSignature.Locators.Take(source.ParentSignature.Locators.Count - 1).ToList();
                var node = new ArcScopeTreeIndividualFunctionNode(descriptor) { SyntaxTree = fn };
                namespaceNode.AddChild(node);
            }

            return mainTree;
        }

        public static IEnumerable<ArcCompilationUnitStructure> GenerateUnitStructure(IEnumerable<ArcCompilationUnit> units)
        {
            var logger = units.First().Logger;

            var pairs = units.Select(u => (u, GeneratePrimitiveGroups(u))).ToList();
            pairs.ForEach(p =>
            {
                var u = p.u;
                var t = p.Item2;

                var availableTree = pairs.Select(p => p.Item2)
                .Aggregate((a, b) =>
                {
                    a.MergeRoot(b);
                    return a;
                });
                availableTree.MergeRoot(ArcPersistentData.BaseTypeScopeTree);

                t.FlattenedNodes.OfType<ArcScopeTreeGroupNode>()
                    .ToList()
                    .ForEach(n =>
                    {
                        var context = new ArcGeneratorContext() { Logger = logger, GlobalScopeTree = t };
                        var source = context.GenerateSource([u.Namespace], n);
                        n.ExpandSubDescriptors(source);
                    });

                t.MergeRoot(availableTree, true);
            });

            pairs.ForEach(p =>
            {
                var u = p.u;
                var t = p.Item2;

                var availableTree = pairs.Select(p => p.Item2)
                .Aggregate((a, b) =>
                {
                    a.MergeRoot(b); return a;
                });

                var namespaceNode = t.GetNamespace(u.Namespace.Identifier.Namespace);
                var context = new ArcGeneratorContext() { Logger = logger, GlobalScopeTree = t };
                var source = context.GenerateSource([u.Namespace], namespaceNode);
                var individualFunctionTree = GenerateIndividualFunctions(source, t, u);

                t.MergeRoot(individualFunctionTree, true);

                t.MergeRoot(availableTree, true);
            });

            return pairs.Select(p => new ArcCompilationUnitStructure(p.Item2, p.u));
        }
    }
}
