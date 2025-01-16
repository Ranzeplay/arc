using Arc.Compiler.PackageGenerator.Base;
using Arc.Compiler.PackageGenerator.Encoders;
using Arc.Compiler.PackageGenerator.Generators;
using Arc.Compiler.PackageGenerator.Models;
using Arc.Compiler.PackageGenerator.Models.Builtin;
using Arc.Compiler.PackageGenerator.Models.Descriptors;
using Arc.Compiler.PackageGenerator.Models.Descriptors.Group;
using Arc.Compiler.PackageGenerator.Models.Intermediate;
using Arc.Compiler.PackageGenerator.Models.Scope;
using Arc.Compiler.SyntaxAnalyzer.Models;
using Arc.Compiler.SyntaxAnalyzer.Models.Blocks;
using Microsoft.Extensions.Logging;

namespace Arc.Compiler.PackageGenerator
{
    public class Flow
    {
        public static ArcGeneratorContext GenerateUnit(ArcCompilationUnit compilationUnit)
        {
            var ns = compilationUnit.Namespace;
            var result = new ArcGeneratorContext() { Logger = compilationUnit.Logger };
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
            compilationUnit.Logger.LogDebug("Generating compilation unit structure: {}", compilationUnit.Name);

            var symbols = new List<ArcSymbolBase>();
            var tree = new ArcScopeTree();
            var context = new ArcGeneratorContext() { Logger = compilationUnit.Logger };
            compilationUnit.Logger.LogDebug("Loading primitive types");
            context.LoadPrimitiveTypes();

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
                var skelecton = ArcGroupGenerator.GenerateGroupDescriptorSkelecton(context.GenerateSource([compilationUnit.Namespace]), grp);
                symbols.Add(skelecton.Item1);

                current.AddChild(skelecton.Item2);
            }
            compilationUnit.Logger.LogDebug("Generated signatures for {} groups", compilationUnit.Namespace.Groups.Count());

            compilationUnit.Logger.LogDebug("Generating function signatures");
            foreach (var fn in compilationUnit.Namespace.Functions)
            {
                var functionDescriptor = ArcFunctionGenerator.GenerateDescriptor(context.GenerateSource([compilationUnit.Namespace]), fn.Declarator);
                symbols.Add(functionDescriptor);

                var functionScope = new ArcScopeTreeIndividualFunctionNode(functionDescriptor);
                current.AddChild(functionScope);
            }
            compilationUnit.Logger.LogDebug("Generated signatures for {} functions", compilationUnit.Namespace.Functions.Count());

            tree.MergeRoot(ArcPersistentData.BaseTypeScopeTree);

            return new ArcCompilationUnitStructure() { Symbols = symbols, ScopeTree = tree };
        }

        public static IEnumerable<byte> DumpFullByteStream(ArcGeneratorContext context)
        {
            context.Logger.LogInformation("Dumping full byte stream for {}", context.PackageDescriptor.Name);

            var result = new List<byte>();

            result.AddRange([0x20, 0x24]);

            context.ApplyFunctionSymbolRelocation();
            result.AddRange(ArcPackageDescriptorEncoder.Encode(context));
            result.AddRange(ArcSymbolTableEncoder.Encode(context));
            result.AddRange(ArcConstantTableEncoder.Encode(context));
            context.TransformLabelRelocationTargets();
            context.PreApplyRelocation();
            context.ApplyRelocation();
            result.AddRange(context.GeneratedData);

            context.Logger.LogTrace("Generated bytes: {}", BitConverter.ToString([.. context.GeneratedData]).Replace("-", " "));
            context.Logger.LogInformation("Generated {} bytes in total", context.GeneratedData.Count);

            return result;
        }
    }
}
