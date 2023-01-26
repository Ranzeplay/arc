using Arc.Compiler.Lexer;
using Arc.Compiler.Parser.Builders.Components;
using Arc.Compiler.Shared.Compilation;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

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
            if (result is not null && result.Section is not null)
            {
                Assert.That(result, Has.Length.EqualTo(5));
                Assert.That(result.Section.Name, Is.EqualTo("identifier_test"));
            }
        }
    }
}
