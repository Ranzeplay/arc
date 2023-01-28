using Arc.Compiler.Parser.Builders.Components;
using Arc.Compiler.Parser.Builders.Components.Data;
using Arc.Compiler.Shared.LexicalAnalysis;
using Arc.Compiler.Shared.Parsing.Components;
using Arc.Compiler.Shared.Parsing.Components.Data;
using Arc.Compiler.Shared.Utils;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Arc.Compiler.Parser.Builders.Blocks
{
    public class DataDeclaratorBuilder
    {
        public static SectionBuildResult<DataDeclarator>? Build(Token[] tokens)
        {
            if (tokens[0].GetKeyword() is not null)
            {
                var keyword = tokens[0].GetKeyword();
                if (keyword == KeywordToken.Declare)
                {
                    // Detect whether it is a variable or constant
                    if (tokens[1].GetKeyword() is not null)
                    {
                        bool isConstant;
                        var vc = tokens[1].GetKeyword();
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

                        var currentIndex = 2;
                        var dataTypeSection = DataTypeBuilder.Build(tokens[currentIndex..]);
                        if (dataTypeSection is not null)
                        {
                            currentIndex += dataTypeSection.Length;

                            // Build identifier
                            var identifierResult = IdentifierBuilder.Build(tokens[currentIndex..]);
                            if (identifierResult is not null)
                            {
                                currentIndex += identifierResult.Length;

                                // This is a statement
                                if (tokens[currentIndex].TokenType == TokenType.Semicolon)
                                {
                                    currentIndex++;
                                    return new(new(dataTypeSection.Section, identifierResult.Section, isConstant), currentIndex);
                                }
                            }
                        }
                    }
                }
            }

            return null;
        }
    }
}
