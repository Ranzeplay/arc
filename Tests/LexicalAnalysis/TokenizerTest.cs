using Arc.Compiler.Lexer;
using Arc.Compiler.Shared.Compilation;

namespace Arc.Compiler.Tests.LexicalAnalysis
{
    [Category("LexicalAnalysis")]
    public class TokenizerTest
    {
        [SetUp]
        public void Setup(){}

        [Test, MaxTime(2000)]
        public void SimpleTest()
        {
            var text = "export func main() { std::io::println(\"123456\"); return 0; }";
            var source = new SourceFile("test", text);
            var result = Tokenizer.Tokenize(source, true);

            Assert.That(result.Tokens, Has.Length.EqualTo(19));
        }
    }
}
