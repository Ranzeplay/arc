using Arc.Compiler.Lexer;
using Arc.Compiler.Parser.Builders.Blocks;
using Arc.Compiler.Shared.Compilation;
using Arc.Compiler.Shared.Parsing.Components.Data;
using Arc.CompilerCommandGenerator.Builders;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Arc.Compiler.Tests.CommandGeneration
{
    [Category("CommandGeneration")]
    internal class SingleActionTest
    {
        [Test]
        public void DeclarationActionTest()
        {
            var text = "decl var std::dynamic num1;";
            var source = new SourceFile("test", text);
            var tokens = Tokenizer.Tokenize(source, true);
            var action = DataDeclarationBuilder.Build(tokens.Tokens);

            var result = DeclarationCommand.Build(new(action!.Section, Enumerable.Empty<DataDeclarator>(), Enumerable.Empty<DataDeclarator>(), null!));

            Assert.That(result, Is.Not.Null);
        }
    }
}
