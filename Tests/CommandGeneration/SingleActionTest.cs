using Arc.Compiler.CommandGenerator.Builders;
using Arc.Compiler.Lexer;
using Arc.Compiler.Parser.Builders.Blocks;
using Arc.Compiler.Shared.CommandGeneration;
using Arc.Compiler.Shared.Compilation;
using Arc.Compiler.Shared.Parsing.Components.Data;
using Arc.Compiler.Shared.Parsing.Components.Function;

namespace Arc.Compiler.Tests.CommandGeneration
{
    [CancelAfter(1000)]
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

            var result = DeclarationCommand.Build(new(action!.Section, new(), new(), new(), new(), new(), null!));

            Assert.That(result, Is.Not.Null);
        }

        [Test]
        public void AssignmentActionTest()
        {
            var text = "a[3] = 2 + 3;";
            var source = new SourceFile("test", text);
            var tokens = Tokenizer.Tokenize(source, true);

            var definedData = new List<DataDeclarator>
            {
                new(new(new(Array.Empty<string>(), "number"), true), new(Array.Empty<string>(), "a"), false)
            };

            var action = DataAssignmentBuilder.Build(new(tokens.Tokens, definedData.ToArray(), Array.Empty<FunctionDeclarator>()));
            var metadata = new PackageMetadata(0, 2, 2, 2, 0, 2);

            var result = AssignmentCommand.Build(new(action!.Section, new(), new(), new(), new(), new(), metadata));

            Assert.That(result, Is.Not.Null);
            if(result is not null)
            {
                Assert.That(result.GeneratedConstants, Has.Count.EqualTo(3));
            }
        }

        [Test]
        public void LoopActionTest()
        {
            var text = "loop { decl var std::dynamic num1; }";
            var source = new SourceFile("test", text);
            var tokens = Tokenizer.Tokenize(source, true);

            var block = LoopBlockBuilder.Build(new(tokens.Tokens, Array.Empty<DataDeclarator>(), Array.Empty<FunctionDeclarator>()))!;

            var metadata = new PackageMetadata(0, 2, 2, 2, 0, 2);

            var result = LoopCommand.Build(new(block!.Section, new(), new(), new(), new(), new(), metadata));

            Assert.That(result, Is.Not.Null);
        }
    }
}
