using Arc.Compiler.Shared.LexicalAnalysis;
using Arc.Compiler.Shared.Parsing.AST;
using Arc.Compiler.Shared.Parsing.Components.Data;
using Arc.Compiler.Shared.Utils;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Reflection.Metadata;
using System.Text;
using System.Threading.Tasks;

namespace Arc.Compiler.Parser.Builders.Components.Data
{
    public class DataDeclaratorBuilder
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
            if (dataTypeSection is not null)
            {
                currentIndex += dataTypeSection.Length;

                // Build identifier
                var identifierResult = IdentifierBuilder.Build(tokens[currentIndex..]);
                if (identifierResult is not null)
                {
                    currentIndex += identifierResult.Length;

                    return new(new(dataTypeSection.Section, identifierResult.Section, isConstant), currentIndex);
                }
            }

            return null;
        }
    }
}
