namespace Arc.Compiler.PackageGenerator.Ginkgo.Instruction;

public class ArcComparisonGreaterThanInstruction : ArcComparisonInstructionBase
{
    public override byte[] Opcode => [0x12];
    
    public ushort SrcL { get; set; }
    public ushort SrcR { get; set; }
    public ushort Dest { get; set; }
    
    public bool LastUseSrcL { get; set; }
    public bool LastUseSrcR { get; set; }
    
    public bool UseDestNext { get; set; }
    
    public bool CompareReference { get; set; }
}
