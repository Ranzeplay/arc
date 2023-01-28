using Arc.Compiler.Shared.LexicalAnalysis;
using Arc.Compiler.Shared.Parsing.Components;
using Arc.Compiler.Shared.Parsing.Components.Data;
using Arc.Compiler.Shared.Parsing.Components.Function;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Arc.Compiler.Parser.Models
{
    public class ExpressionBuildModel
    {
        public Token[] Tokens { get; private set; } = Array.Empty<Token>();

        public DataDeclarator[] DeclaredData { get; }

        public FunctionDeclarator[] DeclaredFunctions { get; }

        public ExpressionBuildModel(Token[] tokens, DataDeclarator[] dataDeclarators, FunctionDeclarator[] functionDeclarators)
        {
            Tokens = tokens;
            DeclaredData = dataDeclarators;
            DeclaredFunctions = functionDeclarators;
        }

        public ExpressionBuildModel SkipTokens(int count)
        {
            var clone = (ExpressionBuildModel)MemberwiseClone();
            clone.Tokens = clone.Tokens[count..];

            return clone;
        }
    }
}
