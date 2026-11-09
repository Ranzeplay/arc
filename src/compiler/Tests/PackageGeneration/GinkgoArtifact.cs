using System.Text;
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
        var package = ReadPackage(artifact.GeneratedData);
        var function = package.Functions.Single();
        Assert.That(function.Registers, Is.EqualTo(2));
        Assert.That(function.Length, Is.Positive);
        var bytes = package.Code;
        Assert.That(bytes[0], Is.EqualTo(0x1d)); // load_i
        Assert.That(bytes[^2], Is.EqualTo(0x1b)); // retn
        Assert.That(bytes[^1], Is.Zero); // expect_return_value is false
        Assert.That(function.Offset + function.Length, Is.EqualTo(bytes.Length));
        Assert.That(Encoding.UTF8.GetString(artifact.GeneratedData), Does.Not.Contain("NTest"));
        Assert.That(artifact.SourceInformation.SymbolMapping.Values, Has.Some.Contains("NTest"));
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
        var package = ReadPackage(artifact.GeneratedData);
        Assert.That(package.Functions, Has.Count.EqualTo(2));
        Assert.That(package.Functions.Select(f => f.Pool), Is.EquivalentTo(new uint[] { 0, 1 }));
        foreach (var function in package.Functions)
        {
            var type = artifact.SourceInformation.SymbolMapping[function.ParameterTypes.Single()];
            var signature = artifact.SourceInformation.SymbolMapping[function.Id];
            Assert.That(type.Contains("FirstValue"), Is.EqualTo(signature.Contains("first")));
            Assert.That(type.Contains("SecondValue"), Is.EqualTo(signature.Contains("second")));
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
        var package = ReadPackage(artifact.GeneratedData);
        var main = package.Functions.Single(f => artifact.SourceInformation.SymbolMapping[f.Id].Contains("main"));
        var external = package.Functions.Single(f => artifact.SourceInformation.SymbolMapping[f.Id].Contains("external"));
        Assert.That(package.Entrypoint, Is.EqualTo(main.Id));
        Assert.That(external.Length, Is.Zero);
        Assert.That(main.Annotations, Is.Not.Empty);
        Assert.That(main.Annotations.All(artifact.SourceInformation.SymbolMapping.ContainsKey), Is.True);
    }

    [Test]
    public void BinaryConstantsEncodeDoublesAndPreserveUtf8AndUnitIndices()
    {
        var artifact = Generate(ArcPackageType.Library,
            "namespace First { public func first(): none { var text: string = \"Hello world\"; var number: decimal = 1.234567890123456789; } }",
            "namespace Second { public func second(): none { var flag: bool = true; } }");
        Assert.That(artifact.DiscardResult, Is.False);
        var package = ReadPackage(artifact.GeneratedData);
        Assert.That(package.Pools, Has.Count.EqualTo(2));
        Assert.That(Encoding.UTF8.GetString(package.Pools[0][0]), Is.EqualTo("Hello world"));
        using var reader = new BinaryReader(new MemoryStream(package.Pools[0][1]));
        Assert.That(reader.BaseStream.Length, Is.EqualTo(8));
        Assert.That(reader.ReadDouble(), Is.EqualTo((double)1.234567890123456789m));
        Assert.That(package.Pools[1].Single(), Is.EqualTo(new byte[] { 1 }));
        Assert.That(Encoding.UTF8.GetString(artifact.GeneratedData), Does.Not.Contain("unit-0"));
    }

    [Test]
    public void GroupFieldsAndLifecycleFunctionsHaveResolvableBinarySymbols()
    {
        var artifact = Generate(ArcPackageType.Library, """
            namespace Test {
                public group Item {
                    public field var count: int;
                    public constructor(var self: Item, const initial: int): none { self.count = initial; }
                }
                public func main(): none { var item: Item = new Item(1); }
            }
            """);
        Assert.That(artifact.DiscardResult, Is.False);
        var package = ReadPackage(artifact.GeneratedData);
        Assert.That(package.Functions, Has.Count.EqualTo(2));
        Assert.That(package.Functions.All(f => f.Length > 0), Is.True);
        Assert.That(package.Functions.Sum(f => f.Length), Is.EqualTo(package.Code.Length));
    }

    private sealed record Function(uint Id, long Offset, long Length, long Registers, uint Pool,
        uint[] ParameterTypes, uint[] Annotations);

    private sealed class Package
    {
        public uint Entrypoint { get; init; }
        public List<Function> Functions { get; } = [];
        public List<List<byte[]>> Pools { get; } = [];
        public HashSet<uint> SymbolIds { get; } = [];
        public List<uint> References { get; } = [];
        public byte[] Code { get; set; } = [];
    }

    private static Package ReadPackage(byte[] data)
    {
        using var reader = new BinaryReader(new MemoryStream(data));
        Assert.That(reader.ReadBytes(8), Is.EqualTo("GINKGO\0\0"u8.ToArray()));
        Assert.That(reader.ReadUInt32(), Is.EqualTo(2));
        reader.ReadByte(); // Package type.
        reader.ReadBytes(checked((int)reader.ReadUInt64())); // UTF-8 package name.
        reader.ReadUInt64(); // Package version.
        var result = new Package { Entrypoint = reader.ReadUInt32() };
        reader.ReadBytes(5 * 8); // Alignment and reserved table positions, as in the original descriptor.
        var count = reader.ReadUInt32();
        for (uint i = 0; i < count; i++)
        {
            var id = reader.ReadUInt32();
            Assert.That(result.SymbolIds.Add(id), Is.True, "Duplicate symbol ID");
            switch (reader.ReadByte())
            {
                case 1: break; // Namespace.
                case 2:
                    var offset = reader.ReadInt64();
                    var length = reader.ReadInt64();
                    Declaration();
                    var parameters = new uint[checked((int)reader.ReadUInt32())];
                    for (var p = 0; p < parameters.Length; p++) parameters[p] = Declaration();
                    var annotations = Ids();
                    Ids(); // Generic types.
                    result.Functions.Add(new Function(id, offset, length, reader.ReadInt64(),
                        reader.ReadUInt32(), parameters, annotations));
                    break;
                case 3: Declaration(); break;
                case 4:
                    if (reader.ReadByte() != 0) result.References.Add(reader.ReadUInt32());
                    break;
                case 6:
                    Ids(); // Fields.
                    var lifecycles = reader.ReadUInt32();
                    for (uint f = 0; f < lifecycles; f++)
                    {
                        reader.ReadByte();
                        result.References.Add(reader.ReadUInt32());
                    }
                    for (var a = 0; a < 5; a++) Ids();
                    break;
                case 7: case 10: result.References.Add(reader.ReadUInt32()); break;
                case 8: Ids(); Ids(); break;
                case 9: Ids(); break;
                default: Assert.Fail("Unexpected symbol kind"); break;
            }
        }
        var pools = reader.ReadUInt32();
        for (uint p = 0; p < pools; p++)
        {
            var constants = reader.ReadUInt32();
            var values = new List<byte[]>();
            for (uint c = 0; c < constants; c++)
            {
                Assert.That(reader.ReadUInt32(), Is.EqualTo(c));
                result.References.Add(reader.ReadUInt32()); // Type ID.
                Assert.That(reader.ReadBoolean(), Is.False);
                values.Add(reader.ReadBytes(checked((int)reader.ReadUInt64())));
            }
            result.Pools.Add(values);
        }
        result.Code = reader.ReadBytes(checked((int)(reader.BaseStream.Length - reader.BaseStream.Position)));
        Assert.That(result.References.Distinct(), Is.SubsetOf(result.SymbolIds));
        return result;

        uint[] Ids()
        {
            var ids = new uint[checked((int)reader.ReadUInt32())];
            for (var i = 0; i < ids.Length; i++) ids[i] = reader.ReadUInt32();
            result.References.AddRange(ids);
            return ids;
        }

        uint Declaration()
        {
            reader.ReadInt32(); // Dimension.
            reader.ReadByte(); // Mutability.
            var id = reader.ReadUInt32();
            result.References.Add(id);
            reader.ReadBoolean(); // Allow none.
            return id;
        }
    }
}
