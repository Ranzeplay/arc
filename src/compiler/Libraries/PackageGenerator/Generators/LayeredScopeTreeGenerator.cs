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

                var derivativeTypeDescriptor = new ArcDerivativeType(descriptor) { Name = descriptor.Name };
                var typeNode = new ArcScopeTreeDataTypeNode(derivativeTypeDescriptor);

                var node = new ArcScopeTreeGroupNode(descriptor);
                node.AddChild(typeNode);

                current = current.AddChild(node);
            }

            return tree;
        }

        public static ArcScopeTree GenerateSearchTree(IEnumerable<ArcScopeTree> availableTrees, ArcCompilationUnit unit)
        {
            var fullTree = new ArcScopeTree();
            foreach (var tree in availableTrees)
            {
                fullTree.MergeRoot(tree);
            }

            var linkedNamespaces = unit.LinkedSymbols
                .Select(s => s.Identifier.Namespace)
                .Select(fullTree.GetNamespace!);

            var linkedNamespaceTree = new ArcScopeTree();
            foreach (var ns in linkedNamespaces)
            {
                linkedNamespaceTree.MergeRoot(ns.GetIsolatedTree());
            }

            var searchTree = new ArcScopeTree();
            searchTree.MergeRoot(fullTree);

            return searchTree;
        }

        public static ArcScopeTreeGroupNode ExpandGroup(ArcGenerationSource source, ArcScopeTreeGroupNode node, IEnumerable<ArcScopeTree> availableTrees, ArcCompilationUnit unit)
        {
            var searchTree = GenerateSearchTree(availableTrees, unit);

            var functionNodes = new List<ArcScopeTreeGroupFunctionNode>();
            foreach (var fn in node.SyntaxTree.Functions)
            {
                var fnDescriptor = ArcFunctionGenerator.GenerateDescriptor(source, fn.Declarator);
                node.Descriptor.Functions.Add(fnDescriptor);
                functionNodes.Add(new ArcScopeTreeGroupFunctionNode(fnDescriptor) { SyntaxTree = fn });
                // Remove the last element since after executing the previous statement, there will be a new function in the parent signature
                source.ParentSignature.Locators = source.ParentSignature.Locators.Take(source.ParentSignature.Locators.Count - 1).ToList();
            }

            var fieldNodes = new List<ArcScopeTreeGroupFieldNode>();
            foreach (var field in node.SyntaxTree.Fields)
            {
                var fieldDescriptor = ArcGroupGenerator.GenerateFieldDescriptor(source, field);
                node.Descriptor.Fields.Add(fieldDescriptor);
                fieldNodes.Add(new ArcScopeTreeGroupFieldNode(fieldDescriptor));
            }

            node.AddChildren(functionNodes)
                .AddChildren(fieldNodes);

            return node;
        }

        public static ArcScopeTree GenerateIndividualFunctions(ArcGenerationSource source, ArcScopeTree mainTree, ArcCompilationUnit unit, IEnumerable<ArcScopeTree> availableTrees)
        {
            var searchTree = GenerateSearchTree(availableTrees, unit);

            var namespaceNode = mainTree.GetNamespace(unit.Namespace.Identifier.Namespace!);
            foreach (var fn in unit.Namespace.Functions)
            {
                var descriptor = ArcFunctionGenerator.GenerateDescriptor(source, fn.Declarator);
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
                    a.MergeRoot(b); return a;
                });
                availableTree.MergeRoot(ArcPersistentData.BaseTypeScopeTree);

                var groups = t.FlattenedNodes.OfType<ArcScopeTreeGroupNode>().Select(n =>
                {
                    var context = new ArcGeneratorContext() { Logger = logger, ScopeTree = t };
                    var source = context.GenerateSource([u.Namespace], n);
                    return ExpandGroup(source, n, [availableTree], u);
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
                availableTree.MergeRoot(ArcPersistentData.BaseTypeScopeTree);

                var individualFunctions = t.FlattenedNodes.OfType<ArcScopeTreeIndividualFunctionNode>().Select(n =>
                {
                    var context = new ArcGeneratorContext() { Logger = logger, ScopeTree = t };
                    var source = context.GenerateSource([u.Namespace], n);
                    return GenerateIndividualFunctions(source, t, u, [availableTree]);
                });

                t.MergeRoot(availableTree, true);
            });

            return pairs.Select(p => new ArcCompilationUnitStructure(p.Item2));
        }
    }
}
