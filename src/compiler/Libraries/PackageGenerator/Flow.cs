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
        public static ArcGeneratorContext GenerateUnit(IEnumerable<ArcCompilationUnit> compilationUnits)
        {
            var structures = LayeredScopeTreeGenerator.GenerateUnitStructure(compilationUnits);

            var result = new ArcGeneratorContext()
            {
                Logger = compilationUnits.First().Logger,
                GlobalScopeTree = null!
            };

            var pairs = compilationUnits.Zip(structures, (unit, structure) => (unit, structure));

            foreach (var pair in pairs)
            {
                var unit = pair.unit;
                var structure = pair.structure;

                var searchTree = LayeredScopeTreeGenerator.GenerateSearchTree(
                    unit,
                    structures.Select(s => s.ScopeTree).Append(ArcStdlib.GetTree())
                );

                var ns = unit.Namespace;
                var iterResult = new ArcGeneratorContext
                {
                    Logger = unit.Logger,
                    GlobalScopeTree = searchTree
                };

                var funcs = structure.ScopeTree
                    .GetNodes<ArcScopeTreeIndividualFunctionNode>();
                foreach (var fn in funcs)
                {
                    var fnResult = ArcFunctionGenerator.Generate(iterResult.GenerateSource([unit.Namespace], fn), fn.SyntaxTree, false);
                    fn.GenerationResult = fnResult;
                }

                var groups = structure.ScopeTree
                    .GetNodes<ArcScopeTreeGroupNode>();
                foreach (var grp in groups)
                {
                    var groupFns = grp.GetChildren<ArcScopeTreeGroupFunctionNode>();
                    foreach (var fn in groupFns)
                    {
                        var fnResult = ArcFunctionGenerator.Generate(iterResult.GenerateSource([unit.Namespace], fn), fn.SyntaxTree, false);
                        fn.GenerationResult = fnResult;
                    }
                }

                structure.ScopeTree
                    .GetNodes<ArcScopeTreeFunctionNodeBase>()
                    .ToImmutableList()
                    .ForEach(t =>
                    {
                        t.Descriptor.EntrypointPos = iterResult.GeneratedData.Count + result.GeneratedData.Count;
                        iterResult.Append(t.GenerationResult);
                    });

                result.Append(iterResult);
            }

            result.GlobalScopeTree = structures
                .Select(s => s.ScopeTree)
                .Append(ArcStdlib.GetTree())
                .Aggregate((a, b) =>
                {
                    a.MergeRoot(b);
                    return a;
                });
            return result;
        }
    }
}
