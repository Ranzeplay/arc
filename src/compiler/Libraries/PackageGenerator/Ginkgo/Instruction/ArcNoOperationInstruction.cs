namespace Arc.Compiler.PackageGenerator.Ginkgo.Instruction;

public class ArcNoOperationInstruction : ArcGinkgoInstructionBase
{
    public override byte[] Opcode => [0x00];
}
