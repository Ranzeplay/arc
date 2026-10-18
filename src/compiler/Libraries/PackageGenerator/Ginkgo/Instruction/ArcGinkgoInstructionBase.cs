using Arc.Compiler.PackageGenerator.Base;
using System.IO;
using Arc.Compiler.PackageGenerator.Models.Generation;
using Arc.Compiler.PackageGenerator.Models.Relocation;

namespace Arc.Compiler.PackageGenerator.Ginkgo.Instruction;

public abstract class ArcGinkgoInstructionBase : ArcPrimitiveInstructionBase
{
    protected static byte Flags(params bool[] flags)
    {
        byte value = 0;
        for (var i = 0; i < flags.Length; i++)
            if (flags[i]) value |= (byte)(1 << i);
        return value;
    }

    public override ArcPartialGenerationResult Encode(ArcGenerationSource source)
        => Encode();

    public ArcPartialGenerationResult Encode()
    {
        using var stream = new MemoryStream();
        using var writer = new BinaryWriter(stream);
        var result = new ArcPartialGenerationResult();
        writer.Write(Opcode);
        switch (this)
        {
            case ArcArithmeticAddInstruction i:
                writer.Write(i.SrcA);
                writer.Write(i.SrcB);
                writer.Write(i.Dest);
                writer.Write(Flags(i.LastUseSrcA, i.LastUseSrcB, i.UseDestNext));
                break;
            case ArcArithmeticSubtractInstruction i:
                writer.Write(i.SrcA);
                writer.Write(i.SrcB);
                writer.Write(i.Dest);
                writer.Write(Flags(i.LastUseSrcA, i.LastUseSrcB, i.UseDestNext));
                break;
            case ArcArithmeticMultiplyInstruction i:
                writer.Write(i.SrcA);
                writer.Write(i.SrcB);
                writer.Write(i.Dest);
                writer.Write(Flags(i.LastUseSrcA, i.LastUseSrcB, i.UseDestNext));
                break;
            case ArcArithmeticDivideInstruction i:
                writer.Write(i.SrcA);
                writer.Write(i.SrcB);
                writer.Write(i.Dest);
                writer.Write(Flags(i.LastUseSrcA, i.LastUseSrcB, i.UseDestNext));
                break;
            case ArcArithmeticModuloInstruction i:
                writer.Write(i.SrcA);
                writer.Write(i.SrcB);
                writer.Write(i.Dest);
                writer.Write(Flags(i.LastUseSrcA, i.LastUseSrcB, i.UseDestNext));
                break;
            case ArcArithmeticBitwiseAndInstruction i:
                writer.Write(i.SrcA);
                writer.Write(i.SrcB);
                writer.Write(i.Dest);
                writer.Write(Flags(i.LastUseSrcA, i.LastUseSrcB, i.UseDestNext));
                break;
            case ArcArithmeticBitwiseOrInstruction i:
                writer.Write(i.SrcA);
                writer.Write(i.SrcB);
                writer.Write(i.Dest);
                writer.Write(Flags(i.LastUseSrcA, i.LastUseSrcB, i.UseDestNext));
                break;
            case ArcArithmeticBitwiseXorInstruction i:
                writer.Write(i.SrcA);
                writer.Write(i.SrcB);
                writer.Write(i.Dest);
                writer.Write(Flags(i.LastUseSrcA, i.LastUseSrcB, i.UseDestNext));
                break;
            case ArcLogicalAndInstruction i:
                writer.Write(i.SrcA);
                writer.Write(i.SrcB);
                writer.Write(i.Dest);
                writer.Write(Flags(i.LastUseSrcA, i.LastUseSrcB, i.UseDestNext));
                break;
            case ArcLogicalOrInstruction i:
                writer.Write(i.SrcA);
                writer.Write(i.SrcB);
                writer.Write(i.Dest);
                writer.Write(Flags(i.LastUseSrcA, i.LastUseSrcB, i.UseDestNext));
                break;
            case ArcLogicalXorInstruction i:
                writer.Write(i.SrcA);
                writer.Write(i.SrcB);
                writer.Write(i.Dest);
                writer.Write(Flags(i.LastUseSrcA, i.LastUseSrcB, i.UseDestNext));
                break;
            case ArcArithmeticInverseInstruction i:
                writer.Write(i.Src);
                writer.Write(i.Dest);
                writer.Write(Flags(i.LastUseSrc, i.UseDestNext));
                break;
            case ArcArithmeticBitwiseNotInstruction i:
                writer.Write(i.Src);
                writer.Write(i.Dest);
                writer.Write(Flags(i.LastUseSrc, i.UseDestNext));
                break;
            case ArcLogicalNotInstruction i:
                writer.Write(i.Src);
                writer.Write(i.Dest);
                writer.Write(Flags(i.LastUseSrc, i.UseDestNext));
                break;
            case ArcDataCopyInstruction i:
                writer.Write(i.Src);
                writer.Write(i.Dest);
                writer.Write(Flags(i.LastUseSrc, i.UseDestNext));
                break;
            case ArcDataCloneInstruction i:
                writer.Write(i.Src);
                writer.Write(i.Dest);
                writer.Write(Flags(i.LastUseSrc, i.UseDestNext));
                break;
            case ArcDataLoadReferenceInstruction i:
                writer.Write(i.Src);
                writer.Write(i.Dest);
                writer.Write(Flags(i.LastUseSrc, i.UseDestNext));
                break;
            case ArcArithmeticLeftShiftInstruction i:
                writer.Write(i.Src);
                writer.Write(i.Amount);
                writer.Write(i.Dest);
                writer.Write(Flags(i.LastUseSrc, i.LastUseAmount, i.UseDestNext));
                break;
            case ArcArithmeticRightShiftInstruction i:
                writer.Write(i.Src);
                writer.Write(i.Amount);
                writer.Write(i.Dest);
                writer.Write(Flags(i.LastUseSrc, i.LastUseAmount, i.UseDestNext));
                break;
            case ArcArithmeticRightLogicalShiftInstruction i:
                writer.Write(i.Src);
                writer.Write(i.Amount);
                writer.Write(i.Dest);
                writer.Write(Flags(i.LastUseSrc, i.LastUseAmount, i.UseDestNext));
                break;
            case ArcComparisonEqualInstruction i:
                writer.Write(i.SrcL);
                writer.Write(i.SrcR);
                writer.Write(i.Dest);
                writer.Write(Flags(i.LastUseSrcL, i.LastUseSrcR, i.UseDestNext, i.CompareReference));
                break;
            case ArcComparisonNotEqualInstruction i:
                writer.Write(i.SrcL);
                writer.Write(i.SrcR);
                writer.Write(i.Dest);
                writer.Write(Flags(i.LastUseSrcL, i.LastUseSrcR, i.UseDestNext, i.CompareReference));
                break;
            case ArcComparisonLessThanInstruction i:
                writer.Write(i.SrcL);
                writer.Write(i.SrcR);
                writer.Write(i.Dest);
                writer.Write(Flags(i.LastUseSrcL, i.LastUseSrcR, i.UseDestNext, i.CompareReference));
                break;
            case ArcComparisonLessThanOrEqualInstruction i:
                writer.Write(i.SrcL);
                writer.Write(i.SrcR);
                writer.Write(i.Dest);
                writer.Write(Flags(i.LastUseSrcL, i.LastUseSrcR, i.UseDestNext, i.CompareReference));
                break;
            case ArcComparisonGreaterThanInstruction i:
                writer.Write(i.SrcL);
                writer.Write(i.SrcR);
                writer.Write(i.Dest);
                writer.Write(Flags(i.LastUseSrcL, i.LastUseSrcR, i.UseDestNext, i.CompareReference));
                break;
            case ArcComparisonGreaterThanOrEqualInstruction i:
                writer.Write(i.SrcL);
                writer.Write(i.SrcR);
                writer.Write(i.Dest);
                writer.Write(Flags(i.LastUseSrcL, i.LastUseSrcR, i.UseDestNext, i.CompareReference));
                break;
            case ArcDataLoadConstantInstruction i:
                writer.Write(i.TableIndex);
                writer.Write(i.Dest);
                writer.Write(Flags(i.LoadAsReference, i.UsageHint));
                break;
            case ArcDataLoadImmediateIntegerInstruction i:
                writer.Write(i.Value);
                writer.Write(i.Dest);
                writer.Write(Flags(i.UsageHint));
                break;
            case ArcDataLoadImmediateFloatInstruction i:
                writer.Write(i.Value);
                writer.Write(i.Dest);
                writer.Write(Flags(i.UsageHint));
                break;
            case ArcDataInitObjectInstruction i:
                writer.Write(i.TypeId);
                writer.Write(i.Dest);
                writer.Write(i.Dimension);
                writer.Write(Flags(i.UsageHint));
                break;
            case ArcDataLoadFieldInstruction i:
                writer.Write(i.Src);
                writer.Write(i.Dest);
                writer.Write(i.FieldId);
                writer.Write(Flags(i.LastUseSrc, i.UsageHint));
                break;
            case ArcDataStoreFieldInstruction i:
                writer.Write(i.Src);
                writer.Write(i.Dest);
                writer.Write(i.FieldId);
                writer.Write(Flags(i.LastUseSrc, i.UsageHint));
                break;
            case ArcControlFlowBindArgInstruction i:
                writer.Write(i.Src);
                writer.Write(Flags(i.LastUseSrc));
                break;
            case ArcControlFlowPrepareCallInstruction i:
                writer.Write(i.FunctionSymbolId);
                break;
            case ArcControlFlowReturnWithValueInstruction i:
                writer.Write(i.Src);
                writer.Write(Flags(i.ExpectReturnValue));
                break;
            case ArcControlFlowReturnInstruction i:
                writer.Write(Flags(i.ExpectReturnValue));
                break;
            case ArcControlFlowInvokeInstruction i:
                writer.Write(i.Dest);
                writer.Write(Flags(i.IsVirtual));
                break;
            case ArcControlFlowUnconditionalJumpInstruction i:
                WriteTarget(i.Target);
                break;
            case ArcControlFlowConditionalJumpInstruction i:
                writer.Write(i.Condition);
                WriteTarget(i.Target);
                writer.Write(Flags(i.ConditionTrue, i.ConditionFalse, i.ConditionNotNone,
                    i.ConditionNone, i.LastUseCondition));
                break;
            case ArcNoOperationInstruction:
                break;
            case ArcControlFlowLabelInstruction i:
                if (i.Label is not null)
                    result.RelocationLabels.Add(new ArcRelocationLabel
                    {
                        Layer = i.Label.Layer, Type = i.Label.Type, Name = i.Label.Name, Location = 0
                    });
                break;
            default:
                throw new NotSupportedException($"No Ginkgo encoder for {GetType().Name}");
        }
        result.GeneratedData.AddRange(stream.ToArray());
        return result;

        void WriteTarget(ArcRelocationTarget target)
        {
            var copy = new ArcRelocationTarget
            {
                TargetType = target.TargetType, Layer = target.Layer,
                Parameter = target.Parameter, Location = stream.Position
            };
            switch (target.TargetType)
            {
                case ArcRelocationTargetType.Absolute: copy.TargetLocation = target.TargetLocation; break;
                case ArcRelocationTargetType.Relative: copy.Offset = target.Offset; break;
                case ArcRelocationTargetType.Label: copy.Label = target.Label; break;
                case ArcRelocationTargetType.Symbol: copy.Symbol = target.Symbol; break;
                default: throw new ArgumentOutOfRangeException(nameof(target));
            }
            result.RelocationTargets.Add(copy);
            writer.Write(0L);
        }
    }
}
