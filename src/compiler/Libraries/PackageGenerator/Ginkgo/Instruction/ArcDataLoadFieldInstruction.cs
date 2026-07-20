namespace Arc.Compiler.PackageGenerator.Ginkgo.Instruction;

public class ArcDataLoadFieldInstruction : ArcDataInstructionBase
{
    public override byte[] Opcode => [0x27];
    
    public ushort Src { get; init; }
    public ushort Dest { get; init; }
    
    public uint FieldId { get; init; }
    
    public bool LastUseSrc { get; init; }
    public bool UsageHint { get; init; }
}