using Arc.Compiler.Lexer;
using Arc.Compiler.Parser;
using Arc.Compiler.Parser.Builders.Blocks;
using Arc.Compiler.Parser.Models;
using Arc.Compiler.Shared.CommandGeneration;
using Arc.Compiler.Shared.CommandGeneration.Relocation;
using Arc.Compiler.Shared.Compilation;
using Arc.Compiler.Shared.Parsing.AST;
using Arc.Compiler.Shared.Parsing.Components.Data;
using Arc.Compiler.Shared.Parsing.Components.Function;
using Arc.CompilerCommandGenerator.Builders;
using Arc.CompilerCommandGenerator.Managers;
using Arc.CompilerCommandGenerator.Models;

namespace Arc.Compiler.Tests.CommandGeneration
{
    [Timeout(1000)]
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

            Assert.Multiple(() =>
            {
                Assert.That(result.RelocationTargets.Where(x => x.RelocationType != RelocationType.Constant).Count(), Is.EqualTo(6));
                Assert.That(result.RelocationReferences, Has.Count.EqualTo(6));
            });
        }

        [Test]
        [Timeout(-1)]
        public void MixedRelocationTest()
        {
            var text = """
                       decl func main()[number]
                       {
                           call external();
                           return 0;
                       }
                       
                       decl func external()[number]
                       {
                           decl var std::dynamic num1;
                           decl var std::dynamic num2;
                           num1 = 1;
                           num2 = 2;

                           if(num1 <> num2)
                           {
                               num1 = num1 + 500;
                           }
                           else
                           {
                               num2 = 1024;
                           }

                           while(num1 < 200)
                           {
                               num2 = num2 + 300;
                           }

                           return num1;
                       }
                       """;
            var source = new SourceFile("test", text);
            var tokens = Tokenizer.Tokenize(source, true);
            var parsingModel = new ExpressionBuildModel(tokens.Tokens, Array.Empty<DataDeclarator>(), Array.Empty<FunctionDeclarator>());

            var functionDeclarators = Pipeline.BuildAllFunctionDeclarators(parsingModel);
            parsingModel.DeclaredFunctions = functionDeclarators.ToArray();

            var parsingResult = Pipeline.BuildAll(parsingModel);
            if(parsingResult is null)
            {
                Assert.Fail("Failed to parse source code");
                return;
            }

            var metadata = new PackageMetadata(0, 2, 2, 2, 0, 2);
            var result = new PartialGenerationResult();
            foreach(var func in parsingResult.DeclaredFunctions)
            {
                var context = new GenerationContext<FunctionBlock>(func, new(), new(), functionDeclarators.ToList(), new(), new(), metadata);
                var partialResult = FunctionCommandGroup.Build(context);
                result.Combine(partialResult);
            }

            var reloc = new FinalRelocationContext
            {
                Commands = result.Commands.ToArray(),
                PackageMetadata = metadata,
                GeneratedConstants = result.GeneratedConstants.ToArray(),
                GeneratedFunctions = functionDeclarators.ToArray(),
                GlobalData = Array.Empty<DataDeclarator>(),
                RelocationReferences = result.RelocationReferences.ToArray(),
                RelocationTargets = result.RelocationTargets.ToArray(),
            };

            reloc.ConvertRelocationTargets();
            reloc.ApplyAllRelocation();

            Assert.Pass();
        }
    }
}
