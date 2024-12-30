using Arc.Compiler.PackageGenerator.Base;
using Arc.Compiler.PackageGenerator.Generators;
using Arc.Compiler.PackageGenerator.Models;
using Arc.Compiler.PackageGenerator.Models.Descriptors;
using Arc.Compiler.PackageGenerator.Models.Descriptors.Group;
using Arc.Compiler.PackageGenerator.Models.Intermediate;
using Arc.Compiler.SyntaxAnalyzer.Models;
using Arc.Compiler.SyntaxAnalyzer.Models.Blocks;

namespace Arc.Compiler.PackageGenerator
{
    public class Flow
    {
        public static ArcGeneratorContext GenerateUnit(ArcCompilationUnit compilationUnit)
        {
            var ns = compilationUnit.Namespace;
            var result = new ArcGeneratorContext();
            result.LoadPrimitiveTypes();

            var structure = GenerateUnitStructure(compilationUnit);
            structure.Symbols = structure.Symbols.Concat(
                structure.Symbols
                    .OfType<ArcGroupDescriptor>()
                    .SelectMany(x => x.ExpandSymbols())
            );
            result.Append(structure);

            foreach (var fn in compilationUnit.Namespace.Functions)
            {
                result.Append(ArcFunctionGenerator.Generate(result.GenerateSource([compilationUnit.Namespace]), fn, false));
            }

            foreach (var grp in compilationUnit.Namespace.Groups)
            {
                foreach (var fn in grp.Functions)
                {
                    var fnResult = ArcFunctionGenerator.Generate(result.GenerateSource([compilationUnit.Namespace, grp]), (ArcBlockIndependentFunction)fn, false);
                    result.Append(fnResult);
                }
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
                var groupDescriptor = ArcGroupGenerator.GenerateGroupDescriptorSkelecton(context.GenerateSource([compilationUnit.Namespace]), grp);
                result.Add(groupDescriptor);
            }

            // Generate function signatures
            foreach (var fn in compilationUnit.Namespace.Functions)
            {
                var functionDescriptor = ArcFunctionGenerator.GenerateDescriptor(context.GenerateSource([compilationUnit.Namespace]), fn.Declarator);
                result.Add(functionDescriptor);
            }

            return new ArcCompilationUnitStructure() { Symbols = result };
        }

        public static IEnumerable<byte> DumpFullByteStream(ArcGeneratorContext context)
        {
            var result = new List<byte>();

            result.AddRange([0x20, 0x24]);

            result.AddRange(ArcDescriptorSerializer.SerializePackageDescriptor(context));
            result.AddRange(ArcDescriptorSerializer.SerializeSymbolTable(context));
            result.AddRange(ArcDescriptorSerializer.SerializeConstantTable(context));
            context.TransformLabelRelocationTargets();
            context.ApplyRelocation();
            result.AddRange(context.GeneratedData);

            Console.WriteLine(BitConverter.ToString(context.GeneratedData.ToArray()).Replace("-", " "));
            Console.WriteLine($"Commands generated, {context.GeneratedData.Count()} bytes in total");

            return result;
        }
    }
}
