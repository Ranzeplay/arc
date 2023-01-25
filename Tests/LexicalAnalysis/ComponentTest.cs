using Arc.Compiler.Lexer.Rules;
using Arc.Compiler.Shared.Compilation;
using Arc.Compiler.Shared.LexicalAnalysis;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Arc.Compiler.Tests.LexicalAnalysis
{
    [Category("LexicalAnalysis")]
    public class ComponentTest
    {
        [SetUp]
        public void Setup() { }

        [Test]
        public void KeywordTest()
        {
            var text = $"{TokenConstants.KeywordMappings[KeywordToken.Implement]} 37413.cc";
            var source = new SourceFile("test", text);
            var result = Keyword.Build(source, 0);

            Assert.That(result, Is.Not.EqualTo(null));

            if (result.Section is not null)
            {
                Assert.Multiple(() =>
                {
                    Assert.That(result.Section.TokenType, Is.EqualTo(TokenType.Keyword));

                    Assert.That(result.Section.GetKeyword(), Is.EqualTo(KeywordToken.Implement));
                });
            }
        }

        [Test]
        public void ContainerTest()
        {
            var text = $"{TokenConstants.ContainerMappings[ContainerToken.AntiBrace]} 37413.cc";
            var source = new SourceFile("test", text);
            var result = Container.Build(source, 0);

            Assert.That(result, Is.Not.EqualTo(null));

            if (result.Section is not null)
            {
                Assert.Multiple(() =>
                {
                    Assert.That(result.Section.TokenType, Is.EqualTo(TokenType.Container));

                    Assert.That(result.Section.GetContainer(), Is.EqualTo(ContainerToken.AntiBrace));
                });
            }
        }
    }
}
