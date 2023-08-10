using Arc.Compiler.Shared.LexicalAnalysis;
using Arc.Compiler.Shared.Parsing.Components.Data;
using Arc.Compiler.Shared.Parsing.Components.Function;

namespace Arc.Compiler.Parser.Models
{
    public class ExpressionBuildModel
    {
        public Token[] Tokens { get; set; } 
        
        public DataDeclarator[] DeclaredData { get; set; } 

        public FunctionDeclarator[] DeclaredFunctions { get; set; }

        public ExpressionBuildModel(Token[] tokens, DataDeclarator[] declaredData, FunctionDeclarator[] declaredFunctions)
        {
            Tokens = tokens;
            DeclaredData = declaredData;
            DeclaredFunctions = declaredFunctions;
        }

        public ExpressionBuildModel SkipTokens(int count)
        {
            return new(Tokens[count..], DeclaredData, DeclaredFunctions);
        }
    }
}
