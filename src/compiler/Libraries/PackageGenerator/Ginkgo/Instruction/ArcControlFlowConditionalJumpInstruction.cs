using Arc.Compiler.PackageGenerator.Models.Relocation;

namespace Arc.Compiler.PackageGenerator.Ginkgo.Instruction;

public class ArcControlFlowConditionalJumpInstruction : ArcControlFlowInstructionBase
{
    public override byte[] Opcode => [0x16];
    
    public required ArcRelocationTarget Target { get; set; }
    
    public ushort Condition { get; set; }
    
    public bool ConditionTrue { get; set; }
    
    public bool ConditionFalse { get; set; }
    
    public bool ConditionNotNone { get; set; }
    
    public bool ConditionNone  { get; set; }
    
    public bool LastUseCondition { get; set; }
}
