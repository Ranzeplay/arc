using Arc.Compiler.Lexer;
using Arc.Compiler.Parser;
using Arc.Compiler.Shared.Compilation;
using Arc.Compiler.Shared.Parsing.Components.Data;
using Arc.Compiler.Shared.Parsing.Components.Function;

namespace Arc.Compiler.Tests.Parsing
{
    [Timeout(1000)]
    [Category("Parsing")]
    public class PipelineTest
    {
        [Test]
        public void BasicPipelineTest()
        {
            var text = """
                link std::io;

                decl group master
                {
                    field var string a(get);
                    method test()[number] { }
                }

                decl func main()[number] { return 0; }
                """;
            var source = new SourceFile("test", text);
            var tokens = Tokenizer.Tokenize(source, true);

            var result = Pipeline.BuildAll(new(tokens.Tokens, Array.Empty<DataDeclarator>(), Array.Empty<FunctionDeclarator>()));

            Assert.That(result, Is.Not.Null);
        }
    }
}
