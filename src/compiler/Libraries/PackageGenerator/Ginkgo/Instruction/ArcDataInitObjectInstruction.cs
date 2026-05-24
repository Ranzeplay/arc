namespace Arc.Compiler.PackageGenerator.Ginkgo.Instruction;

public class ArcDataInitObjectInstruction : ArcDataInstructionBase
{
    public override byte[] Opcode => [0x1c];
    
    public uint TypeId { get; set; }
    
    public ushort Dest { get; set; }
    
    public ushort Dimension { get; set; }
    
    public bool UsageHint { get; set; }
}
