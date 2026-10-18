namespace Arc.Compiler.PackageGenerator.Ginkgo.Instruction;

public class ArcControlFlowPrepareCallInstruction(uint id) : ArcControlFlowInstructionBase
{
    public override byte[] Opcode => [0x17];

    public uint FunctionSymbolId { get; } = id;
}
