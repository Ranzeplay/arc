using System.Buffers.Binary;
using Arc.Compiler.PackageGenerator.Ginkgo.Instruction;
using Arc.Compiler.PackageGenerator.Ginkgo.Transformation;
using Arc.Compiler.PackageGenerator.Models.Relocation;

namespace Arc.Compiler.Tests.PackageGeneration;

public class GinkgoEncoding
{
    [Test]
    public void EveryDocumentedOpcodeHasAnEncoderAndTheExpectedWidth()
    {
        var target = new ArcRelocationTarget
            { Layer = Guid.NewGuid(), TargetType = ArcRelocationTargetType.Absolute, TargetLocation = 0 };
        ArcGinkgoInstructionBase[] instructions =
        [
            new ArcNoOperationInstruction(),
            new ArcArithmeticAddInstruction(),
            new ArcArithmeticSubtractInstruction(),
            new ArcArithmeticMultiplyInstruction(),
            new ArcArithmeticDivideInstruction(),
            new ArcArithmeticModuloInstruction(),
            new ArcArithmeticInverseInstruction(),
            new ArcArithmeticLeftShiftInstruction(),
            new ArcArithmeticRightShiftInstruction(),
            new ArcArithmeticRightLogicalShiftInstruction(),
            new ArcArithmeticBitwiseAndInstruction(),
            new ArcArithmeticBitwiseOrInstruction(),
            new ArcArithmeticBitwiseXorInstruction(),
            new ArcArithmeticBitwiseNotInstruction(),
            new ArcComparisonEqualInstruction(),
            new ArcComparisonNotEqualInstruction(),
            new ArcComparisonLessThanInstruction(),
            new ArcComparisonLessThanOrEqualInstruction(),
            new ArcComparisonGreaterThanInstruction(),
            new ArcComparisonGreaterThanOrEqualInstruction(),
            new ArcControlFlowLabelInstruction(),
            new ArcControlFlowUnconditionalJumpInstruction { Target = target },
            new ArcControlFlowConditionalJumpInstruction { Target = target },
            new ArcControlFlowPrepareCallInstruction(0),
            new ArcControlFlowBindArgInstruction(),
            ArcControlFlowInvokeInstruction.Normal,
            new ArcControlFlowReturnWithValueInstruction(),
            new ArcControlFlowReturnInstruction(),
            new ArcDataLoadConstantInstruction(),
            new ArcDataLoadImmediateIntegerInstruction(),
            new ArcDataLoadImmediateFloatInstruction(),
            new ArcDataCopyInstruction(),
            new ArcDataCloneInstruction(),
            new ArcDataLoadReferenceInstruction(),
            new ArcDataInitObjectInstruction(),
            new ArcLogicalAndInstruction(),
            new ArcLogicalOrInstruction(),
            new ArcLogicalXorInstruction(),
            new ArcLogicalNotInstruction(),
            new ArcDataLoadFieldInstruction(),
            new ArcDataStoreFieldInstruction(),
        ];
        int[] widths = [1, 8, 8, 8, 8, 8, 6, 8, 8, 8, 8, 8, 8, 6, 8, 8, 8, 8, 8, 8, 1, 9, 12, 5, 4, 4, 4, 2, 8, 12, 12, 6, 6, 6, 10, 8, 8, 8, 6, 10, 10];
        for (var opcode = 0; opcode < instructions.Length; opcode++)
        {
            var bytes = instructions[opcode].Encode().GeneratedData;
            Assert.Multiple(() =>
            {
                Assert.That(bytes[0], Is.EqualTo(opcode), instructions[opcode].GetType().Name);
                Assert.That(bytes.Count, Is.EqualTo(widths[opcode]), instructions[opcode].GetType().Name);
            });
        }
    }
    [TestCase(false, (byte)0)]
    [TestCase(true, (byte)1)]
    public void ReturnsEncodeExpectReturnValueInBitZero(bool expectReturnValue, byte flags)
    {
        Assert.That(new ArcControlFlowReturnWithValueInstruction
            { Src = 0x1234, ExpectReturnValue = expectReturnValue }.Encode().GeneratedData,
            Is.EqualTo(new byte[] { 0x1a, 0x34, 0x12, flags }));
        Assert.That(new ArcControlFlowReturnInstruction
            { ExpectReturnValue = expectReturnValue }.Encode().GeneratedData,
            Is.EqualTo(new byte[] { 0x1b, flags }));
    }

    [Test]
    public void ArithmeticHasLittleEndianOperandsAndIndependentFlags()
    {
        var instruction = new ArcArithmeticSubtractInstruction
        {
            SrcA = 0x1234, SrcB = 0xabcd, Dest = 0x5678,
            LastUseSrcA = true, UseDestNext = true
        };
        Assert.That(instruction.Encode().GeneratedData,
            Is.EqualTo(new byte[] { 0x02, 0x34, 0x12, 0xcd, 0xab, 0x78, 0x56, 0x05 }));
    }

    [Test]
    public void ImmediateAndFieldOperandsKeepTheirFullWidth()
    {
        Assert.That(new ArcDataLoadImmediateIntegerInstruction { Value = -2, Dest = 0x1234 }.Encode().GeneratedData,
            Is.EqualTo(new byte[] { 0x1d, 0xfe, 0xff, 0xff, 0xff, 0xff, 0xff, 0xff, 0xff, 0x34, 0x12, 0 }));
        Assert.That(new ArcDataStoreFieldInstruction
            { Src = 1, Dest = 2, FieldId = 0x12345678, LastUseSrc = true }.Encode().GeneratedData,
            Is.EqualTo(new byte[] { 0x28, 1, 0, 2, 0, 0x78, 0x56, 0x34, 0x12, 1 }));
        Assert.That(new ArcDataLoadImmediateFloatInstruction { Value = 1.5, Dest = 2 }.Encode().GeneratedData,
            Is.EqualTo(new byte[] { 0x1e, 0, 0, 0, 0, 0, 0, 0xf8, 0x3f, 2, 0, 0 }));
    }

    [Test]
    public void ForwardAndBackwardBranchesAreRepeatableWithoutMutatingTargets()
    {
        var label = ArcRelocationLabel.Create();
        var target = ArcRelocationTarget.ForLabel(label);
        var input = new ArcGinkgoInstructionGenerationResult
        {
            Instructions =
            [
                new ArcControlFlowUnconditionalJumpInstruction { Target = target },
                new ArcControlFlowLabelInstruction { Label = label },
                new ArcControlFlowUnconditionalJumpInstruction { Target = target }
            ]
        };
        var first = ArcGinkgoAssembler.Assemble(input);
        var second = ArcGinkgoAssembler.Assemble(input);
        Assert.Multiple(() =>
        {
            Assert.That(first.DiscardResult, Is.False);
            Assert.That(first.GeneratedData, Is.EqualTo(second.GeneratedData));
            Assert.That(BinaryPrimitives.ReadInt64LittleEndian(first.GeneratedData.ToArray().AsSpan(1)), Is.EqualTo(9));
            Assert.That(BinaryPrimitives.ReadInt64LittleEndian(first.GeneratedData.ToArray().AsSpan(11)), Is.EqualTo(9));
            Assert.That(target.Location, Is.Zero);
        });
    }

    [Test]
    public void MissingLabelsAndMidInstructionTargetsAreRejected()
    {
        var missing = new ArcGinkgoInstructionGenerationResult
        {
            Instructions = [new ArcControlFlowUnconditionalJumpInstruction
            {
                Target = new ArcRelocationTarget { Layer = Guid.NewGuid(),
                    TargetType = ArcRelocationTargetType.Label, Label = ArcRelocationLabelType.BeginBlock }
            }]
        };
        var missingResult = ArcGinkgoAssembler.Assemble(missing);
        Assert.That(missingResult.DiscardResult, Is.True);
        Assert.That(missingResult.GeneratedData, Is.Empty);
        var invalid = new ArcGinkgoInstructionGenerationResult
        {
            Instructions = [new ArcControlFlowUnconditionalJumpInstruction
            {
                Target = new ArcRelocationTarget { Layer = Guid.NewGuid(),
                    TargetType = ArcRelocationTargetType.Absolute, TargetLocation = 1 }
            }]
        };
        var invalidResult = ArcGinkgoAssembler.Assemble(invalid);
        Assert.That(invalidResult.DiscardResult, Is.True);
        Assert.That(invalidResult.GeneratedData, Is.Empty);
    }

    [Test]
    public void InvokeInstancesDoNotShareMutableFlags()
    {
        var normal = ArcControlFlowInvokeInstruction.Normal;
        normal.IsVirtual = true;
        normal.Dest = 0x1234;
        Assert.That(normal.Encode().GeneratedData, Is.EqualTo(new byte[] { 0x19, 0x34, 0x12, 1 }));
        Assert.That(ArcControlFlowInvokeInstruction.Normal.Encode().GeneratedData,
            Is.EqualTo(new byte[] { 0x19, 0, 0, 0 }));
    }
}
