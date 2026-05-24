namespace Arc.Compiler.PackageGenerator.Ginkgo.Instruction;

public class ArcControlFlowPrepareCallInstruction : ArcDataInstructionBase
{
    public override byte[] Opcode => [0x17];
    
    public uint FunctionSymbolId { get; set; }
}
