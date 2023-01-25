using Arc.Compiler.Lexer;
using Arc.Compiler.Lexer.Rules;
using Arc.Compiler.Shared.Compilation;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

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
