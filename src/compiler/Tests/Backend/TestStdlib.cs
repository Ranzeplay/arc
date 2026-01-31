using System.Text;
using Arc.Compiler.PackageGenerator;
using Arc.Compiler.PackageGenerator.Models.Builtin;
using Arc.Compiler.PackageGenerator.Models.Descriptors;
using Arc.Compiler.SyntaxAnalyzer;
using Arc.Compiler.SyntaxAnalyzer.Models;
using Arc.Compiler.SyntaxAnalyzer.Models.Identifier;
using Microsoft.Extensions.Logging;

namespace Arc.Compiler.Tests.Backend;

[TestFixture]
[CancelAfter(1000)]
[Category("Backend")]
public class TestStdlib
{
    private readonly ILogger _logger = LoggerFactory.Create(_ => { }).CreateLogger<TestStdlib>();

    private readonly Dictionary<ArcStdlibScope, (string, Lazy<string>)> _namespaceSourceMapping = new()
    {
        {
            ArcStdlibScope.Compilation, ("Arc::Std::Compilation",
                new Lazy<string>(() => Encoding.UTF8.GetString(ArcStdlibSource.NamespaceCompilation)))
        },
        {
            ArcStdlibScope.Array,
            ("Arc::Std::Array", new Lazy<string>(() => Encoding.UTF8.GetString(ArcStdlibSource.NamespaceArray)))
        },
        {
            ArcStdlibScope.Console,
            ("Arc::Std::Console", new Lazy<string>(() => Encoding.UTF8.GetString(ArcStdlibSource.NamespaceConsole)))
        },
        {
            ArcStdlibScope.Math,
            ("Arc::Std::Math", new Lazy<string>(() => Encoding.UTF8.GetString(ArcStdlibSource.NamespaceMath)))
        },
        {
            ArcStdlibScope.CollectionList, ("Arc::Std::Collection",
                new Lazy<string>(() => Encoding.UTF8.GetString(ArcStdlibSource.NamespaceCollectionList)))
        },
        {
            ArcStdlibScope.CollectionLinkedList, ("Arc::Std::Collection",
                new Lazy<string>(() => Encoding.UTF8.GetString(ArcStdlibSource.NamespaceCollectionLinkedList)))
        },
    };

    private void TestNamespaceHelper(string ns, byte[] source, IEnumerable<ArcStdlibScope>? extraScopes = null)
    {
        using (Assert.EnterMultipleScope())
        {
            Assert.That(ns, Is.Not.Null, "Namespace must not be null, check configuration");
            Assert.That(source, Is.Not.Null, "Namespace source must not be null, check configuration");
        }

        var nss = Encoding.UTF8.GetString(source);
        Assert.That(nss, Is.Not.Null);
        var parsingCtx = AntlrAdapter.ParseCompilationUnit(nss, _logger);

        Assert.That(parsingCtx, Is.Not.Null);
        Assert.That(parsingCtx.exception, Is.Null);

        var nsIdent = new ArcNamespaceIdentifier(parsingCtx.arc_namespace_block().arc_namespace_declarator()
            .arc_namespace_identifier());
        Assert.That(string.Join("::", nsIdent.Namespace), Is.EqualTo(ns),
            "Namespace must be equal to namespace in the source code, check configuration");

        var nsu = new ArcCompilationUnit(parsingCtx, _logger, ns);
        IEnumerable<ArcCompilationUnit> genUnits = [nsu, ..extraScopes?.Select(ParseStdScope) ?? []];
        var genCtx = ArcCombinedUnitGenerator.GenerateUnits(genUnits,
            ArcPackageDescriptor.Default(ArcPackageType.Library), false, _logger);

        Assert.That(genCtx, Is.Not.Null);
        Assert.That(genCtx.Logs.Where(l => l.Level > LogLevel.Information), Is.Empty);

        Assert.That(genCtx.GlobalScopeTree, Is.Not.Null);
        Assert.That(genCtx.GlobalScopeTree.GetNamespace(ns.Split("::")), Is.Not.Null);
    }

    private ArcCompilationUnit ParseStdScope(ArcStdlibScope scope)
    {
        var nss = _namespaceSourceMapping[scope].Item2.Value;
        var parsingCtx = AntlrAdapter.ParseCompilationUnit(nss, _logger);
        var nsu = new ArcCompilationUnit(parsingCtx, _logger, _namespaceSourceMapping[scope].Item1);

        return nsu;
    }

    [Test]
    public void TestEmpty()
    {
        var genCtx = ArcCombinedUnitGenerator.GenerateUnits([], ArcPackageDescriptor.Default(ArcPackageType.Library),
            false, _logger);

        Assert.That(genCtx, Is.Not.Null);
        Assert.That(genCtx.Logs.Where(l => l.Level > LogLevel.Information), Is.Empty);

        Assert.That(genCtx.GlobalScopeTree, Is.Not.Null);
        Assert.That(genCtx.GlobalScopeTree.GetNamespace("Arc::Base".Split("::")), Is.Not.Null);
    }

    [Test]
    [Order(0)]
    public void TestCompilation()
    {
        TestNamespaceHelper("Arc::Std::Compilation", ArcStdlibSource.NamespaceCompilation);
    }

    [Test]
    [Order(1)]
    public void TestArray()
    {
        TestNamespaceHelper("Arc::Std::Array", ArcStdlibSource.NamespaceArray, [ArcStdlibScope.Compilation]);
    }

    [Test]
    public void TestConsole()
    {
        TestNamespaceHelper("Arc::Std::Console", ArcStdlibSource.NamespaceConsole, [ArcStdlibScope.Compilation]);
    }

    [Test]
    public void TestMath()
    {
        TestNamespaceHelper("Arc::Std::Math", ArcStdlibSource.NamespaceMath, [ArcStdlibScope.Compilation]);
    }

    [Test]
    public void TestCollectionList()
    {
        TestNamespaceHelper("Arc::Std::Collection", ArcStdlibSource.NamespaceCollectionList, [ArcStdlibScope.Compilation, ArcStdlibScope.Array]);
    }

    [Test]
    public void TestCollectionLinkedList()
    {
        TestNamespaceHelper("Arc::Std::Collection", ArcStdlibSource.NamespaceCollectionLinkedList, [ArcStdlibScope.Compilation, ArcStdlibScope.Array]);
    }
}