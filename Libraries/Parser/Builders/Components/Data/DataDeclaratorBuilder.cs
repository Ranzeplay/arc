using Arc.Compiler.Shared.LexicalAnalysis;
using Arc.Compiler.Shared.Parsing.Components.Data;
using Arc.Compiler.Shared.Utils;

namespace Arc.Compiler.Parser.Builders.Components.Data
{
    internal class DataDeclaratorBuilder
    {
        public static SectionBuildResult<DataDeclarator>? Build(Token[] tokens)
        {
            // Detect whether it is a variable or constant
            bool isConstant;
            var vc = tokens[0].GetKeyword().GetValueOrDefault();
            if (vc == KeywordToken.Var)
            {
                isConstant = false;
            }
            else if (vc == KeywordToken.Const)
            {
                isConstant = true;
            }
            else
            {
                throw new ArgumentException("Modifier must be var or const");
                // return null;
            }

            var currentIndex = 1;
            var dataTypeSection = DataTypeBuilder.Build(tokens[currentIndex..]);
            if (dataTypeSection is null)
            {
                return null;
            }

            currentIndex += dataTypeSection.Length;

            // Build identifier
            var identifierResult = IdentifierBuilder.Build(tokens[currentIndex..]);
            if (identifierResult is null)
            {
                return null;
            }

            currentIndex += identifierResult.Length;
            return new(new(dataTypeSection.Section, identifierResult.Section, isConstant), currentIndex);
        }
    }
}
