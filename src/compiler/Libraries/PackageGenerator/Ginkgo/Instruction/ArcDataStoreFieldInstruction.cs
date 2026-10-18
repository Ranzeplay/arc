namespace Arc.Compiler.PackageGenerator.Ginkgo.Instruction;

public class ArcDataStoreFieldInstruction : ArcDataInstructionBase
{
    public override byte[] Opcode => [0x28];
    
    public ushort Src { get; set; }
    public ushort Dest { get; set; }
    
    public uint FieldId { get; set; }
    
    public bool LastUseSrc { get; set; }
    public bool UsageHint { get; set; }
}
