using Arc.Compiler.PackageGenerator.Ginkgo.Metadata.Data;
using Arc.Compiler.PackageGenerator.Ginkgo.Transformation;
using Arc.Compiler.PackageGenerator.Models.Logging;
using Arc.Compiler.SyntaxAnalyzer.Models.Statements;
using Microsoft.Extensions.Logging;

namespace Arc.Compiler.PackageGenerator.Ginkgo.Generator;

public static class ArcLocalDataDeclarationGenerator
{
    public static ArcGinkgoInstructionGenerationResult Generate(
        ref ArcFunctionGenerationContext ctx,
        ArcStatementDeclaration syntax)
    {
        var declarator = syntax.DataDeclarator;

        var result = new ArcGinkgoInstructionGenerationResult();
        if (ctx.ImplicitRegisterRegistry.ContainsKey(declarator.Identifier.Name))
        {
            result.Logs.Add(new ArcSourceLocatableLog(LogLevel.Error, 0, "Duplicated variable definition", ctx.UnitName, declarator.Context.arc_single_identifier()));
            return result;
        }

        var maxImpRegs = ushort.MaxValue;
        if (ctx.ImplicitRegisterRegistry.Count >= maxImpRegs)
        {
            result.Logs.Add(new ArcSourceLocatableLog(LogLevel.Error, 0, $"Too many variable definitions ({maxImpRegs} entries maximum)", ctx.UnitName, syntax.Context));
            return result;
        }

        if (syntax.InitialValueExpression is not null)
        {
            return ArcExpressionEvaluationGenerator.Generate(ref ctx, syntax.InitialValueExpression, declarator);
        }
        
        var (localObj, logs) = ArcImplicitRegister.Create(ctx, declarator, ArcImplicitRegisterType.LocalData);

        if (localObj is null)
        {
            result.Logs.AddRange(logs);
            return result;
        }
        
        ctx.AddImplicitRegister(localObj);

        return result;
    }
}