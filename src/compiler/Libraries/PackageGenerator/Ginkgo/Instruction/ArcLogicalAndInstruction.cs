namespace Arc.Compiler.PackageGenerator.Ginkgo.Instruction;

public class ArcLogicalAndInstruction : ArcLogicalInstructionBase
{
    public override byte[] Opcode => [0x23];
    
    public ushort SrcA { get; set; }
    public ushort SrcB { get; set; }
    public ushort Dest { get; set; }
    
    public bool LastUseSrcA { get; set; }
    public bool LastUseSrcB { get; set; }
    
    public bool UseDestNext { get; set; }
}
