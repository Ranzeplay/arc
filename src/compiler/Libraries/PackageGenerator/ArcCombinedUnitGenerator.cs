using Arc.Compiler.PackageGenerator.AnnotationProcessors;
using Arc.Compiler.PackageGenerator.Generators;
using Arc.Compiler.PackageGenerator.Generators.Instructions;
using Arc.Compiler.PackageGenerator.Models;
using Arc.Compiler.PackageGenerator.Models.Builtin;
using Arc.Compiler.PackageGenerator.Models.Builtin.Stdlib;
using Arc.Compiler.PackageGenerator.Models.Generation;
using Arc.Compiler.PackageGenerator.Models.Scope;
using Arc.Compiler.PackageGenerator.StdlibSource;
using Arc.Compiler.SyntaxAnalyzer.Generated.ANTLR;
using Arc.Compiler.SyntaxAnalyzer.Models;
using System.Collections.Immutable;

namespace Arc.Compiler.PackageGenerator
{
    public class ArcCombinedUnitGenerator
    {
        public static ArcGeneratorContext GenerateUnits(IEnumerable<ArcCompilationUnit> compilationUnits, bool withStd = true)
        {
            var structures = ArcLayeredScopeTreeGenerator.GenerateUnitStructure(compilationUnits);

            if (withStd)
            {
                structures = [..structures, ..ArcStdlibLoader.Load(compilationUnits.First().Logger)];
            }

            var result = new ArcGeneratorContext()
            {
                Logger = compilationUnits.First().Logger,
                GlobalScopeTree = null!
            };

            var globalScopeTree = structures
                .Select(s => s.ScopeTree)
                // .Append(withStd ? ArcStdlib.GetTree() : new ArcScopeTree())
                .Append(ArcPersistentData.BaseTypeScopeTree)
                .Aggregate((a, b) =>
                {
                    a.MergeRoot(b);
                    return a;
                });

            ArcPostScopeTreeGenerationProcessor.All(ref globalScopeTree);

            foreach (var structure in structures)
            {
                var unit = structure.CompilationUnit;

                var ns = unit.Namespace;
                var iterContext = new ArcGeneratorContext
                {
                    Logger = unit.Logger,
                    GlobalScopeTree = globalScopeTree
                };

                var genSource = iterContext.GenerateSource([unit.Namespace]);

                genSource.LinkedNamespaces = structure.LinkedNamespaces;

                // Generate functions

                var funcs = structure.ScopeTree
                    .GetNodes<ArcScopeTreeIndividualFunctionNode>();
                foreach (var fn in funcs)
                {
                    genSource.CurrentNode = fn;

                    if (fn.Annotations.Keys.Any(k => k.Signature == "NArc+NCompilation+ADeclaratorOnly"))
                    {
                        fn.BlockLength = 0;
                        fn.GenerationResult = new ArcPartialGenerationResult();
                    }
                    else
                    {
                        var fnResult = ArcFunctionGenerator.Generate<ArcSourceCodeParser.Arc_function_blockContext, ArcScopeTreeIndividualFunctionNode>(genSource, fn, fn.SyntaxTree);
                        fn.BlockLength = fnResult.GeneratedData.Count;
                        fn.GenerationResult = fnResult;
                    }
                }

                var groups = structure.ScopeTree
                    .GetNodes<ArcScopeTreeGroupNode>();
                foreach (var grp in groups)
                {
                    var groupFns = grp.GetChildren<ArcScopeTreeGroupFunctionNode>();
                    foreach (var fn in groupFns)
                    {
                        genSource.CurrentNode = fn;

                        if (fn.Annotations.Keys.Any(k => k.Signature == "NArc+NCompilation+ADeclaratorOnly"))
                        {
                            fn.BlockLength = 0;
                            fn.GenerationResult = new ArcPartialGenerationResult();
                        }
                        else
                        {
                            var fnResult = ArcFunctionGenerator.Generate<ArcSourceCodeParser.Arc_group_functionContext, ArcScopeTreeGroupFunctionNode>(genSource, fn, fn.SyntaxTree);
                            fn.BlockLength = fnResult.GeneratedData.Count;
                            fn.GenerationResult = fnResult;
                        }
                    }
                }

                structure.ScopeTree
                    .GetNodes<ArcScopeTreeFunctionNodeBase>()
                    .ToImmutableList()
                    .ForEach(t =>
                    {
                        // Assign entrypoint location
                        t.EntrypointPos = iterContext.GeneratedData.Count + result.GeneratedData.Count;
                        if (t.GenerationResult != null)
                        {
                            // Append the generated data
                            iterContext.Append(t.GenerationResult);
                        }
                    });

                result.Append(iterContext);
            }

            result.GlobalScopeTree = globalScopeTree;
            return result;
        }
    }
}
