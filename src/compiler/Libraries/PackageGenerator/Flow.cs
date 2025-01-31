using Arc.Compiler.PackageGenerator.Base;
using Arc.Compiler.PackageGenerator.Generators;
using Arc.Compiler.PackageGenerator.Generators.Instructions;
using Arc.Compiler.PackageGenerator.Models;
using Arc.Compiler.PackageGenerator.Models.Builtin;
using Arc.Compiler.PackageGenerator.Models.Descriptors;
using Arc.Compiler.PackageGenerator.Models.Intermediate;
using Arc.Compiler.PackageGenerator.Models.Scope;
using Arc.Compiler.SyntaxAnalyzer.Models;
using Microsoft.Extensions.Logging;
using System.Collections.Immutable;

namespace Arc.Compiler.PackageGenerator
{
    public class Flow
    {
        public static ArcGeneratorContext GenerateUnit(ArcCompilationUnit compilationUnit)
        {
            var ns = compilationUnit.Namespace;
            var result = new ArcGeneratorContext() { Logger = compilationUnit.Logger };

            var structure = LayeredScopeTreeGenerator.GenerateUnitStructure([compilationUnit]).First();
            result.SearchTree = structure.ScopeTree;

            var funcs = structure.ScopeTree
                .GetNodes<ArcScopeTreeIndividualFunctionNode>();
            foreach (var fn in funcs)
            {
                var fnResult = ArcFunctionGenerator.Generate(result.GenerateSource([compilationUnit.Namespace], fn), fn.SyntaxTree, false);
                fn.GenerationResult = fnResult;
            }

            var groups = structure.ScopeTree
                .GetNodes<ArcScopeTreeGroupNode>();
            foreach (var grp in groups)
            {
                var groupFns = grp.GetChildren<ArcScopeTreeGroupFunctionNode>();
                foreach (var fn in groupFns)
                {
                    var fnResult = ArcFunctionGenerator.Generate(result.GenerateSource([compilationUnit.Namespace], fn), fn.SyntaxTree, false);
                    fn.GenerationResult = fnResult;
                }
            }

            structure.ScopeTree
                .GetNodes<ArcScopeTreeIndividualFunctionNode>()
                .ToImmutableList()
                .ForEach(t =>
                {
                    t.Descriptor.EntrypointPos = result.GeneratedData.Count;
                    result.Append(t.GenerationResult);
                });

            structure.ScopeTree
                .GetNodes<ArcScopeTreeGroupFunctionNode>()
                .ToImmutableList()
                .ForEach(t =>
                {
                    t.Descriptor.EntrypointPos = result.GeneratedData.Count;
                    result.Append(t.GenerationResult);
                });

            return result;
        }

        public static ArcCompilationUnitStructure GenerateUnitStructure(ArcCompilationUnit compilationUnit)
        {
            compilationUnit.Logger.LogDebug("Generating compilation unit structure: {}", compilationUnit.Name);

            var symbols = new List<ArcSymbolBase>();
            var tree = new ArcScopeTree();
            var context = new ArcGeneratorContext() { Logger = compilationUnit.Logger };
            compilationUnit.Logger.LogDebug("Loading primitive types");

            var ns = compilationUnit.Namespace;
            symbols.Add(new ArcNamespaceDescriptor { Name = ns.GetSignature() });

            // Split namespace
            var current = tree.Root;
            foreach (var part in ns.Identifier.Namespace ?? [])
            {
                var namespaceScope = new ArcScopeTreeNamespaceNode(part);
                current = current.AddChild(namespaceScope);
            }

            compilationUnit.Logger.LogDebug("Generating group signatures");
            foreach (var grp in compilationUnit.Namespace.Groups)
            {
                var skelecton = ArcGroupGenerator.GenerateGroupDescriptorSkelecton(context.GenerateSource([compilationUnit.Namespace], null!), grp);
                symbols.Add(skelecton.Item1);

                current.AddChild(skelecton.Item2);
            }
            compilationUnit.Logger.LogDebug("Generated signatures for {} groups", compilationUnit.Namespace.Groups.Count());

            compilationUnit.Logger.LogDebug("Generating function signatures");
            foreach (var fn in compilationUnit.Namespace.Functions)
            {
                var functionDescriptor = ArcFunctionGenerator.GenerateDescriptor(context.GenerateSource([compilationUnit.Namespace], null!), fn.Declarator);
                symbols.Add(functionDescriptor);

                var functionScope = new ArcScopeTreeIndividualFunctionNode(functionDescriptor) { SyntaxTree = fn };
                current.AddChild(functionScope);
            }
            compilationUnit.Logger.LogDebug("Generated signatures for {} functions", compilationUnit.Namespace.Functions.Count());

            tree.MergeRoot(ArcPersistentData.BaseTypeScopeTree);

            return new(tree);
        }
    }
}
