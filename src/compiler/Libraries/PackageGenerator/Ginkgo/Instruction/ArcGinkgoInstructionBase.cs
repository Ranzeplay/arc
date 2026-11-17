using System.Buffers.Binary;
using Arc.Compiler.PackageGenerator.Base;
using Arc.Compiler.PackageGenerator.Models.Generation;
using Arc.Compiler.PackageGenerator.Models.Relocation;

namespace Arc.Compiler.PackageGenerator.Ginkgo.Instruction;

public closed class ArcGinkgoInstructionBase : ArcPrimitiveInstructionBase
{
    protected static byte Flags(params bool[] flags)
    {
        byte value = 0;
        for (var i = 0; i < flags.Length; i++)
            if (flags[i]) value |= (byte)(1 << i);
        return value;
    }

    private static byte[] LittleEndian(ushort value)
    {
        var bytes = new byte[sizeof(ushort)];
        BinaryPrimitives.WriteUInt16LittleEndian(bytes, value);
        return bytes;
    }

    private static byte[] LittleEndian(uint value)
    {
        var bytes = new byte[sizeof(uint)];
        BinaryPrimitives.WriteUInt32LittleEndian(bytes, value);
        return bytes;
    }

    public override ArcPartialGenerationResult Encode(ArcGenerationSource source)
        => Encode();

    public ArcPartialGenerationResult Encode()
    {
        var result = new ArcPartialGenerationResult();
        byte[] operands = this switch
        {
            ArcArithmeticAddInstruction i => [.. LittleEndian(i.SrcA), .. LittleEndian(i.SrcB), .. LittleEndian(i.Dest), Flags(i.LastUseSrcA, i.LastUseSrcB, i.UseDestNext)],
            ArcArithmeticSubtractInstruction i => [.. LittleEndian(i.SrcA), .. LittleEndian(i.SrcB), .. LittleEndian(i.Dest), Flags(i.LastUseSrcA, i.LastUseSrcB, i.UseDestNext)],
            ArcArithmeticMultiplyInstruction i => [.. LittleEndian(i.SrcA), .. LittleEndian(i.SrcB), .. LittleEndian(i.Dest), Flags(i.LastUseSrcA, i.LastUseSrcB, i.UseDestNext)],
            ArcArithmeticDivideInstruction i => [.. LittleEndian(i.SrcA), .. LittleEndian(i.SrcB), .. LittleEndian(i.Dest), Flags(i.LastUseSrcA, i.LastUseSrcB, i.UseDestNext)],
            ArcArithmeticModuloInstruction i => [.. LittleEndian(i.SrcA), .. LittleEndian(i.SrcB), .. LittleEndian(i.Dest), Flags(i.LastUseSrcA, i.LastUseSrcB, i.UseDestNext)],
            ArcArithmeticBitwiseAndInstruction i => [.. LittleEndian(i.SrcA), .. LittleEndian(i.SrcB), .. LittleEndian(i.Dest), Flags(i.LastUseSrcA, i.LastUseSrcB, i.UseDestNext)],
            ArcArithmeticBitwiseOrInstruction i => [.. LittleEndian(i.SrcA), .. LittleEndian(i.SrcB), .. LittleEndian(i.Dest), Flags(i.LastUseSrcA, i.LastUseSrcB, i.UseDestNext)],
            ArcArithmeticBitwiseXorInstruction i => [.. LittleEndian(i.SrcA), .. LittleEndian(i.SrcB), .. LittleEndian(i.Dest), Flags(i.LastUseSrcA, i.LastUseSrcB, i.UseDestNext)],
            ArcLogicalAndInstruction i => [.. LittleEndian(i.SrcA), .. LittleEndian(i.SrcB), .. LittleEndian(i.Dest), Flags(i.LastUseSrcA, i.LastUseSrcB, i.UseDestNext)],
            ArcLogicalOrInstruction i => [.. LittleEndian(i.SrcA), .. LittleEndian(i.SrcB), .. LittleEndian(i.Dest), Flags(i.LastUseSrcA, i.LastUseSrcB, i.UseDestNext)],
            ArcLogicalXorInstruction i => [.. LittleEndian(i.SrcA), .. LittleEndian(i.SrcB), .. LittleEndian(i.Dest), Flags(i.LastUseSrcA, i.LastUseSrcB, i.UseDestNext)],
            ArcArithmeticInverseInstruction i => [.. LittleEndian(i.Src), .. LittleEndian(i.Dest), Flags(i.LastUseSrc, i.UseDestNext)],
            ArcArithmeticBitwiseNotInstruction i => [.. LittleEndian(i.Src), .. LittleEndian(i.Dest), Flags(i.LastUseSrc, i.UseDestNext)],
            ArcLogicalNotInstruction i => [.. LittleEndian(i.Src), .. LittleEndian(i.Dest), Flags(i.LastUseSrc, i.UseDestNext)],
            ArcDataCopyInstruction i => [.. LittleEndian(i.Src), .. LittleEndian(i.Dest), Flags(i.LastUseSrc, i.UseDestNext)],
            ArcDataCloneInstruction i => [.. LittleEndian(i.Src), .. LittleEndian(i.Dest), Flags(i.LastUseSrc, i.UseDestNext)],
            ArcDataLoadReferenceInstruction i => [.. LittleEndian(i.Src), .. LittleEndian(i.Dest), Flags(i.LastUseSrc, i.UseDestNext)],
            ArcArithmeticLeftShiftInstruction i => [.. LittleEndian(i.Src), .. LittleEndian(i.Amount), .. LittleEndian(i.Dest), Flags(i.LastUseSrc, i.LastUseAmount, i.UseDestNext)],
            ArcArithmeticRightShiftInstruction i => [.. LittleEndian(i.Src), .. LittleEndian(i.Amount), .. LittleEndian(i.Dest), Flags(i.LastUseSrc, i.LastUseAmount, i.UseDestNext)],
            ArcArithmeticRightLogicalShiftInstruction i => [.. LittleEndian(i.Src), .. LittleEndian(i.Amount), .. LittleEndian(i.Dest), Flags(i.LastUseSrc, i.LastUseAmount, i.UseDestNext)],
            ArcComparisonEqualInstruction i => [
                .. LittleEndian(i.SrcL),
                .. LittleEndian(i.SrcR),
                .. LittleEndian(i.Dest), Flags(i.LastUseSrcL, i.LastUseSrcR, i.UseDestNext, i.CompareReference)],
            ArcComparisonNotEqualInstruction i => [
                .. LittleEndian(i.SrcL),
                .. LittleEndian(i.SrcR),
                .. LittleEndian(i.Dest), Flags(i.LastUseSrcL, i.LastUseSrcR, i.UseDestNext, i.CompareReference)],
            ArcComparisonLessThanInstruction i => [
                .. LittleEndian(i.SrcL),
                .. LittleEndian(i.SrcR),
                .. LittleEndian(i.Dest), Flags(i.LastUseSrcL, i.LastUseSrcR, i.UseDestNext, i.CompareReference)],
            ArcComparisonLessThanOrEqualInstruction i => [
                .. LittleEndian(i.SrcL),
                .. LittleEndian(i.SrcR),
                .. LittleEndian(i.Dest), Flags(i.LastUseSrcL, i.LastUseSrcR, i.UseDestNext, i.CompareReference)],
            ArcComparisonGreaterThanInstruction i => [
                .. LittleEndian(i.SrcL),
                .. LittleEndian(i.SrcR),
                .. LittleEndian(i.Dest), Flags(i.LastUseSrcL, i.LastUseSrcR, i.UseDestNext, i.CompareReference)],
            ArcComparisonGreaterThanOrEqualInstruction i => [
                .. LittleEndian(i.SrcL),
                .. LittleEndian(i.SrcR),
                .. LittleEndian(i.Dest), Flags(i.LastUseSrcL, i.LastUseSrcR, i.UseDestNext, i.CompareReference)],
            ArcDataLoadInstruction i => [.. LittleEndian(i.TableIndex), .. LittleEndian(i.Dest), Flags(i.LoadAsReference, i.UsageHint)],
            ArcDataInitObjectInstruction i => [.. LittleEndian(i.TypeId), .. LittleEndian(i.Dest), .. LittleEndian(i.Dimension), Flags(i.UsageHint)],
            ArcDataLoadFieldInstruction i => [.. LittleEndian(i.Src), .. LittleEndian(i.Dest), .. LittleEndian(i.FieldId), Flags(i.LastUseSrc, i.UsageHint)],
            ArcDataStoreFieldInstruction i => [.. LittleEndian(i.Src), .. LittleEndian(i.Dest), .. LittleEndian(i.FieldId), Flags(i.LastUseSrc, i.UsageHint)],
            ArcControlFlowBindArgInstruction i => [.. LittleEndian(i.Src), Flags(i.LastUseSrc)],
            ArcControlFlowPrepareCallInstruction i => [.. LittleEndian(i.FunctionSymbolId)],
            ArcControlFlowReturnWithValueInstruction i => [.. LittleEndian(i.Src), Flags(i.ExpectReturnValue)],
            ArcControlFlowReturnInstruction i => [Flags(i.ExpectReturnValue)],
            ArcControlFlowInvokeInstruction i => [.. LittleEndian(i.Dest), Flags(i.IsVirtual)],
            ArcControlFlowUnconditionalJumpInstruction i => [.. EncodeTarget(i.Target, 0)],
            ArcControlFlowConditionalJumpInstruction i => [
                .. LittleEndian(i.Condition),
                .. EncodeTarget(i.Target, sizeof(ushort)), Flags(i.ConditionTrue, i.ConditionFalse, i.ConditionNotNone, i.ConditionNone, i.LastUseCondition)],
            ArcNoOperationInstruction => [],
            ArcControlFlowLabelInstruction i => EncodeLabel(i),
        };
        result.GeneratedData.AddRange(Opcode);
        result.GeneratedData.AddRange(operands);
        return result;

        byte[] EncodeLabel(ArcControlFlowLabelInstruction instruction)
        {
            if (instruction.Label is { } label)
                result.RelocationLabels.Add(new ArcRelocationLabel
                {
                    Layer = label.Layer, Type = label.Type, Name = label.Name, Location = 0
                });
            return [];
        }

        byte[] EncodeTarget(ArcRelocationTarget target, int operandOffset)
        {
            var copy = new ArcRelocationTarget
            {
                TargetType = target.TargetType, Layer = target.Layer,
                Parameter = target.Parameter, Location = Opcode.Length + operandOffset
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
            return new byte[sizeof(long)];
        }
    }
}
