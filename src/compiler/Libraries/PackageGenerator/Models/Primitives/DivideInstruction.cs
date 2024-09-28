using Arc.Compiler.PackageGenerator.Interfaces;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Arc.Compiler.PackageGenerator.Models.Primitives
{
    internal class DivideInstruction : ArcPrimitiveInstructionBase
    {
        public override byte[] Opcode => [0x0a];
    }
}
