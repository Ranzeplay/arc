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
    public record ExpressionBuildModel(Token[] Tokens, DataDeclarator[] DeclaredData, FunctionDeclarator[] DeclaredFunctions)
    {
        public ExpressionBuildModel SkipTokens(int count)
        {
            return new(Tokens[count..], DeclaredData, DeclaredFunctions);
        }
    }
}
