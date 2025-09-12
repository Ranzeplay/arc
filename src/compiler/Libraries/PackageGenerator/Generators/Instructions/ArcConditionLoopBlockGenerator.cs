using Arc.Compiler.PackageGenerator.Models.Generation;
using Arc.Compiler.PackageGenerator.Models.PrimitiveInstructions;
using Arc.Compiler.PackageGenerator.Models.Relocation;
using Arc.Compiler.PackageGenerator.Models.Scope;
using Arc.Compiler.SyntaxAnalyzer.Models.Blocks;

namespace Arc.Compiler.PackageGenerator.Generators.Instructions
{
    internal class ArcConditionLoopBlockGenerator
    {
        public static ArcPartialGenerationResult EncodeWhileLoop(ArcGenerationSource source, ArcBlockConditionalLoop clBlock, ArcScopeTreeFunctionNodeBase fnNode)
        {
            var relocationLayer = Guid.NewGuid();

            var result = new ArcPartialGenerationResult();

            var beginBlockLabel = new ArcLabellingInstruction(ArcRelocationLabelType.BeginLoopBlock, "begin", relocationLayer).Encode(source);

            var expr = ArcExpressionEvaluationGenerator.GenerateEvaluationCommand(source, clBlock.ConditionalBlock.Expression);

            var body = ArcSequentialExecutionGenerator.Generate(source, clBlock.ConditionalBlock.Body, fnNode);

            var jumpOutRelocator = new ArcRelocationTarget()
            {
                TargetType = ArcRelocationTargetType.Label,
                Label = ArcRelocationLabelType.EndLoopBlock,
                Parameter = 1,
                Layer = relocationLayer
            };
            var jumpOutInstruction = new ArcConditionalJumpInstruction(jumpOutRelocator).Encode(source);

            var jumpBackRelocator = new ArcRelocationTarget()
            {
                TargetType = ArcRelocationTargetType.Label,
                Label = ArcRelocationLabelType.BeginLoopBlock,
                Parameter = -1,
                Layer = relocationLayer
            };
            var jumpBackInstruction = new ArcUnconditionalJumpInstruction(jumpBackRelocator).Encode(source);

            var endBlockLabel = new ArcLabellingInstruction(ArcRelocationLabelType.EndLoopBlock, "end", relocationLayer).Encode(source);

            result.Append(beginBlockLabel);
            result.Append(expr);
            result.Append(jumpOutInstruction);
            result.Append(body);
            result.Append(jumpBackInstruction);
            result.Append(endBlockLabel);

            return result;
        }
        
        public static ArcPartialGenerationResult EncodeForLoop(ArcGenerationSource source, ArcBlockExtendedConditionalLoop forBlock, ArcScopeTreeFunctionNodeBase fnNode)
        {
            var relocationLayer = Guid.NewGuid();

            var result = new ArcPartialGenerationResult();
            
            var init = ArcDeclarationStatementGenerator.Generate(forBlock.Initializer, source, fnNode);
            result.Append(init);

            source.LocalDataSlots.Add(init.DataSlots.First());

            var beginBlockLabel = new ArcLabellingInstruction(ArcRelocationLabelType.BeginLoopBlock, "begin", relocationLayer).Encode(source);

            var expr = ArcExpressionEvaluationGenerator.GenerateEvaluationCommand(source, forBlock.Condition);
            
            var body = ArcSequentialExecutionGenerator.Generate(source, forBlock.Body, fnNode);
            var iterator = forBlock.Iterator.Generate(source);
            
            var jumpBackRelocator = new ArcRelocationTarget()
            {
                TargetType = ArcRelocationTargetType.Label,
                Label = ArcRelocationLabelType.BeginLoopBlock,
                Parameter = -1,
                Layer = relocationLayer
            };
            var jumpBackInstruction = new ArcConditionalJumpInstruction(jumpBackRelocator).Encode(source);

            var endBlockLabel = new ArcLabellingInstruction(ArcRelocationLabelType.EndLoopBlock, "end", relocationLayer).Encode(source);

            result.Append(init);
            result.Append(beginBlockLabel);
            result.Append(expr);
            result.Append(body);
            result.Append(iterator);
            result.Append(jumpBackInstruction);
            result.Append(endBlockLabel);
            
            result.DataSlots.RemoveAt(result.DataSlots.Count - 1);

            return result;
        }
    }
}
