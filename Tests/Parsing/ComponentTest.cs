using Arc.Compiler.Lexer;
using Arc.Compiler.Parser.Builders.Blocks;
using Arc.Compiler.Parser.Builders.Components;
using Arc.Compiler.Parser.Builders.Components.Expression;
using Arc.Compiler.Shared.Compilation;
using Arc.Compiler.Shared.Parsing.Components.Data;
using Arc.Compiler.Shared.Parsing.Components.Expression;
using Arc.Compiler.Shared.Parsing.Components.Function;

namespace Arc.Compiler.Tests.Parsing
{
    [Category("Parsing")]
    public class ComponentTest
    {
        [SetUp]
        public void Setup() { }

        [Test]
        public void IdentifierTest()
        {
            var text = "arc::compiler::identifier_test 37413";
            var source = new SourceFile("test", text);
            var tokens = Tokenizer.Tokenize(source, true);

            var result = IdentifierBuilder.Build(tokens.Tokens);

            Assert.That(result, Is.Not.EqualTo(null));
            if (result is not null)
            {
                Assert.That(result, Has.Length.EqualTo(5));
                Assert.That(result.Section.Name, Is.EqualTo("identifier_test"));
            }
        }

        [Test]
        public void SimpleExpressionTest()
        {
            var text = "2 + (var1 * func1(var2))";
            var source = new SourceFile("test", text);
            var tokens = Tokenizer.Tokenize(source, true);

            var definedData = new DataDeclarator[]
            {
                new(new(new(Array.Empty<string>(), "type1"), false), new(Array.Empty<string>(), "var1"), false),
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

            var result = ExpressionBuilder.Build(new(tokens.Tokens, definedData, definedFunctions));

            Assert.That(result, Is.Not.EqualTo(null));
            if (result is not null)
            {
                Assert.That(result.Section.Terms, Has.Length.EqualTo(7));
                Assert.That(result.Section.Terms[5].TermType, Is.EqualTo(ExpressionTermType.Data));
                var data = result.Section.Terms[5].GetDataTerm();
                if (data is not null)
                {
                    Assert.That(data.DataTermType, Is.EqualTo(ExpressionDataTermType.FunctionCall));
                }
            }
        }

        [Test]
        public void LoopBreakTest()
        {
            var text = "break;";
            var source = new SourceFile("test", text);
            var tokens = Tokenizer.Tokenize(source, true);

            var result = LoopBreakActionBuilder.Build(tokens.Tokens);
            Assert.That(result, Is.Not.EqualTo(null));
        }

        [Test]
        public void LoopContinueTest()
        {
            var text = "continue;";
            var source = new SourceFile("test", text);
            var tokens = Tokenizer.Tokenize(source, true);

            var result = LoopContinueActionBuilder.Build(tokens.Tokens);
            Assert.That(result, Is.Not.EqualTo(null));
        }
    }
}
