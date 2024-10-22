using Arc.Compiler.PackageGenerator.Generators;
using Arc.Compiler.PackageGenerator.Models;
using Arc.Compiler.SyntaxAnalyzer.Models;

namespace Arc.Compiler.PackageGenerator
{
    internal class Flow
    {
        public static ArcGeneratorContext GenerateUnit(ArcCompilationUnit compilationUnit)
        {
            var ns = compilationUnit.Namespace;
            var result = new ArcGeneratorContext();
            result.LoadPrimitiveTypes();
            foreach (var fn in compilationUnit.Namespace.Functions)
            {
                result.Append(ArcFunctionGenerator.Generate(result.GenerateSource(), fn, ns));
            }

            return result;
        }
    }
}
