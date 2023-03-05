using Arc.Compiler.Lexer;
using Arc.Compiler.Parser.Builders.Components.Expression;
using Arc.Compiler.Shared.Compilation;
using Arc.Compiler.Shared.Parsing.Components.Data;
using Arc.Compiler.Shared.Parsing.Components.Function;
using Arc.CompilerCommandGenerator;
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

            var result = Utils.ExpressionInfixToPostfix(expressionBlock!.Section);

            Assert.That(result, Is.Not.Null);
            if(result != null)
            {
                Assert.That(result.Terms, Has.Length.EqualTo(11));
            }
        }
    }
}
