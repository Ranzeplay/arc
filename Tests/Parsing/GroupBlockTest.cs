using Arc.Compiler.Lexer;
using Arc.Compiler.Parser.Builders.Group;
using Arc.Compiler.Shared.Compilation;
using Arc.Compiler.Shared.Parsing.Components.Data;
using Arc.Compiler.Shared.Parsing.Components.Function;

namespace Arc.Compiler.Tests.Parsing
{
    [Category("Parsing")]
    public class GroupTest
    {
        [Test]
        public void GroupFieldTest()
        {
            var text = "field var number[] abc (get { decl var number value; }, set);";
            var source = new SourceFile("test", text);
            var tokens = Tokenizer.Tokenize(source, true);

            var result = GroupDeclarationBuilder.BuildGroupField(new(tokens.Tokens, Array.Empty<DataDeclarator>(), Array.Empty<FunctionDeclarator>()));

            Assert.That(result, Is.Not.EqualTo(null));
        }

        [Test]
        public void GroupBlockTest()
        {
            var text = """
                decl group arc {
                    field const string str(get, set);
                    method setup()[arc] { }
                    func from()[arc] { }
                }
                """;
            var source = new SourceFile("test", text);
            var tokens = Tokenizer.Tokenize(source, true);

            var result = GroupDeclarationBuilder.BuildGroupBlock(new(tokens.Tokens, Array.Empty<DataDeclarator>(), Array.Empty<FunctionDeclarator>()));

            Assert.That(result, Is.Not.EqualTo(null));
        }
    }
}
