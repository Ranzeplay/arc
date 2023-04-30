using Arc.Compiler.Shared.Parsing.AST;
using Arc.CompilerCommandGenerator.Models;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Arc.CompilerCommandGenerator.Builders
{
    internal class LoopCommand
    {
        public static PartialGenerationResult? Build(GenerationContext<ConditionalLoopBlock> source)
        {
            var expression = ExpressionCommand.BuildRelationalExpression(source.TransferToNewComponent(source.Component.Condition));


            return null;
        }
    }
}
