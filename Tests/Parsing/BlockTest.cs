using Arc.Compiler.Lexer;
using Arc.Compiler.Parser.Builders.Blocks;
using Arc.Compiler.Shared.Compilation;
using Arc.Compiler.Shared.Parsing.Components.Data;
using Arc.Compiler.Shared.Parsing.Components.Function;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

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
    }
}
