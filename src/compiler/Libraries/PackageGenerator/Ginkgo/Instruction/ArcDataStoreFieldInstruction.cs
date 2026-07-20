namespace Arc.Compiler.PackageGenerator.Ginkgo.Instruction;

public class ArcDataStoreFieldInstruction : ArcDataInstructionBase
{
    public override byte[] Opcode => [0x27];
    
    public ushort Src { get; private set; }
    public ushort Dest { get; private set; }
    
    public uint FieldId { get; private set; }
    
    public bool LastUseSrc { get; private set; }
    public bool UsageHint { get; set; }
}