namespace Arc.Compiler.PackageGenerator.Ginkgo.Instruction;

public class ArcDataLoadInstruction : ArcDataInstructionBase
{
    public override byte[] Opcode => [0x1c];
    
    public uint TableIndex { get; set; }
    
    public ushort Dest { get; set; }
    
    public bool LoadAsReference { get; set; }
    
    public bool UsageHint { get; set; }
}
