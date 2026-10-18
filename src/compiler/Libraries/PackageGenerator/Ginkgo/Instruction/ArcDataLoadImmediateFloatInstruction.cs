namespace Arc.Compiler.PackageGenerator.Ginkgo.Instruction;

public class ArcDataLoadImmediateFloatInstruction : ArcDataInstructionBase
{
    public override byte[] Opcode => [0x1e];
    public double Value { get; set; }
    public ushort Dest { get; set; }
    public bool UsageHint { get; set; }
}
