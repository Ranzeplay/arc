using Arc.Compiler.Lexer;
using Arc.Compiler.Parser.Builders.Blocks;
using Arc.Compiler.Shared.Compilation;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Arc.Compiler.Tests.Parsing
{
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

            var result = DataDeclaratorBuilder.Build(tokens.Tokens);

            Assert.That(result, Is.Not.Null);
        }
    }
}
