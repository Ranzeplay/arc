using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Arc.Compiler.Shared.Parsing.AST
{
    public enum ASTNodeType
    {
        Invalid,
        DataAssignment,
        DataDeclaration,
        FunctionCall,
        FunctionReturn,
        ConditionalLoop,
        ConditionalExec,
        UnconditionalLoop,
        LoopContinue,
        LoopBreak
    }
}
