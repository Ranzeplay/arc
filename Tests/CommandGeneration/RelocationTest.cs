using Arc.Compiler.Lexer;
using Arc.Compiler.Parser.Builders.Blocks;
using Arc.Compiler.Shared.CommandGeneration;
using Arc.Compiler.Shared.CommandGeneration.Relocation;
using Arc.Compiler.Shared.Compilation;
using Arc.Compiler.Shared.Parsing.Components.Data;
using Arc.Compiler.Shared.Parsing.Components.Function;
using Arc.CompilerCommandGenerator.Builders;
using Arc.CompilerCommandGenerator.Managers;

namespace Arc.Compiler.Tests.CommandGeneration
{
    [Category("PackageGeneration")]
    internal class RelocationTest
    {
        [Test]
        public void LoopBlockRelocationTest()
        {
            var text = "loop { decl var std::dynamic num1; }";
            var source = new SourceFile("test", text);
            var tokens = Tokenizer.Tokenize(source, true);

            var block = LoopBlockBuilder.Build(new(tokens.Tokens, Array.Empty<DataDeclarator>(), Array.Empty<FunctionDeclarator>()))!;

            var metadata = new PackageMetadata(0, 2, 2, 2, 0, 2);

            var result = LoopCommand.Build(new(block!.Section, new(), new(), new(), new(), new(), metadata));
            if(result == null)
            {
                Assert.Fail();
                return;
            }

            RelocationManager.ApplyRelocation(ref result, metadata);

            Console.WriteLine(result.Commands);
        }

        [Test]
        public void ConditionalLoopRelocationTest()
        {
            var text = "while (3 > 2) { decl var std::dynamic num1; }";
            var source = new SourceFile("test", text);
            var tokens = Tokenizer.Tokenize(source, true);

            var block = ConditionalLoopBlockBuilder.Build(new(tokens.Tokens, Array.Empty<DataDeclarator>(), Array.Empty<FunctionDeclarator>()))!;

            var metadata = new PackageMetadata(0, 2, 2, 2, 0, 2);

            var result = ConditionalLoopCommand.Build(new(block!.Section, new(), new(), new(), new(), new(), metadata));
            if (result == null)
            {
                Assert.Fail();
                return;
            }

            RelocationManager.ApplyRelocation(ref result, metadata);
        }

        [Test]
        public void ConditionalExecRelocationTest()
        {
            var text = "if (3 > 2) { decl var std::dynamic num1; } elif (5 > 3) { decl var std::dynamic num2; } else { decl var std::dynamic num3; }";
            var source = new SourceFile("test", text);
            var tokens = Tokenizer.Tokenize(source, true);

            var block = ConditionalExecBlockBuilder.Build(new(tokens.Tokens, Array.Empty<DataDeclarator>(), Array.Empty<FunctionDeclarator>()))!;

            var metadata = new PackageMetadata(0, 2, 2, 2, 0, 2);

            var result = ConditionalExecCommand.Build(new(block!.Section, new(), new(), new(), new(), new(), metadata));
            if (result == null)
            {
                Assert.Fail();
                return;
            }

            Assert.That(result.RelocationTargets.Where(x => x.RelocationType != RelocationType.Constant).Count(), Is.EqualTo(6));
            Assert.That(result.RelocationReferences, Has.Count.EqualTo(6));
        }
    }
}
