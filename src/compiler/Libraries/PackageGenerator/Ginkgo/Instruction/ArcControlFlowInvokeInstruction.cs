namespace Arc.Compiler.PackageGenerator.Ginkgo.Instruction;

public class ArcControlFlowInvokeInstruction : ArcControlFlowInstructionBase
{
    public override byte[] Opcode => [0x19];
    
    public bool IsVirtual { get; set; }

    private ArcControlFlowInvokeInstruction(bool isVirtual)
    {
        IsVirtual = isVirtual;
    }

    public static ArcControlFlowInvokeInstruction Normal = new(false);
    public static ArcControlFlowInvokeInstruction Virtual = new(true);
}
