using Arc.Compiler.PackageGenerator.Base;
using Arc.Compiler.PackageGenerator.Generators;
using Arc.Compiler.PackageGenerator.Models;
using Arc.Compiler.PackageGenerator.Models.Descriptors;
using Arc.Compiler.PackageGenerator.Models.Intermediate;
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
                result.Append(ArcFunctionGenerator.Generate(result.GenerateSource([compilationUnit.Namespace]), fn, ns));
            }

            return result;
        }

        public static ArcCompilationUnitStructure GenerateUnitStructure(ArcCompilationUnit compilationUnit)
        {
            var result = new List<ArcSymbolBase>();
            var context = new ArcGeneratorContext();
            context.LoadPrimitiveTypes();

            var ns = compilationUnit.Namespace;
            result.Add(new ArcNamespaceDescriptor { Name = ns.GetSignature() });

            // Generate group signatures
            foreach (var grp in compilationUnit.Namespace.Groups)
            {
                // TODO: Code here
            }

            // Generate function signatures
            foreach (var fn in compilationUnit.Namespace.Functions)
            {
                var functionDescriptor = ArcFunctionGenerator.GenerateDescriptor(context.GenerateSource([compilationUnit.Namespace]), fn.Declarator);
                result.Add(functionDescriptor);
            }

            return new ArcCompilationUnitStructure() { Symbols = result };
        }
    }
}
