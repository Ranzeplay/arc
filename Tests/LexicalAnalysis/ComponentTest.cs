using Arc.Compiler.Lexer.Rules;
using Arc.Compiler.Shared.Compilation;
using Arc.Compiler.Shared.LexicalAnalysis;

namespace Arc.Compiler.Tests.LexicalAnalysis
{
    [Timeout(1000)]
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

            Assert.Multiple(() =>
            {
                if (result is not null)
                {
                    Assert.That(result, Is.Not.EqualTo(null));

                    if (result.Section is not null)
                    {
                        Assert.That(result.Section.TokenType, Is.EqualTo(TokenType.Keyword));
                        Assert.That(result.Section.GetKeyword(), Is.EqualTo(KeywordToken.Implement));
                    }
                    else
                    {
                        Assert.Fail();
                    }
                }
                else
                {
                    Assert.Fail();
                }
            });
        }

        [Test]
        public void ContainerTest()
        {
            var text = $"{TokenConstants.ContainerMappings[ContainerToken.AntiBrace]} 37413.cc";
            var source = new SourceFile("test", text);
            var result = Container.Build(source, 0);


            Assert.Multiple(() =>
            {
                if (result is not null)
                {
                    Assert.That(result, Is.Not.EqualTo(null));
                    if (result.Section is not null)
                    {
                        Assert.That(result.Section.TokenType, Is.EqualTo(TokenType.Container));
                        Assert.That(result.Section.GetContainer(), Is.EqualTo(ContainerToken.AntiBrace));
                    }
                    else
                    {
                        Assert.Fail();
                    }
                }
                else
                {
                    Assert.Fail();
                }
            });
        }

        [Test]
        public void RootOperatorTest()
        {
            var text = $"{TokenConstants.RootOperatorMappings[OperatorTokenType.Scope]} 37413.cc";
            var source = new SourceFile("test", text);
            var result = Operator.Build(source, 0);

            Assert.Multiple(() =>
            {
                if (result is not null)
                {
                    Assert.That(result, Is.Not.EqualTo(null));
                    if (result.Section is not null)
                    {
                        Assert.That(result.Section.TokenType, Is.EqualTo(TokenType.Operator));

                        var operatorToken = result.Section.GetOperator();
                        if (operatorToken is not null)
                        {
                            Assert.That(operatorToken.Type, Is.EqualTo(OperatorTokenType.Scope));
                        }
                    }
                    else
                    {
                        Assert.Fail();
                    }
                }
                else
                {
                    Assert.Fail();
                }
            });
        }

        [Test]
        public void SubOperatorTest()
        {
            var text = $"{TokenConstants.RelationOperatorMappings[RelationOperatorType.Greater]} 37413.cc";
            var source = new SourceFile("test", text);
            var result = Operator.Build(source, 0);

            Assert.Multiple(() =>
            {
                if (result is not null)
                {
                    Assert.That(result, Is.Not.EqualTo(null));
                    if (result.Section is not null)
                    {
                        Assert.That(result.Section.TokenType, Is.EqualTo(TokenType.Operator));

                        var operatorToken = result.Section.GetOperator();
                        if (operatorToken is not null)
                        {
                            Assert.That(operatorToken.RelationOperator, Is.EqualTo(RelationOperatorType.Greater));
                            Assert.That(operatorToken.CalculationOperator, Is.EqualTo(CalculationOperatorType.Invalid));
                        }
                    }
                    else
                    {
                        Assert.Fail();
                    }
                }
                else
                {
                    Assert.Fail();
                }
            });
        }

        [Test]
        public void WhitespaceTest()
        {
            var text = $"\t\r\n   37413.cc";
            var source = new SourceFile("test", text);
            var result = Whitespace.Build(source, 0);

            Assert.Multiple(() =>
            {
                if (result is not null)
                {
                    Assert.That(result, Is.Not.EqualTo(null));

                    if (result.Section is not null)
                    {
                        Assert.That(result.Section.TokenType, Is.EqualTo(TokenType.Whitespace));
                        Assert.That(result.Section.Position, Has.Length.EqualTo(6));
                    }
                    else
                    {
                        Assert.Fail();
                    }
                }
                else
                {
                    Assert.Fail();
                }
            });
        }

        [Test]
        public void IdentifierTest()
        {
            var text = $"arc 37413.cc";
            var source = new SourceFile("test", text);
            var result = Identifier.Build(source, 0);

            Assert.Multiple(() =>
            {
                if (result is not null)
                {
                    Assert.That(result, Is.Not.EqualTo(null));

                    if (result.Section is not null)
                    {
                        Assert.That(result.Section.TokenType, Is.EqualTo(TokenType.Identifier));
                        Assert.That(result.Section.GetIdentifier(), Has.Length.EqualTo(3));
                    }
                    else
                    {
                        Assert.Fail();
                    }
                }
                else
                {
                    Assert.Fail();
                }
            });
        }

        [Test]
        public void IdentifierToleranceTest()
        {
            var text = $"implement 37413.cc";
            var source = new SourceFile("test", text);
            var result = Identifier.Build(source, 0);

            Assert.Multiple(() =>
            {
                if (result is not null)
                {
                    Assert.That(result, Is.Not.EqualTo(null));

                    if (result.Section is not null)
                    {
                        Assert.That(result.Section.TokenType, Is.EqualTo(TokenType.Identifier));
                        Assert.That(result.Section.GetIdentifier(), Is.EqualTo("implement"));
                    }
                    else
                    {
                        Assert.Fail();
                    }
                }
                else
                {
                    Assert.Fail();
                }
            });
        }

        [Test]
        public void NumberTest()
        {
            var text = $"-433.24 37413.cc";
            var source = new SourceFile("test", text);
            var result = Number.Build(source, 0);

            Assert.Multiple(() =>
            {
                if (result is not null)
                {
                    Assert.That(result, Is.Not.EqualTo(null));

                    if (result.Section is not null)
                    {
                        Assert.That(result.Section.TokenType, Is.EqualTo(TokenType.Number));
                        Assert.That(result.Section.GetNumber(), Is.EqualTo("-433.24"));
                    }
                    else
                    {
                        Assert.Fail();
                    }
                }
                else
                {
                    Assert.Fail();
                }
            });
        }

        [Test]
        public void SemicolonTest()
        {
            var text = $"-433.24 ; 37413.cc";
            var source = new SourceFile("test", text);
            var result = Semicolon.Build(source, text.IndexOf(';'));

            Assert.Multiple(() =>
            {
                if (result is not null)
                {
                    Assert.That(result, Is.Not.EqualTo(null));

                    if (result.Section is not null)
                    {
                        Assert.That(result.Section.TokenType, Is.EqualTo(TokenType.Semicolon));
                    }
                    else
                    {
                        Assert.Fail();
                    }
                }
                else
                {
                    Assert.Fail();
                }
            });
        }

        [Test]
        public void CommentTest()
        {
            var text = $"7890 // open port 37413";
            var source = new SourceFile("test", text);
            var result = Comment.Build(source, 5);

            Assert.Multiple(() =>
            {
                if (result is not null)
                {
                    Assert.That(result, Is.Not.EqualTo(null));

                    if (result.Section is not null)
                    {
                        Assert.That(result.Section.TokenType, Is.EqualTo(TokenType.Comment));
                        Assert.That(result.Section.Position, Has.Length.EqualTo(18));
                    }
                    else
                    {
                        Assert.Fail();
                    }
                }
                else
                {
                    Assert.Fail();
                }
            });
        }

        [Test]
        public void StringTest()
        {
            var text = $"\"Welcome to \\\"The Arc Programming Language\\\"!\"";
            var source = new SourceFile("test", text);
            var result = Str.Build(source, 0);

            Assert.Multiple(() =>
            {
                if (result is not null)
                {
                    Assert.That(result, Is.Not.EqualTo(null));

                    if (result.Section is not null)
                    {
                        Assert.That(result.Section.TokenType, Is.EqualTo(TokenType.String));
                        Assert.That(result.Section.Position, Has.Length.EqualTo(46));
                    }
                    else
                    {
                        Assert.Fail();
                    }
                }
                else
                {
                    Assert.Fail();
                }
            });
        }
    }
}
