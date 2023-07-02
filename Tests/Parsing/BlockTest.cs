using Arc.Compiler.Lexer;
using Arc.Compiler.Parser.Builders.Blocks;
using Arc.Compiler.Shared.Compilation;
using Arc.Compiler.Shared.Parsing.Components.Data;
using Arc.Compiler.Shared.Parsing.Components.Function;

namespace Arc.Compiler.Tests.Parsing
{
    [Category("Parsing")]
    internal class BlockTest
    {
        [SetUp]
        public void Setup() { }

        [Test]
        public void DeclarationBlockTest()
        {
            var text = "decl var std::dynamic num1;";
            var source = new SourceFile("test", text);
            var tokens = Tokenizer.Tokenize(source, true);

            var result = DataDeclarationBuilder.Build(tokens.Tokens);

            Assert.That(result, Is.Not.Null);
        }

        [Test]
        public void AssignmentTest()
        {
            var text = "var1 = 3 * 7;";
            var source = new SourceFile("test", text);
            var tokens = Tokenizer.Tokenize(source, true);

            var definedData = new DataDeclarator[]
            {
                new(new(new(Array.Empty<string>(), "type1"), false), new(Array.Empty<string>(), "var1"), false),
            };

            var result = DataAssignmentBuilder.Build(new(tokens.Tokens, definedData, Array.Empty<FunctionDeclarator>()));

            Assert.That(result, Is.Not.Null);
            if (result is not null)
            {
                Assert.That(result, Has.Length.EqualTo(6));
            }
        }

        [Test]
        public void FunctionCallTest()
        {
            var text = "call func1(var2);";
            var source = new SourceFile("test", text);
            var tokens = Tokenizer.Tokenize(source, true);

            var definedData = new DataDeclarator[]
            {
                new(new(new(Array.Empty<string>(), "type1"), false), new(Array.Empty<string>(), "var2"), false)
            };

            var definedFunctions = new FunctionDeclarator[]
            {
                new(
                    new(Array.Empty<string>(), "func1"),
                    new(new(Array.Empty<string>(), "retType1"), false),
                    new FunctionParameter[]
                    {
                        new(new(new(Array.Empty<string>(), "type1"), false), new(Array.Empty<string>(), "param1"), false)
                    })
            };

            var result = FunctionCallBuilder.Build(new(tokens.Tokens, definedData, definedFunctions));

            Assert.That(result, Is.Not.Null);
            if (result is not null)
            {
                Assert.That(result, Has.Length.EqualTo(6));
            }
        }

        [Test]
        public void FunctionReturnTest()
        {
            var text = "return x + y;";
            var source = new SourceFile("test", text);
            var tokens = Tokenizer.Tokenize(source, true);

            var definedData = new DataDeclarator[]
            {
                new(new(new(Array.Empty<string>(), "type1"), false), new(Array.Empty<string>(), "x"), false),
                new(new(new(Array.Empty<string>(), "type1"), false), new(Array.Empty<string>(), "y"), false)
            };

            var result = FunctionReturnBuilder.Build(new(tokens.Tokens, definedData, Array.Empty<FunctionDeclarator>()));

            Assert.That(result, Is.Not.Null);
            if (result is not null)
            {
                Assert.Multiple(() =>
                {
                    Assert.That(result.Section.Expression, Is.Not.Null);
                    Assert.That(result, Has.Length.EqualTo(5));
                });
            }
        }

        [Test]
        public void ConditionalLoopTest()
        {
            var text = "while (3 > 2) { decl var number a; }";
            var source = new SourceFile("test", text);
            var tokens = Tokenizer.Tokenize(source, true);

            var result = ConditionalLoopBlockBuilder.Build(new(tokens.Tokens, Array.Empty<DataDeclarator>(), Array.Empty<FunctionDeclarator>()));

            Assert.That(result, Is.Not.Null);
            if (result is not null)
            {
                Assert.That(result.Section.Actions.ASTNodes, Has.Length.EqualTo(1));
            }
        }

        [Test]
        public void ConditionalExecTest()
        {
            var text = "if (3 > 2) { decl var number a; } elif (1 < 4) { decl var string b; } else { }";
            var source = new SourceFile("test", text);
            var tokens = Tokenizer.Tokenize(source, true);

            var result = ConditionalExecBlockBuilder.Build(new(tokens.Tokens, Array.Empty<DataDeclarator>(), Array.Empty<FunctionDeclarator>()));
            Assert.That(result, Is.Not.Null);
        }

        [Test]
        public void FunctionBlockTest()
        {
            var text = "decl func alpha(var number a)[number] { decl var string b; }";
            var source = new SourceFile("test", text);
            var tokens = Tokenizer.Tokenize(source, true);

            var result = FunctionBlockBuilder.Build(new(tokens.Tokens, Array.Empty<DataDeclarator>(), Array.Empty<FunctionDeclarator>()));

            Assert.That(result, Is.Not.Null);
            if (result is not null)
            {
                Assert.Multiple(() =>
                {
                    Assert.That(result, Has.Length.EqualTo(18));
                    Assert.That(result.Section.Actions.ASTNodes, Has.Length.EqualTo(1));
                    Assert.That(result.Section.Declarator.Parameters, Has.Length.EqualTo(1));
                });
            }
        }

        [TestCase("link std;")]
        [TestCase("link \"path.als\";")]
        public void LinkBlockTest(string text)
        {
            var source = new SourceFile("test", text);
            var tokens = Tokenizer.Tokenize(source, true);

            var result = LinkBlockBuilder.Build(tokens.Tokens);
            Assert.That(result, Is.Not.Null);
            if(result != null)
            {
                Assert.That(result, Has.Length.EqualTo(3));
            }
        }
    }
}
