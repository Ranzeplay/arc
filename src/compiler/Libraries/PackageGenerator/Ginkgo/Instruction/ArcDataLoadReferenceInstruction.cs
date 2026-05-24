namespace Arc.Compiler.PackageGenerator.Ginkgo.Instruction;

public class ArcDataLoadReferenceInstruction : ArcDataInstructionBase
{
    public override byte[] Opcode => [0x21];
    
    public ushort Src { get; set; }
    
    public ushort Dest { get; set; }
    
    public bool LastUseSrc { get; set; }
    
    public bool UseDestNext { get; set; }
}
