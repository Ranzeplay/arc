using Arc.Compiler.PackageGenerator.Generators;
using Arc.Compiler.PackageGenerator.Generators.Instructions;
using Arc.Compiler.PackageGenerator.Models;
using Arc.Compiler.PackageGenerator.Models.Builtin.Stdlib;
using Arc.Compiler.PackageGenerator.Models.Scope;
using Arc.Compiler.SyntaxAnalyzer.Models;
using System.Collections.Immutable;

namespace Arc.Compiler.PackageGenerator
{
    public class Flow
    {
        public static ArcGeneratorContext GenerateUnits(IEnumerable<ArcCompilationUnit> compilationUnits)
        {
            var structures = LayeredScopeTreeGenerator.GenerateUnitStructure(compilationUnits);

            var result = new ArcGeneratorContext()
            {
                Logger = compilationUnits.First().Logger,
                GlobalScopeTree = null!
            };

            var globalScopeTree = structures
                .Select(s => s.ScopeTree)
                .Append(ArcStdlib.GetTree())
                .Aggregate((a, b) =>
                {
                    a.MergeRoot(b);
                    return a;
                });

            foreach (var structure in structures)
            {
                var unit = structure.CompilationUnit;

                var ns = unit.Namespace;
                var iterResult = new ArcGeneratorContext
                {
                    Logger = unit.Logger,
                    GlobalScopeTree = globalScopeTree
                };

                var genSource = iterResult.GenerateSource([unit.Namespace]);

                genSource.LinkedNamespaces = unit.LinkedSymbols
                    .SelectMany(ls => structures.Select(s => s.ScopeTree)
                                        .Select(t => t.GetNamespace(ls.Identifier.Namespace))
                    )
                    .TakeWhile(n => n != null)
                    .Append(structure.ScopeTree.GetNamespace(ns.Identifier.Namespace))
                    .Cast<ArcScopeTreeNamespaceNode>() ?? [];

                var funcs = structure.ScopeTree
                    .GetNodes<ArcScopeTreeIndividualFunctionNode>();
                foreach (var fn in funcs)
                {
                    genSource.CurrentNode = fn;
                    var fnResult = ArcFunctionGenerator.Generate(genSource, fn.SyntaxTree, false);
                    fn.Descriptor.BlockLength = fnResult.GeneratedData.Count;
                    fn.GenerationResult = fnResult;
                }

                var groups = structure.ScopeTree
                    .GetNodes<ArcScopeTreeGroupNode>();
                foreach (var grp in groups)
                {
                    var groupFns = grp.GetChildren<ArcScopeTreeGroupFunctionNode>();
                    foreach (var fn in groupFns)
                    {
                        genSource.CurrentNode = fn;
                        var fnResult = ArcFunctionGenerator.Generate(genSource, fn.SyntaxTree, false);
                        fn.Descriptor.BlockLength = fnResult.GeneratedData.Count;
                        fn.GenerationResult = fnResult;
                    }
                }

                structure.ScopeTree
                    .GetNodes<ArcScopeTreeFunctionNodeBase>()
                    .ToImmutableList()
                    .ForEach(t =>
                    {
                        t.Descriptor.EntrypointPos = iterResult.GeneratedData.Count + result.GeneratedData.Count;
                        if (t.GenerationResult != null)
                        {
                            iterResult.Append(t.GenerationResult);
                        }
                    });

                result.Append(iterResult);
            }

            result.GlobalScopeTree = globalScopeTree;
            return result;
        }
    }
}
