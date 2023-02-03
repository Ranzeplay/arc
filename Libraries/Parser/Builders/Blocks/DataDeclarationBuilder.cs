using Arc.Compiler.Parser.Builders.Components;
using Arc.Compiler.Parser.Builders.Components.Data;
using Arc.Compiler.Shared.LexicalAnalysis;
using Arc.Compiler.Shared.Parsing.AST;
using Arc.Compiler.Shared.Parsing.Components;
using Arc.Compiler.Shared.Parsing.Components.Data;
using Arc.Compiler.Shared.Utils;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Reflection.Metadata;
using System.Text;
using System.Threading.Tasks;

namespace Arc.Compiler.Parser.Builders.Blocks
{
    public class DataDeclarationBuilder
    {
        public static SectionBuildResult<DataDeclarationBlock>? Build(Token[] tokens)
        {
            if (tokens[0].GetKeyword().GetValueOrDefault() == KeywordToken.Declare)
            {
                var declaratorResult = DataDeclaratorBuilder.Build(tokens[1..]);
                if (declaratorResult == null)
                {
                    return null;
                }

                // This is a statement
                if (tokens[1 + declaratorResult.Length].TokenType == TokenType.Semicolon)
                {
                    var declarator = declaratorResult.Section;
                    return new(new(declarator.DataType, declarator.Identifier, declarator.IsConstant), declaratorResult.Length + 2);
                }

                return null;
            }

            return null;
        }
    }
}
