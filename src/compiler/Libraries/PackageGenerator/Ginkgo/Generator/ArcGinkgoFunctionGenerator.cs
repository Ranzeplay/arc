using Arc.Compiler.PackageGenerator.Ginkgo.Instruction;
using Arc.Compiler.PackageGenerator.Ginkgo.Metadata.Data;
using Arc.Compiler.PackageGenerator.Ginkgo.Scope;
using Arc.Compiler.PackageGenerator.Ginkgo.Transformation;
using Arc.Compiler.SyntaxAnalyzer.Models.Blocks;
using Arc.Compiler.SyntaxAnalyzer.Models.Statements;
using Microsoft.Extensions.Logging;

namespace Arc.Compiler.PackageGenerator.Ginkgo.Generator;

/// <summary>Entry point for lowering a resolved function without changing the legacy package pipeline.</summary>
public static class ArcGinkgoFunctionGenerator
{
    public static ArcGinkgoInstructionGenerationResult Generate(
        ArcGinkgoUnitGenerationContext unit, ArcScopeTreeFunctionNodeBase function, ArcBlockSequentialExecution body)
    {
        try
        {
            var result = GenerateCore(unit, function, body);
            if (!result.DiscardResult)
            {
                return result;
            }

            function.GenerationResult = ArcGinkgoAssembler.Assemble(result);
            function.BlockLength = 0;
            return result;
        }
        catch (OverflowException)
        {
            return Failure("Ginkgo operand exceeds its supported width");
        }
        catch (InvalidOperationException error) when (error.Message == "Ginkgo function exceeds 65536 registers")
        {
            return Failure(error.Message);
        }

        ArcGinkgoInstructionGenerationResult Failure(string message)
        {
            var failed = ArcGinkgoInstructionGenerationResult.WithLog(LogLevel.Error, 0, message, unit.UnitName);
            function.GenerationResult = ArcGinkgoAssembler.Assemble(failed);
            function.BlockLength = 0;
            return failed;
        }
    }

    private static ArcGinkgoInstructionGenerationResult GenerateCore(
        ArcGinkgoUnitGenerationContext unit, ArcScopeTreeFunctionNodeBase function, ArcBlockSequentialExecution body)
    {
        var ctx = new ArcFunctionGenerationContext(unit, function, unit.LinkedTypes,
        [
            .. unit.LinkedGenericTypes.Concat(function.GenericTypes),
            .. (function.Parent as ArcScopeTreeGroupNode)?.GenericTypes ?? []
        ]);
        
        var result = new ArcGinkgoInstructionGenerationResult();
        foreach (var parameter in function.Parameters)
        {
            var (register, logs) =
                ArcImplicitRegister.Create(ctx, parameter.DataType, ArcImplicitRegisterType.Argument);
            result.Logs.AddRange(logs);
            
            if (register is null || result.DiscardResult)
            {
                return result;
            }
            
            if (ctx.GetImplicitRegister(register.Name) is not null)
            {
                return ArcGinkgoInstructionGenerationResult.WithLog(LogLevel.Error, 0, "Duplicate parameter",
                    unit.UnitName);
            }
            
            ctx.AddImplicitRegister(register);
        }

        result.Append(ArcFunctionBodyGenerator.Generate(ref ctx, body));
        if (function.ReturnValueType.Type.TypeId == 0 && function.ReturnValueType.Dimension == 0)
        {
            result.Instructions.Add(new ArcControlFlowReturnInstruction());
        }
        else if (!Returns(body))
        {
            result.Append(ArcGinkgoInstructionGenerationResult.WithLog(LogLevel.Error, 0,
                "Value-returning function may fall through without a return", unit.UnitName));
        }
        
        function.DataCount = ctx.RegisterCount;
        function.GenerationResult = ArcGinkgoAssembler.Assemble(result);
        result.Logs.AddRange(function.GenerationResult.Logs.Skip(result.Logs.Count));
        function.BlockLength = function.GenerationResult.GeneratedData.Count;
        return result;
    }

    private static bool Returns(ArcBlockSequentialExecution body)
        => body.ExecutionSteps.Any(step => step is ArcStatementReturn ||
                                           step is ArcBlockIf conditional && conditional.ElseBody is not null &&
                                           conditional.ConditionalBlocks.All(branch => Returns(branch.Body)) &&
                                           Returns(conditional.ElseBody));
}