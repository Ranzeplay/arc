using Arc.Compiler.Lexer;
using Arc.Compiler.Parser.Builders.Components.Expression;
using Arc.Compiler.Shared.CommandGeneration;
using Arc.Compiler.Shared.Compilation;
using Arc.Compiler.Shared.Parsing.Components.Data;
using Arc.Compiler.Shared.Parsing.Components.Function;
using Arc.CompilerCommandGenerator;
using Arc.CompilerCommandGenerator.Builders;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Arc.Compiler.Tests.CommandGeneration
{
    [Category("PackageGeneration")]
    internal class ExpressionTest
    {
        [Test]
        public void InfixToPostfix()
        {
            var text = "2 + (var1 * func1(var2) + (5 + 3) * 2)";
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

            var expressionBlock = ExpressionBuilder.BuildSimpleExpression(new(tokens.Tokens, definedData, definedFunctions));

            var result = expressionBlock!.Section.ToPostfixExpression();

            Assert.That(result, Is.Not.Null);
            if(result != null)
            {
                Assert.That(result.Terms, Has.Length.EqualTo(11));
            }
        }

        [Test]
        public void CommandWithDataAccessor()
        {
            var text = "2 + var1 * var2[136]";
            var source = new SourceFile("test", text);
            var tokens = Tokenizer.Tokenize(source, true);

            var definedData = new List<DataDeclarator>
            {
                new(new(new(Array.Empty<string>(), "type1"), false), new(Array.Empty<string>(), "var1"), false),
                new(new(new(Array.Empty<string>(), "type1"), false), new(Array.Empty<string>(), "var2"), true)
            };

            var expressionBlock = ExpressionBuilder.BuildSimpleExpression(new(tokens.Tokens, definedData.ToArray(), Array.Empty<FunctionDeclarator>()));

            var postfixExpression = expressionBlock!.Section.ToPostfixExpression();

            var metadata = new PackageMetadata(0, 2, 2, 2, 0, 2);
            var result = ExpressionCommand.BuildSimpleExpression(new(postfixExpression, definedData, new(), new(), metadata));

            Assert.That(result, Is.Not.Null);
            if(result != null)
            {
                Assert.Multiple(() =>
                {
                    Assert.That(result.GeneratedConstants.Count(), Is.EqualTo(1));
                    Assert.That(result.Commands, Has.Length.EqualTo(18));
                });
            }
        }

        [Test]
        public void SimpleCommandGeneration()
        {
            var text = "2 + 8 - 3 * \"Hello\"";
            var source = new SourceFile("test", text);
            var tokens = Tokenizer.Tokenize(source, true);

            var expressionBlock = ExpressionBuilder.BuildSimpleExpression(new(tokens.Tokens, Array.Empty<DataDeclarator>(), Array.Empty<FunctionDeclarator>()));

            var postfixExpression = expressionBlock!.Section.ToPostfixExpression();

            var metadata = new PackageMetadata(0, 2, 2, 2, 0, 2);
            var result = ExpressionCommand.BuildSimpleExpression(new(postfixExpression, new(), new(), new(), metadata));

            Assert.That(result, Is.Not.Null);
            if (result != null)
            {
                Assert.Multiple(() =>
                {
                    Assert.That(result.GeneratedConstants.Count(), Is.EqualTo(4));
                    Assert.That(result.Commands, Has.Length.EqualTo(18));
                });
            }
        }

        [Test]
        public void ExpressionWithFunction()
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

            var expressionBlock = ExpressionBuilder.BuildSimpleExpression(new(tokens.Tokens, definedData, definedFunctions));
            var postfixExpression = expressionBlock!.Section.ToPostfixExpression();

            var metadata = new PackageMetadata(0, 2, 2, 2, 0, 2);
            var result = ExpressionCommand.BuildSimpleExpression(new(postfixExpression, definedData.ToList(), new(), definedFunctions.ToList(), metadata));

            Assert.That(result, Is.Not.Null);
            if (result != null)
            {
                Assert.That(result.Commands, Has.Length.EqualTo(18));
            }
        }
    }
}
