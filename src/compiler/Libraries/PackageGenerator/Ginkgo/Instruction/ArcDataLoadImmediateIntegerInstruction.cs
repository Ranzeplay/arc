namespace Arc.Compiler.PackageGenerator.Ginkgo.Instruction;

public class ArcDataLoadImmediateIntegerInstruction : ArcDataInstructionBase
{
    public override byte[] Opcode => [0x1d];
    
    public long Value { get; set; }
    
    public ushort Dest { get; set; }
    
    public bool UsageHint { get; set; }
}
