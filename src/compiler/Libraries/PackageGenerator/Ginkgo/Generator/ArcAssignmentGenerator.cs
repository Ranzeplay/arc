using Antlr4.Runtime;
using Arc.Compiler.PackageGenerator.Ginkgo.Instruction;
using Arc.Compiler.PackageGenerator.Ginkgo.Metadata.Data;
using Arc.Compiler.PackageGenerator.Ginkgo.Transformation;
using Arc.Compiler.PackageGenerator.Models.Relocation;
using Arc.Compiler.SyntaxAnalyzer.Interfaces;
using Arc.Compiler.SyntaxAnalyzer.Models.Components.CallChain;
using Arc.Compiler.SyntaxAnalyzer.Models.Data;
using Arc.Compiler.SyntaxAnalyzer.Models.Statements;
using Microsoft.Extensions.Logging;

namespace Arc.Compiler.PackageGenerator.Ginkgo.Generator;

public static class ArcAssignmentGenerator
{
    public static ArcGinkgoInstructionGenerationResult Generate(
        ref ArcFunctionGenerationContext ctx, ArcStatementAssign syntax)
    {
        var terms = syntax.CallChain.Terms.ToList();
        if (syntax.CallChain.ConstructorCall is not null || terms.Count == 0 ||
            terms.Any(t => t.Type != ArcCallChainTermType.Identifier || t.Indices.Any()))
        {
            return Error(ctx, "Invalid assignment target", syntax);
        }
        
        var result = new ArcGinkgoInstructionGenerationResult();
        var assignmentTarget = ctx.GetImplicitRegister(terms.First().Identifier!.Name);
        if (assignmentTarget is null)
        {
            return Error(ctx, "Unknown assignment target", terms.First());
        }
        
        // Early detection and report
        if (assignmentTarget.Instance.Mutability == ArcMutability.Constant && terms.Count == 1)
        {
            return Error(ctx, "Cannot assign to a constant", terms.First());
        }

        // Traverse intermediate fields once
        // Give the final field a store location
        for (var i = 1; i < terms.Count - 1; i++)
        {
            var type = ArcDataTypeHelper.GetDataTypeNode(ctx, assignmentTarget.Instance.Type.ResolvedType);
            var field = type?.ComplexTypeGroup?.Fields.FirstOrDefault(f => f.IdentifierName == terms[i].Identifier!.Name);
            if (field is null)
            {
                return Error(ctx, "Unknown field in assignment target", syntax.Expression);
            }
            var (next, logs) = ArcImplicitRegister.CreateAnonymous(ctx, field.DataType, ArcImplicitRegisterType.LocalData);
            result.Logs.AddRange(logs);
            if (next is null) return result;
            ctx.AddImplicitRegister(next);
            result.Instructions.Add(new ArcDataLoadFieldInstruction { Src = assignmentTarget.Id, Dest = next.Id, FieldId = field.Id });
            assignmentTarget = next;
        }
        
        var targetType = assignmentTarget.Instance;
        uint? fieldId = null;
        if (terms.Count > 1)
        {
            var type = ArcDataTypeHelper.GetDataTypeNode(ctx, assignmentTarget.Instance.Type.ResolvedType);
            var field = type?.ComplexTypeGroup?.Fields.FirstOrDefault(f => f.IdentifierName == terms[^1].Identifier!.Name);
            if (field is null)
            {
                return Error(ctx, "Unknown field in assignment target", syntax.Expression);
            }
            var (instance, logs) = ArcDataTypeInstance.Create(ctx, field.DataType);
            result.Logs.AddRange(logs);
            
            if (instance is null)
            {
                return result;
            }
            
            targetType = instance;
            fieldId = field.Id;
            
            if (targetType.Mutability == ArcMutability.Constant)
            {
                return Error(ctx, "Cannot assign to a constant field", syntax.Expression);
            }
        }
        
        ArcRelocationLabel? end = null;
        if (syntax.AssignIfNull)
        {
            var condition = assignmentTarget.Id;
            if (fieldId.HasValue)
            {
                var (current, logs) = ArcImplicitRegister.CreateAnonymous(ctx, targetType, ArcImplicitRegisterType.LocalData);
                result.Logs.AddRange(logs);
                if (current is null) return result;
                ctx.AddImplicitRegister(current);
                result.Instructions.Add(new ArcDataLoadFieldInstruction
                    { Src = assignmentTarget.Id, Dest = current.Id, FieldId = fieldId.Value });
                condition = current.Id;
            }
            end = ArcRelocationLabel.Create();
            result.Instructions.Add(new ArcControlFlowConditionalJumpInstruction
                { Condition = condition, ConditionNotNone = true, Target = ArcRelocationTarget.ForLabel(end) });
        }

        // Evaluate the RHS only when the conditional assignment needs a value.
        var value = ArcExpressionEvaluationGenerator.Generate(ref ctx, syntax.Expression);
        result.Append(value);
        
        if (result.DiscardResult || value.OutImplicitRegister is null)
        {
            return result;
        }
        
        if (targetType.Type.ActualTypeId != value.OutImplicitRegister.Instance.Type.ActualTypeId ||
            targetType.Dimension != value.OutImplicitRegister.Instance.Dimension)
        {
            return Error(ctx, "Assignment type mismatch", syntax.Expression);
        }
        
        if (fieldId.HasValue)
            result.Instructions.Add(new ArcDataStoreFieldInstruction
                { Src = value.OutImplicitRegister.Id, Dest = assignmentTarget.Id, FieldId = fieldId.Value });
        else
        {
            result.OutImplicitRegister = value.OutImplicitRegister;
            result.OutputInvoke = value.OutputInvoke;
            result.StoreOutput(assignmentTarget);
        }

        if (end is not null)
            result.Instructions.Add(new ArcControlFlowLabelInstruction { Label = end });
        
        return result;
    }

    public static ArcGinkgoInstructionGenerationResult GenerateTargetLocator(
        ref ArcFunctionGenerationContext ctx, ArcCallChain syntax)
        => ArcCallChainGenerator.Generate(ref ctx, syntax);

    private static ArcGinkgoInstructionGenerationResult Error(ArcFunctionGenerationContext ctx, string message, IArcTraceable<ParserRuleContext> part)
        => ArcGinkgoInstructionGenerationResult.WithLog(LogLevel.Error, 0, message, ctx.UnitName, part);
}
