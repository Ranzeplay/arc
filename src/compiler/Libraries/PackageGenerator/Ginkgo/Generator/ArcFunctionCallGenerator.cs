using Arc.Compiler.PackageGenerator.Ginkgo.Instruction;
using Arc.Compiler.PackageGenerator.Ginkgo.Metadata;
using Arc.Compiler.PackageGenerator.Ginkgo.Metadata.Data;
using Arc.Compiler.PackageGenerator.Ginkgo.Scope;
using Arc.Compiler.PackageGenerator.Ginkgo.Transformation;
using Arc.Compiler.PackageGenerator.Models.Logging;
using Arc.Compiler.SyntaxAnalyzer.Models.Function;
using Microsoft.Extensions.Logging;

namespace Arc.Compiler.PackageGenerator.Ginkgo.Generator;

public static class ArcFunctionCallGenerator
{
    public static ArcGinkgoInstructionGenerationResult Generate(
        ref ArcFunctionGenerationContext ctx,
        ArcFunctionCall syntax)
    {
        var result = new ArcGinkgoInstructionGenerationResult();
        
        // Locate function
        var (func, funcNodeSearchLog) = GetFunctionNode(ctx.GlobalContext, syntax);
        if (func is null)
        {
            result.Logs.AddRange(funcNodeSearchLog);
        }
        
        // Build arguments
        var args = new ArcGinkgoInstructionGenerationResult[func?.Parameters.Count() ?? syntax.Arguments.Count()];
        for (var i = 0; i < syntax.Arguments.Count(); i++)
        {
            var expr = syntax.Arguments.ElementAt(i).Expression;
            if (i < args.Length)
            {
                args[i] = ArcExpressionEvaluationGenerator.Generate(ref ctx, expr);
                // TODO: Verify argument type
                result.Append(args[i]);
            }
            else
            {
                result.Logs.Add(new ArcSourceLocatableLog(LogLevel.Error, 0, $"Too many arguments for argument {i}", ctx.UnitName, expr));
            }
        }

        if (syntax.Arguments.Count() < func?.Parameters.Count())
        {
            result.Logs.Add(new ArcSourceLocatableLog(LogLevel.Error, 0, $"Too few arguments, {func.Parameters.Count()} required but only {syntax.Arguments.Count()} provided", ctx.UnitName, syntax.Identifier));
        }
        
        // Encapsulate function call
        result.Instructions.AddRange([
            new ArcControlFlowPrepareCallInstruction(func?.Id ?? 0),
            ..args.Select(a => new ArcControlFlowBindArgInstruction{ Src = a.OutImplicitRegister.Id }),
            ArcControlFlowInvokeInstruction.Normal
        ]);
        
        return result;
    }

    public static ArcGinkgoInstructionGenerationResult GenerateSelfCall(
        ref ArcFunctionGenerationContext ctx,
        ArcFunctionCall syntax,
        ArcImplicitRegister baseObj)
    {
        throw new NotImplementedException();
    }

    private static (ArcScopeTreeFunctionNodeBase?, IEnumerable<ArcCompilationLogBase>) GetFunctionNode(
        ArcGinkgoUnitGenerationContext globalContext,
        ArcFunctionCall syntax)
    {
        if (syntax.Identifier.Namespace is not null)
        {
            var funcDecl = globalContext.ScopeTree
                .GetNode<ArcScopeTreeFunctionNodeBase>(syntax.Identifier.NameArray);

            if (funcDecl == null)
            {
                return (null, [
                    new ArcSourceLocatableLog(LogLevel.Error, 0, $"Function \"{syntax.Identifier}\" does not exist in explicit location", globalContext.UnitName, syntax.Identifier)
                ]);
            }

            return (funcDecl, []);
        }
        
        var funcNode = globalContext.LinkedNodes
            .OfType<ArcScopeTreeFunctionNodeBase>()
            .FirstOrDefault(n => n.Name == syntax.Identifier.Name);

        if (funcNode == null)
        {
            return (null, [
                new ArcSourceLocatableLog(LogLevel.Error, 0, $"Function \"{syntax.Identifier}\" does not exist in any linked namespace", globalContext.UnitName, syntax.Identifier)
            ]);
        }

        return (funcNode, []);
    }
}