using Arc.Compiler.PackageGenerator.Ginkgo.Instruction;
using Arc.Compiler.PackageGenerator.Ginkgo.Metadata.Data;
using Arc.Compiler.PackageGenerator.Ginkgo.Transformation;
using Arc.Compiler.PackageGenerator.Models.Relocation;
using Arc.Compiler.SyntaxAnalyzer.Models.Blocks;
using Arc.Compiler.SyntaxAnalyzer.Models.Components;
using Arc.Compiler.SyntaxAnalyzer.Models.Components.CallChain;
using Arc.Compiler.SyntaxAnalyzer.Models.Function;
using Arc.Compiler.SyntaxAnalyzer.Models.Expression;
using Arc.Compiler.SyntaxAnalyzer.Models.Statements;
using Microsoft.Extensions.Logging;

namespace Arc.Compiler.PackageGenerator.Ginkgo.Generator;

/// <summary>Lowers structured control flow using function-local byte-address labels.</summary>
public static class ArcFunctionBodyGenerator
{
    public static ArcGinkgoInstructionGenerationResult Generate(
        ref ArcFunctionGenerationContext ctx, ArcBlockSequentialExecution body)
        => GenerateBody(ref ctx, body, new Stack<(ArcRelocationLabel Break, ArcRelocationLabel Continue)>());

    private static ArcGinkgoInstructionGenerationResult GenerateBody(
        ref ArcFunctionGenerationContext ctx, ArcBlockSequentialExecution body,
        Stack<(ArcRelocationLabel Break, ArcRelocationLabel Continue)> loops)
    {
        var result = new ArcGinkgoInstructionGenerationResult();
        ctx.EnterScope();
        try
        {
            foreach (var step in body.ExecutionSteps)
            {
                result.AppendInstructions(GenerateStep(ref ctx, step, loops));
                if (result.DiscardResult) break;
                if (step is ArcStatementReturn or ArcStatementBreak or ArcStatementContinue) break;
            }
        }
        finally { ctx.ExitScope(); }
        return result;
    }

    private static ArcGinkgoInstructionGenerationResult GenerateStep(
        ref ArcFunctionGenerationContext ctx, ArcExecutionStepBase step,
        Stack<(ArcRelocationLabel Break, ArcRelocationLabel Continue)> loops)
        => step switch
        {
            ArcStatementDeclaration declaration => ArcLocalDataDeclarationGenerator.Generate(ref ctx, declaration),
            ArcStatementAssign assignment => ArcAssignmentGenerator.Generate(ref ctx, assignment),
            ArcStatementCall call => call.Value switch
            {
                ArcFunctionCall function => ArcFunctionCallGenerator.Generate(ref ctx, function),
                ArcCallChain chain => ArcCallChainGenerator.Generate(ref ctx, chain),
                null => throw new InvalidOperationException("Uninitialized statement call"),
            },
            ArcStatementReturn statement => GenerateReturn(ref ctx, statement),
            ArcStatementBreak => GenerateLoopControl(ctx, loops, true),
            ArcStatementContinue => GenerateLoopControl(ctx, loops, false),
            ArcBlockIf conditional => GenerateConditional(ref ctx, conditional, loops),
            ArcBlockConditionalLoop loop => GenerateLoop(ref ctx, loop.ConditionalBlock.Body, loop.ConditionalBlock.Expression, null, null, loops),
            ArcBlockExtendedConditionalLoop loop => GenerateLoop(ref ctx, loop.Body, loop.Condition, loop.Initializer, loop.Iterator, loops),
            ArcBlockLoop loop => GenerateLoop(ref ctx, loop.Body, null, null, null, loops),
            ArcStatementThrow => Error(ctx, "No Ginkgo lowering for ArcStatementThrow; the ISA must define its runtime semantics"),
            null => throw new ArgumentNullException(nameof(step)),
        };

    private static ArcGinkgoInstructionGenerationResult GenerateReturn(
        ref ArcFunctionGenerationContext ctx, ArcStatementReturn statement)
    {
        var result = new ArcGinkgoInstructionGenerationResult();
        if (statement.Expression is null)
        {
            if (ctx.CurrentNode.ReturnValueType.Type.TypeId != 0 || ctx.CurrentNode.ReturnValueType.Dimension != 0)
                return Error(ctx, "Return value required");
            result.Instructions.Add(new ArcControlFlowReturnInstruction());
        }
        else
        {
            var value = ArcExpressionEvaluationGenerator.Generate(ref ctx, statement.Expression);
            result.AppendInstructions(value);
            if (result.DiscardResult || value.OutImplicitRegister is null) return result;
            var expected = ctx.CurrentNode.ReturnValueType;
            if (expected.Type.TypeId != value.OutImplicitRegister.Instance.Type.ResolvedType.TypeId ||
                expected.Dimension != value.OutImplicitRegister.Instance.Dimension)
                return Error(ctx, "Return type mismatch");
            result.Instructions.Add(new ArcControlFlowReturnWithValueInstruction
            {
                Src = value.OutImplicitRegister.Id,
                ExpectReturnValue = expected.Type.TypeId != 0 || expected.Dimension != 0
            });
        }
        return result;
    }

    private static ArcGinkgoInstructionGenerationResult GenerateLoopControl(
        ArcFunctionGenerationContext ctx,
        Stack<(ArcRelocationLabel Break, ArcRelocationLabel Continue)> loops, bool isBreak)
    {
        var result = new ArcGinkgoInstructionGenerationResult();
        if (loops.Count == 0) return Error(ctx, "Loop control statement outside a loop");
        result.Instructions.Add(Jump(isBreak ? loops.Peek().Break : loops.Peek().Continue));
        return result;
    }

    private static ArcGinkgoInstructionGenerationResult GenerateConditional(
        ref ArcFunctionGenerationContext ctx, ArcBlockIf conditional,
        Stack<(ArcRelocationLabel Break, ArcRelocationLabel Continue)> loops)
    {
        var result = new ArcGinkgoInstructionGenerationResult();
        var end = ArcRelocationLabel.Create();
        foreach (var branch in conditional.ConditionalBlocks)
        {
            var next = ArcRelocationLabel.Create();
            result.AppendInstructions(Condition(ref ctx, branch.Expression, next));
            if (result.DiscardResult) return result;
            result.AppendInstructions(GenerateBody(ref ctx, branch.Body, loops));
            result.Instructions.Add(Jump(end));
            result.Instructions.Add(new ArcControlFlowLabelInstruction { Label = next });
        }
        if (conditional.ElseBody is not null)
            result.AppendInstructions(GenerateBody(ref ctx, conditional.ElseBody, loops));
        result.Instructions.Add(new ArcControlFlowLabelInstruction { Label = end });
        return result;
    }

    private static ArcGinkgoInstructionGenerationResult GenerateLoop(
        ref ArcFunctionGenerationContext ctx, ArcBlockSequentialExecution body, ArcExpression? condition,
        ArcStatementDeclaration? initializer, ArcStatementAssign? iterator,
        Stack<(ArcRelocationLabel Break, ArcRelocationLabel Continue)> loops)
    {
        var result = new ArcGinkgoInstructionGenerationResult();
        var start = ArcRelocationLabel.Create();
        var next = ArcRelocationLabel.Create();
        var end = ArcRelocationLabel.Create();
        ctx.EnterScope();
        try
        {
            if (initializer is not null) result.AppendInstructions(ArcLocalDataDeclarationGenerator.Generate(ref ctx, initializer));
            if (result.DiscardResult) return result;
            result.Instructions.Add(new ArcControlFlowLabelInstruction { Label = start });
            if (condition is not null) result.AppendInstructions(Condition(ref ctx, condition, end));
            if (result.DiscardResult) return result;
            loops.Push((end, iterator is null ? start : next));
            try { result.AppendInstructions(GenerateBody(ref ctx, body, loops)); }
            finally { loops.Pop(); }
            result.Instructions.Add(new ArcControlFlowLabelInstruction { Label = next });
            if (iterator is not null) result.AppendInstructions(ArcAssignmentGenerator.Generate(ref ctx, iterator));
            result.Instructions.Add(Jump(start));
            result.Instructions.Add(new ArcControlFlowLabelInstruction { Label = end });
        }
        finally { ctx.ExitScope(); }
        return result;
    }

    private static ArcGinkgoInstructionGenerationResult Condition(
        ref ArcFunctionGenerationContext ctx, ArcExpression expression, ArcRelocationLabel otherwise)
    {
        var result = ArcExpressionEvaluationGenerator.Generate(ref ctx, expression);
        if (result.DiscardResult || result.OutImplicitRegister is null) return result;
        if (result.OutImplicitRegister.Instance.Type.ShortName != "bool" || result.OutImplicitRegister.Instance.Dimension != 0)
            return Error(ctx, "Condition must be a scalar bool");
        result.Instructions.Add(new ArcControlFlowConditionalJumpInstruction
        {
            Condition = result.OutImplicitRegister.Id, ConditionFalse = true,
            Target = ArcRelocationTarget.ForLabel(otherwise)
        });
        result.SetOutput(null);
        return result;
    }

    private static ArcControlFlowUnconditionalJumpInstruction Jump(ArcRelocationLabel label)
        => new() { Target = ArcRelocationTarget.ForLabel(label) };

    private static ArcGinkgoInstructionGenerationResult Error(ArcFunctionGenerationContext ctx, string message)
        => ArcGinkgoInstructionGenerationResult.WithLog(LogLevel.Error, 0, message, ctx.UnitName);
}
