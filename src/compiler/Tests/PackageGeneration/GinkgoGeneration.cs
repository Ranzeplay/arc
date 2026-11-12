using System.Buffers.Binary;
using Arc.Compiler.PackageGenerator.Ginkgo.Scope;
using Arc.Compiler.PackageGenerator.Ginkgo.Transformation;
using Arc.Compiler.PackageGenerator.Models.Descriptors;
using Arc.Compiler.SyntaxAnalyzer;
using Arc.Compiler.SyntaxAnalyzer.Models;
using Microsoft.Extensions.Logging;

namespace Arc.Compiler.Tests.PackageGeneration;

public class GinkgoGeneration
{
    private static (IReadOnlyList<ArcGinkgoUnitGenerationContext> Units,
        IReadOnlyList<Arc.Compiler.PackageGenerator.Models.Logging.ArcCompilationLogBase> Logs) Generate(string body)
        => GenerateSource($"namespace Test {{ public func main(): none {{ {body} }} }}");

    private static (IReadOnlyList<ArcGinkgoUnitGenerationContext> Units,
        IReadOnlyList<Arc.Compiler.PackageGenerator.Models.Logging.ArcCompilationLogBase> Logs) GenerateSource(string text)
    {
        using var factory = LoggerFactory.Create(_ => { });
        var logger = factory.CreateLogger<GinkgoGeneration>();
        var unit = new ArcCompilationUnit(AntlrAdapter.ParseCompilationUnit(text, logger), logger, "ginkgo-test");
        return ArcGinkgoUnitGenerator.Generate([unit], ArcPackageDescriptor.Default(ArcPackageType.Library));
    }

    [TestCase("int", "return 42;", (byte)1)]
    [TestCase("none", "return;", (byte)0)]
    [TestCase("none", "", (byte)0)]
    public void ReturnsSetExpectReturnValueFromTheDeclaredType(string returnType, string body, byte expectedFlag)
    {
        var (units, logs) = GenerateSource(
            $"namespace Test {{ public func main(): {returnType} {{ {body} }} }}");
        Assert.That(logs.Any(l => l.Level is LogLevel.Error or LogLevel.Critical), Is.False);
        var function = units.Single().ScopeTree.GetNodes<ArcScopeTreeFunctionNodeBase>().Single();
        Assert.That(function.GenerationResult.GeneratedData.Last(), Is.EqualTo(expectedFlag));
    }

    [Test]
    public void NestedControlFlowResolvesBranchesAndKeepsRegistersUnique()
    {
        var (units, logs) = Generate("""
            var counter: int = 0;
            while (counter < 3) {
                if (counter == 1) { counter = counter + 1; continue; }
                counter = counter + 1;
                if (counter == 3) { break; }
            }
            """);
        Assert.That(logs.Any(l => l.Level is LogLevel.Error or LogLevel.Critical), Is.False);
        var function = units.Single().ScopeTree.GetNodes<ArcScopeTreeFunctionNodeBase>().Single();
        Assert.Multiple(() =>
        {
            Assert.That(function.GenerationResult.GeneratedData, Is.Not.Empty);
            Assert.That(function.GenerationResult.RelocationTargets, Is.Empty);
            Assert.That(function.DataCount, Is.GreaterThan(1));
        });
    }

    [Test]
    public void ABlockLocalIsNotVisibleAfterItsBlock()
    {
        var (_, logs) = Generate("if (true) { var hidden: int = 1; } hidden = 2;");
        Assert.That(logs.Any(l => l.Level == LogLevel.Error), Is.True);
    }

    [Test]
    public void InvalidLoopControlProducesADiagnostic()
    {
        var (_, logs) = Generate("break;");
        Assert.That(logs.Any(l => l.Level == LogLevel.Error), Is.True);
    }

    [Test]
    public void CallInitializerUsesTheDeclaredOutputRegisterDirectly()
    {
        var (units, logs) = GenerateSource("""
            namespace Test {
                public func answer(): int { return 42; }
                public func main(): none { var value: int = Test::answer(); }
            }
            """);
        Assert.That(logs.Any(l => l.Level >= LogLevel.Error), Is.False);
        var function = units.Single().ScopeTree.GetNodes<ArcScopeTreeFunctionNodeBase>().Single(f => f.Name == "main");
        var bytes = function.GenerationResult.GeneratedData.ToArray();
        Assert.That(bytes.Length, Is.EqualTo(11)); // pcall, invoke, retn; no copy
        Assert.That(bytes[5], Is.EqualTo(0x19));
        Assert.That(BinaryPrimitives.ReadUInt16LittleEndian(bytes.AsSpan(6)), Is.EqualTo(1));
    }

    [Test]
    public void CallAssignmentWritesToTheExistingLocalRegister()
    {
        var (units, logs) = GenerateSource("""
            namespace Test {
                public func answer(): int { return 42; }
                public func main(): none { var value: int = 0; value = Test::answer(); }
            }
            """);
        Assert.That(logs.Any(l => l.Level >= LogLevel.Error), Is.False);
        var function = units.Single().ScopeTree.GetNodes<ArcScopeTreeFunctionNodeBase>().Single(f => f.Name == "main");
        var bytes = function.GenerationResult.GeneratedData.ToArray();
        Assert.That(bytes.Length, Is.EqualTo(25)); // ld, copy, pcall, invoke, retn
        Assert.That(bytes[19], Is.EqualTo(0x19));
        Assert.That(BinaryPrimitives.ReadUInt16LittleEndian(bytes.AsSpan(20)),
            Is.EqualTo(BinaryPrimitives.ReadUInt16LittleEndian(bytes.AsSpan(11))));
    }

    [Test]
    public void ReturnedCallReadsTheInvokeDestination()
    {
        var (units, logs) = GenerateSource("""
            namespace Test {
                public func answer(): int { return 42; }
                public func main(): int { return Test::answer(); }
            }
            """);
        Assert.That(logs.Any(l => l.Level >= LogLevel.Error), Is.False);
        var function = units.Single().ScopeTree.GetNodes<ArcScopeTreeFunctionNodeBase>().Single(f => f.Name == "main");
        var bytes = function.GenerationResult.GeneratedData.ToArray();
        Assert.That(bytes.Length, Is.EqualTo(13)); // pcall, invoke, retv
        Assert.That(bytes[5], Is.EqualTo(0x19));
        Assert.That(bytes[9], Is.EqualTo(0x1a));
        Assert.That(BinaryPrimitives.ReadUInt16LittleEndian(bytes.AsSpan(10)),
            Is.EqualTo(BinaryPrimitives.ReadUInt16LittleEndian(bytes.AsSpan(6))));
    }

    [Test]
    public void NestedValueCallsCanInitializeLocalsAndFeedOtherCalls()
    {
        var (_, logs) = GenerateSource("""
            namespace Test {
                public func identity(const value: int): int { return value; }
                public func main(): none {
                    var result: int = Test::identity(Test::identity(42)) + 1;
                    call Test::identity(result);
                }
            }
            """);
        Assert.That(logs.Any(l => l.Level is LogLevel.Error or LogLevel.Critical), Is.False);
    }

    [Test]
    public void VoidCallsCannotInitializeValueLocals()
    {
        var (_, logs) = GenerateSource("""
            namespace Test {
                public func sink(): none {}
                public func main(): none { var value: int = Test::sink(); }
            }
            """);
        Assert.That(logs.Any(l => l.Level == LogLevel.Error), Is.True);
    }

    [Test]
    public void QualifiedCallsResolveAllOverloads()
    {
        var (_, logs) = GenerateSource("""
            namespace Test {
                public func sink(const value: int): none {}
                public func sink(const value: bool): none {}
                public func main(): none { call Test::sink(true); }
            }
            """);
        Assert.That(logs.Any(l => l.Level is LogLevel.Error or LogLevel.Critical), Is.False);
    }

    [Test]
    public void ConstructorAndFieldAssignmentPreserveDeclaredMutability()
    {
        var (units, logs) = GenerateSource("""
            namespace Test {
                public group Item {
                    public field var count: int;
                    public constructor(var self: Item, const initial: int): none {
                        self.count = initial;
                    }
                }
                public func main(): none {
                    var item: Item = new Item(1);
                    item.count = 2;
                }
            }
            """);
        Assert.That(logs.Any(l => l.Level is LogLevel.Error or LogLevel.Critical), Is.False);
        Assert.That(units.Single().ScopeTree.GetNodes<ArcScopeTreeFunctionNodeBase>()
            .All(f => f.GenerationResult.GeneratedData.Count > 0), Is.True);
    }

    [Test]
    public void ConstantFieldsCannotBeAssigned()
    {
        var (_, logs) = GenerateSource("""
            namespace Test {
                public group Item { public field const count: int; }
                public func main(): none {
                    var item: Item = new Item();
                    item.count = 2;
                }
            }
            """);
        Assert.That(logs.Any(l => l.Level == LogLevel.Error && l.FormattedMessage.Contains("constant field")), Is.True);
    }

    [TestCase(false)]
    [TestCase(true)]
    public void ConstructionStoresTheAllocatedObjectRatherThanTheVoidCallResult(bool explicitConstructor)
    {
        var constructor = explicitConstructor ? "public constructor(var self: Item): none {}" : "";
        var (units, logs) = GenerateSource($$"""
            namespace Test {
                public group Item { {{constructor}} }
                public func main(): none { var item: Item = new Item(); }
            }
            """);
        Assert.That(logs.Any(l => l.Level >= LogLevel.Error), Is.False);
        var function = units.Single().ScopeTree.GetNodes<ArcScopeTreeFunctionNodeBase>().Single(f => f.Name == "main");
        var bytes = function.GenerationResult.GeneratedData.ToArray();
        var copyOffset = explicitConstructor ? 23 : 10;
        Assert.That(bytes.Length, Is.EqualTo(copyOffset + 8)); // copy and retn
        Assert.That(bytes[copyOffset], Is.EqualTo(0x1f));
        Assert.That(BinaryPrimitives.ReadUInt16LittleEndian(bytes.AsSpan(copyOffset + 1)), Is.Zero);
        Assert.That(BinaryPrimitives.ReadUInt16LittleEndian(bytes.AsSpan(copyOffset + 3)), Is.EqualTo(1));
        if (explicitConstructor)
        {
            Assert.That(bytes[19], Is.EqualTo(0x19));
            Assert.That(BinaryPrimitives.ReadUInt16LittleEndian(bytes.AsSpan(20)), Is.Zero);
        }
    }

    [TestCase(false)]
    [TestCase(true)]
    public void AssignIfNoneSkipsTheRhsCallAndStoreForAnExistingValue(bool field)
    {
        var declaration = field ? "var item: Item = new Item();" : "var value: int = 1;";
        var target = field ? "item.value" : "value";
        var (units, logs) = GenerateSource($$"""
            namespace Test {
                public group Item { public field var value: int; }
                public func answer(): int { return 42; }
                public func main(): none { {{declaration}} {{target}} ??= Test::answer(); }
            }
            """);
        Assert.That(logs.Any(l => l.Level >= LogLevel.Error), Is.False);
        var function = units.Single().ScopeTree.GetNodes<ArcScopeTreeFunctionNodeBase>().Single(f => f.Name == "main");
        var bytes = function.GenerationResult.GeneratedData.ToArray();
        var guardOffset = field ? 26 : 14;
        var endOffset = field ? 57 : 35;
        Assert.That(bytes[guardOffset], Is.EqualTo(0x16));
        Assert.That(bytes[guardOffset + 11], Is.EqualTo(4)); // condition_not_none
        Assert.That(BinaryPrimitives.ReadInt64LittleEndian(bytes.AsSpan(guardOffset + 3)), Is.EqualTo(endOffset));
        Assert.That(bytes[guardOffset + 12], Is.EqualTo(0x17)); // RHS pcall follows the guard
        Assert.That(bytes[endOffset], Is.EqualTo(0x14));
        Assert.That(bytes[endOffset + 1], Is.EqualTo(0x1b));
        if (field)
        {
            Assert.That(bytes[16], Is.EqualTo(0x27)); // load the current field value once
            Assert.That(BinaryPrimitives.ReadUInt16LittleEndian(bytes.AsSpan(guardOffset + 1)),
                Is.EqualTo(BinaryPrimitives.ReadUInt16LittleEndian(bytes.AsSpan(19))));
            Assert.That(bytes[endOffset - 10], Is.EqualTo(0x28)); // field store is inside the guard
        }
    }

    [TestCase("var item: Item = new Item<int>();", "constructor specialization")]
    [TestCase("var item: Item = new Item<?>();", "constructor specialization")]
    [TestCase("var value: int = Test::answer<int>();", "function specialization")]
    [TestCase("var value: int = Test::answer<?>();", "function specialization")]
    public void UnsupportedExplicitSpecializationProducesADiagnostic(string body, string diagnostic)
    {
        var (_, logs) = GenerateSource($$"""
            namespace Test {
                public group Item {}
                public func answer(): int { return 42; }
                public func main(): none { {{body}} }
            }
            """);
        Assert.That(logs.Any(l => l.Level == LogLevel.Error && l.FormattedMessage.Contains(diagnostic)), Is.True);
    }

    [Test]
    public void DeferredLibraryGenerationSkipsUnrequestedBodies()
    {
        using var factory = LoggerFactory.Create(_ => { });
        var logger = factory.CreateLogger<GinkgoGeneration>();
        var user = new ArcCompilationUnit(AntlrAdapter.ParseCompilationUnit(
            "namespace Test { public func main(): none {} }", logger), logger, "user");
        var library = new ArcCompilationUnit(AntlrAdapter.ParseCompilationUnit($$"""
            namespace Test {
                public func entry(): none {}
                public func leaf(): none {}
                public func unused(): none {}
            }
            """, logger), logger, "library");
        var (units, logs) = ArcGinkgoUnitGenerator.Generate([user, library],
            ArcPackageDescriptor.Default(ArcPackageType.Library), deferredUnits: [library]);
        Assert.That(logs.Any(l => l.Level >= LogLevel.Error), Is.False);
        var functions = units[1].ScopeTree.GetNodes<ArcScopeTreeFunctionNodeBase>().ToList();
        Assert.That(functions.Single(f => f.Name == "entry").GenerationResult, Is.Null);
        Assert.That(functions.Single(f => f.Name == "leaf").GenerationResult, Is.Null);
        Assert.That(functions.Single(f => f.Name == "unused").GenerationResult, Is.Null);
    }

    [Test]
    public void VariableParametersRemainAssignable()
    {
        var (_, logs) = GenerateSource("""
            namespace Test {
                public func update(var value: int): none { value = value + 1; }
            }
            """);
        Assert.That(logs.Any(l => l.Level is LogLevel.Error or LogLevel.Critical), Is.False);
    }

    [Test]
    public void NewlyConnectedExpressionOperatorsLowerToInstructions()
    {
        var (_, logs) = Generate("""
            var remainder: int = 7 % 3;
            var shifted: int = remainder << 1;
            var right: int = shifted >> 1;
            var inverted: int = right~;
            var unequal: bool = right <!> inverted;
            var negated: bool = !unequal;
            """);
        Assert.That(logs.Any(l => l.Level is LogLevel.Error or LogLevel.Critical), Is.False);
    }
}
