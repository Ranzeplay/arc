using Arc.Compiler.PackageGenerator.Ginkgo.Metadata.Data;
using Arc.Compiler.PackageGenerator.Ginkgo.Transformation;
using Arc.Compiler.SyntaxAnalyzer.Models.Components.CallChain;
using Arc.Compiler.SyntaxAnalyzer.Models.Statements;
using Microsoft.Extensions.Logging;

namespace Arc.Compiler.PackageGenerator.Ginkgo.Generator;

public static class ArcAssignmentGenerator
{
    public static ArcGinkgoInstructionGenerationResult Generate(
        ref ArcFunctionGenerationContext ctx,
        ArcStatementAssign syntax)
    {
        var result = new ArcGinkgoInstructionGenerationResult();
        
        // Resolve target implicit register
        var targetLocator = GenerateTargetLocator(ref ctx, syntax.CallChain);
        result.Append(targetLocator);

        var targetReg = targetLocator.OutImplicitRegister;

        var exprEval = ArcExpressionEvaluationGenerator.Generate(ref ctx, syntax.Expression, targetReg);
        result.Append(exprEval);
        
        return result;
    }

    public static ArcGinkgoInstructionGenerationResult GenerateTargetLocator(
        ref ArcFunctionGenerationContext ctx,
        ArcCallChain syntax)
    {
        if (syntax.ConstructorCall is not null || syntax.Terms.Any(t => t.Type == ArcCallChainTermType.FunctionCall))
        {
            return ArcGinkgoInstructionGenerationResult.WithLog(LogLevel.Error, 0, "Invalid assignment target", ctx.UnitName, syntax.Terms.First(t => t.Type == ArcCallChainTermType.FunctionCall));
        }

        var terms = syntax.Terms.Select(t => t.Identifier!).ToList();
        var firstTerm = terms.First();

        var localObj = ctx.GetImplicitRegister(firstTerm.Name);
        if (localObj is null)
        {
            return ArcGinkgoInstructionGenerationResult.WithLog(LogLevel.Error, 0, "Invalid local variable name", ctx.UnitName, syntax.Terms.First(t => t.Type == ArcCallChainTermType.FunctionCall));
        }

        foreach (var term in terms.Skip(1))
        {
            var dt = ArcDataTypeHelper.GetDataType(ctx, localObj.Instance.Type.ResolvedType);
        }
    }
}