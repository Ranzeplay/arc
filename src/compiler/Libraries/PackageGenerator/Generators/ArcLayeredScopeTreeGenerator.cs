using Arc.Compiler.PackageGenerator.Generators.Instructions;
using Arc.Compiler.PackageGenerator.Helpers;
using Arc.Compiler.PackageGenerator.Models;
using Arc.Compiler.PackageGenerator.Models.Builtin;
using Arc.Compiler.PackageGenerator.Models.Descriptors;
using Arc.Compiler.PackageGenerator.Models.Generation;
using Arc.Compiler.PackageGenerator.Models.Intermediate;
using Arc.Compiler.PackageGenerator.Models.Logging;
using Arc.Compiler.PackageGenerator.Models.Scope;
using Arc.Compiler.SyntaxAnalyzer.Models;
using Arc.Compiler.SyntaxAnalyzer.Models.Components;
using Microsoft.Extensions.Logging;

namespace Arc.Compiler.PackageGenerator.Generators
{
    internal static class ArcLayeredScopeTreeGenerator
    {
        private static ArcScopeTree GeneratePrimitiveGroups(ArcCompilationUnit unit)
        {
            var tree = new ArcScopeTree();
            var current = tree.Root;
            var ns = GenerateUnitNamespace(unit, ref current);

            foreach (var group in ns.Groups)
            {
                var node = new ArcScopeTreeGroupNode { SyntaxTree = group, ShortName = group.Identifier.Name };
                current.AddChild(node);

                var complexTypeDescriptor = new ArcComplexType(node) { Identifier = node.Name };
                var typeNode = new ArcScopeTreeDataTypeNode(ArcDataTypeType.Group, complexTypeDescriptor, group.Identifier.Name)
                {
                    ComplexTypeGroup = node
                };
                current.AddChild(typeNode);

                var annotationNode = new ArcScopeTreeAnnotationNode { TargetGroup = node };
                current.AddChild(annotationNode);

                var genericTypeParameters = group.GenericTypes.Select(t => new ArcScopeTreeGenericTypeNode { Identifier = t.Name });
                current.AddChildren(genericTypeParameters);
            }

            return tree;
        }

        private static ArcScopeTree GenerateEnums(ArcCompilationUnit unit)
        {
            var tree = new ArcScopeTree();
            var current = tree.Root;
            var ns = GenerateUnitNamespace(unit, ref current);

            foreach (var enumBlock in ns.Enums)
            {
                var node = ArcEnumGenerator.GenerateEnum(enumBlock);
                current.AddChild(node);
                var typeDesc = new ArcEnumType(node);
                var typeNode = new ArcScopeTreeDataTypeNode(ArcDataTypeType.Enum, typeDesc, node.Name)
                {
                    EnumGroup = node
                };

                current.AddChild(typeNode);
            }

            return tree;
        }

        private static ArcNamespaceBlock GenerateUnitNamespace(ArcCompilationUnit unit, ref ArcScopeTreeNodeBase current)
        {
            // Split namespace
            var ns = unit.Namespace;
            foreach (var part in ns.Identifier.Namespace)
            {
                var namespaceScope = new ArcScopeTreeNamespaceNode(part);
                current = current.AddChild(namespaceScope);
            }

            return ns;
        }

        private static (ArcScopeTree, IEnumerable<ArcCompilationLogBase>) GenerateIndividualFunctions(ArcGenerationSource source, ArcScopeTree mainTree, ArcCompilationUnit unit)
        {
            var logs = new List<ArcCompilationLogBase>();

            var namespaceNode = mainTree.GetNamespace(unit.Namespace.Identifier.Namespace);

            if (namespaceNode == null)
            {
                logs.Add(new ArcSourceLocatableLog(LogLevel.Error, 0, "Namespace not found", source.Name, unit.Namespace.Context));
                return (mainTree, logs);
            }

            foreach (var fn in unit.Namespace.Functions)
            {
                var (descriptor, iterLogs) = ArcFunctionGenerator.GenerateDescriptor<ArcScopeTreeIndividualFunctionNode>(source, fn.Declarator);

                logs.AddRange(iterLogs);

                descriptor.SyntaxTree = fn;
                source.ParentSignature.Locators = [.. source.ParentSignature.Locators.Take(source.ParentSignature.Locators.Count - 1)];
                namespaceNode.AddChild(descriptor);
            }

            return (mainTree, logs);
        }

        public static (IEnumerable<ArcCompilationUnitStructure>, IEnumerable<ArcCompilationLogBase>) GenerateUnitStructure(IEnumerable<ArcCompilationUnit> units, ArcPackageDescriptor packageDescriptor)
        {
            var logs = new List<ArcCompilationLogBase>();
            var unitList = units.ToList();
            var logger = unitList.First().Logger;
            var globalScopeTree = new ArcScopeTree();

            // globalScopeTree.MergeRoot(ArcStdlib.GetTree());
            globalScopeTree.MergeRoot(ArcPersistentData.BaseTypeScopeTree);

            var unitStructures = unitList
                .Select(u => new ArcCompilationUnitStructure(new ArcScopeTree(), u, globalScopeTree))
                .ToList();
            // Merge the scope tree generated by each compilation unit to the global scope tree
            unitStructures.ForEach(us =>
            {
                us.ScopeTree.MergeRoot(GeneratePrimitiveGroups(us.CompilationUnit));
                globalScopeTree.MergeRoot(us.ScopeTree);
            });
            
            // Generate the enums for each compilation unit
            unitStructures.ForEach(us =>
            {
                var context = new ArcGeneratorContext
                {
                    Logger = logger,
                    GlobalScopeTree = globalScopeTree,
                    PackageDescriptor = packageDescriptor
                };
                _ = context.GenerateSource([us.CompilationUnit.Namespace], us.LinkedNamespaces);
                var enumTree = GenerateEnums(us.CompilationUnit);
                us.ScopeTree.MergeRoot(enumTree, true);
                globalScopeTree.MergeRoot(enumTree);
            });

            // Generate the group structure for each group node
            unitStructures.ForEach(us =>
            {
                us.ScopeTree.FlattenedNodes.OfType<ArcScopeTreeGroupNode>()
                    .ToList()
                    .ForEach(n =>
                    {
                        var context = new ArcGeneratorContext
                        {
                            Logger = logger,
                            GlobalScopeTree = globalScopeTree,
                            PackageDescriptor = packageDescriptor
                        };
                        var source = context.GenerateSource([us.CompilationUnit.Namespace], n, us.LinkedNamespaces);
                        n.Annotations = n.SyntaxTree.Annotations
                            .ToDictionary(
                                a => ArcAnnotationHelper.FindAnnotationNode(source, a),
                                a => a.CallArguments.Select(ca => ca.Expression)
                            );
                        n.ExpandSubDescriptors(source);
                    });
            });

            // Generate individual functions for each individual function node in the compilation unit
            unitStructures.ForEach(us =>
            {
                var context = new ArcGeneratorContext
                {
                    Logger = logger,
                    GlobalScopeTree = globalScopeTree,
                    PackageDescriptor = packageDescriptor
                };
                var source = context.GenerateSource([us.CompilationUnit.Namespace], us.LinkedNamespaces);
                var (individualFunctionTree, genLogs) = GenerateIndividualFunctions(source, us.ScopeTree, us.CompilationUnit);
                logs.AddRange(genLogs);

                us.ScopeTree.MergeRoot(individualFunctionTree, true);
                globalScopeTree.MergeRoot(us.ScopeTree, true);
            });

            return (unitStructures, logs);
        }
    }
}