using System.Text;
using System.Text.Json;
using Arc.Compiler.PackageGenerator.Ginkgo.Transformation;
using Arc.Compiler.PackageGenerator.Models.Descriptors;
using Arc.Compiler.SyntaxAnalyzer;
using Arc.Compiler.SyntaxAnalyzer.Models;
using Microsoft.Extensions.Logging;

namespace Arc.Compiler.Tests.PackageGeneration;

public class GinkgoArtifact
{
    private static ArcGinkgoArtifact Generate(ArcPackageType type, params string[] sources)
    {
        using var factory = LoggerFactory.Create(_ => { });
        var logger = factory.CreateLogger<GinkgoArtifact>();
        var units = sources.Select((source, index) => new ArcCompilationUnit(
            AntlrAdapter.ParseCompilationUnit(source, logger), logger, $"unit-{index}"));
        return ArcGinkgoArtifactGenerator.Generate(units, ArcPackageDescriptor.Default(type), withStd: false);
    }

    [Test]
    public void ArtifactHasDistinctHeaderAndGinkgoFunctionBytes()
    {
        var artifact = Generate(ArcPackageType.Library,
            "namespace Test { public func main(): none { var value: int = 42; } }");
        Assert.That(artifact.DiscardResult, Is.False);
        using var reader = new BinaryReader(new MemoryStream(artifact.GeneratedData));
        Assert.That(Encoding.ASCII.GetString(reader.ReadBytes(8)), Is.EqualTo("GINKGO\0\0"));
        Assert.That(reader.ReadUInt32(), Is.EqualTo(1));
        var manifestLength = reader.ReadUInt32();
        var codeLength = reader.ReadUInt64();
        using var manifest = JsonDocument.Parse(reader.ReadBytes(checked((int)manifestLength)));
        Assert.That(manifest.RootElement.GetProperty("InstructionSet").GetString(), Is.EqualTo("ginkgo"));
        var function = manifest.RootElement.GetProperty("Functions")[0];
        Assert.That(function.GetProperty("Registers").GetInt64(), Is.EqualTo(2));
        Assert.That(function.GetProperty("External").GetBoolean(), Is.False);
        var bytes = reader.ReadBytes(checked((int)codeLength));
        Assert.That(bytes[0], Is.EqualTo(0x1d)); // load_i
        Assert.That(bytes[^2], Is.EqualTo(0x1b)); // retn
        Assert.That(bytes[^1], Is.Zero); // expect_return_value is false
        Assert.That(reader.BaseStream.Position, Is.EqualTo(reader.BaseStream.Length));
    }

    [TestCase("")]
    [TestCase("var value: int = Arc::Std::Math::AbsInt(1);")]
    [TestCase("var value: decimal = Arc::Std::Math::AbsDecimal(1.0);")]
    [TestCase("call Arc::Std::Console::PrintInteger(1);")]
    public void DefaultStandardLibraryDoesNotLowerUnusedUnsupportedBodies(string body)
    {
        using var factory = LoggerFactory.Create(_ => { });
        var logger = factory.CreateLogger<GinkgoArtifact>();
        var unit = new ArcCompilationUnit(AntlrAdapter.ParseCompilationUnit(
            $"namespace Test {{ public func main(): none {{ {body} }} }}", logger), logger, "user");
        var artifact = ArcGinkgoArtifactGenerator.Generate([unit], ArcPackageDescriptor.Default(ArcPackageType.Library));
        Assert.That(artifact.DiscardResult, Is.False);
        Assert.That(artifact.GeneratedData, Is.Not.Empty);
    }

    [Test]
    public void FunctionsKeepTheirOwnUnitConstantPool()
    {
        var artifact = Generate(ArcPackageType.Library,
            "namespace First { public group FirstValue {} public func first(const value: FirstValue): none { } }",
            "namespace Second { public group SecondValue {} public func second(const value: SecondValue): none { } }");
        Assert.That(artifact.DiscardResult, Is.False);
        using var reader = new BinaryReader(new MemoryStream(artifact.GeneratedData));
        reader.ReadBytes(8);
        reader.ReadUInt32();
        var manifestLength = reader.ReadUInt32();
        reader.ReadUInt64();
        using var manifest = JsonDocument.Parse(reader.ReadBytes(checked((int)manifestLength)));
        var root = manifest.RootElement;
        Assert.That(root.GetProperty("Functions").GetArrayLength(), Is.EqualTo(2));
        var symbols = root.GetProperty("Symbols").EnumerateArray()
            .GroupBy(symbol => symbol.GetProperty("Name").GetString()!)
            .ToDictionary(group => group.Key,
                group => group.First().GetProperty("Id").GetUInt64());
        Assert.That(symbols.Keys, Does.Contain("FirstValue"));
        Assert.That(symbols.Keys, Does.Contain("SecondValue"));
        foreach (var function in root.GetProperty("Functions").EnumerateArray())
        {
            var parameterTypeId = function.GetProperty("Parameters")[0]
                .GetProperty("Type").GetProperty("Id").GetUInt64();
            Assert.That(symbols.Values, Does.Contain(parameterTypeId));
        }
    }

    [Test]
    public void ErrorsPreventArtifactEmission()
    {
        var invalid = Generate(ArcPackageType.Library,
            "namespace Test { public func main(): none { break; } }");
        Assert.That(invalid.DiscardResult, Is.True);
        Assert.That(invalid.GeneratedData, Is.Empty);
        var missingEntrypoint = Generate(ArcPackageType.Executable,
            "namespace Test { public func main(): none {} }");
        Assert.That(missingEntrypoint.DiscardResult, Is.True);
        Assert.That(missingEntrypoint.GeneratedData, Is.Empty);
    }

    [Test]
    public void StandardNamespaceAnnotationsSelectEntrypointsAndExternalDeclarations()
    {
        var artifact = Generate(ArcPackageType.Executable,
            "namespace Arc::Std::Compilation { public group Entrypoint {} public group DeclaratorOnly {} }",
            """
            link Arc::Std::Compilation;
            namespace Test {
                @DeclaratorOnly
                public func external(): int {}
                @Entrypoint
                public func main(): int { return 0; }
            }
            """);
        Assert.That(artifact.DiscardResult, Is.False);
        using var reader = new BinaryReader(new MemoryStream(artifact.GeneratedData));
        reader.ReadBytes(8);
        reader.ReadUInt32();
        var manifestLength = reader.ReadUInt32();
        reader.ReadUInt64();
        using var manifest = JsonDocument.Parse(reader.ReadBytes(checked((int)manifestLength)));
        var root = manifest.RootElement;
        var functions = root.GetProperty("Functions").EnumerateArray().ToArray();
        var main = functions.Single(f => f.GetProperty("Signature").GetString()!.Contains("main"));
        var external = functions.Single(f => f.GetProperty("Signature").GetString()!.Contains("external"));
        Assert.That(root.GetProperty("Package").GetProperty("EntrypointFunctionId").GetUInt64(),
            Is.EqualTo(main.GetProperty("Id").GetUInt64()));
        Assert.That(external.GetProperty("External").GetBoolean(), Is.True);
        Assert.That(external.GetProperty("Length").GetInt64(), Is.Zero);
    }
}
