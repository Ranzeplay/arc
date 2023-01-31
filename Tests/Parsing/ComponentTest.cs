using Arc.Compiler.Lexer;
using Arc.Compiler.Parser.Builders.Components;
using Arc.Compiler.Parser.Builders.Components.Expression;
using Arc.Compiler.Shared.Compilation;
using Arc.Compiler.Shared.LexicalAnalysis;
using Arc.Compiler.Shared.Parsing.Components;
using Arc.Compiler.Shared.Parsing.Components.Data;
using Arc.Compiler.Shared.Parsing.Components.Expression;
using Arc.Compiler.Shared.Parsing.Components.Function;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Arc.Compiler.Tests.Parsing
{
    [Category("Parsing")]
    public class ComponentTest
    {
        [SetUp]
        public void Setup() { }

        [Test]
        public void IdentifierTest()
        {
            var text = "arc::compiler::identifier_test 37413";
            var source = new SourceFile("test", text);
            var tokens = Tokenizer.Tokenize(source, true);

            var result = IdentifierBuilder.Build(tokens.Tokens);

            Assert.That(result, Is.Not.EqualTo(null));
            if (result is not null)
            {
                Assert.That(result, Has.Length.EqualTo(5));
                Assert.That(result.Section.Name, Is.EqualTo("identifier_test"));
            }
        }

        [Test]
        public void SimpleExpressionTest()
        {
            var text = "2 + (var1 * func1(var2))";
            var source = new SourceFile("test", text);
            var tokens = Tokenizer.Tokenize(source, true);

            var definedData = new DataDeclarator[]
            {
                new(new(new(Array.Empty<string>(), "type1"), false), new(Array.Empty<string>(), "var1"), false),
                new(new(new(Array.Empty<string>(), "type1"), false), new(Array.Empty<string>(), "var2"), false)
            };

            var definedFunctions = new FunctionDeclarator[]
            {
                new(
                    new(Array.Empty<string>(), "func1"),
                    new(new(Array.Empty<string>(), "retType1"), false),
                    new FunctionParameter[]
                    {
                    new(new(new(Array.Empty<string>(), "type1"), false), new(Array.Empty<string>(), "param1"), false)
                    })
            };

            var result = ExpressionBuilder.BuildSimpleExpression(new(tokens.Tokens, definedData, definedFunctions));

            Assert.That(result, Is.Not.EqualTo(null));
            if (result is not null)
            {
                Assert.That(result.Section.Terms, Has.Length.EqualTo(7));
                Assert.That(result.Section.Terms[5].TermType, Is.EqualTo(ExpressionTermType.Data));
                var data = result.Section.Terms[5].GetDataTerm();
                if (data is not null)
                {
                    Assert.That(data.DataTermType, Is.EqualTo(ExpressionDataTermType.FunctionCall));
                }
            }
        }

        [Test]
        public void RelationalExpressionTest()
        {
            var text = "3 > 4 * 2";
            var source = new SourceFile("test", text);
            var tokens = Tokenizer.Tokenize(source, true);

            var result = ExpressionBuilder.BuildRelationalExpression(new(tokens.Tokens, Array.Empty<DataDeclarator>(), Array.Empty<FunctionDeclarator>()));
            Assert.That(result, Is.Not.EqualTo(null));
            if (result is not null)
            {
                Assert.Multiple(() =>
                {
                    Assert.That(result.Section.Relation, Is.EqualTo(RelationOperatorType.Greater));
                    Assert.That(result.Section.LhsExpression.Terms, Has.Length.EqualTo(1));
                    Assert.That(result.Section.RhsExpression.Terms, Has.Length.EqualTo(3));
                });
            }
        }
    }
}
