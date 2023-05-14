namespace Arc.Compiler.Shared.Parsing.AST
{
    public class ASTNode
    {
        public ASTNodeType NodeType { get; }

        private object? Target { get; }

        public ASTNode(ASTNodeType nodeType)
        {
            NodeType = nodeType;
            Target = null;
        }

        public ASTNode(DataDeclarationBlock declarationBlock)
        {
            NodeType = ASTNodeType.DataDeclaration;
            Target = declarationBlock;
        }

        public DataDeclarationBlock? GetDeclarationBlock()
        {
            if (NodeType == ASTNodeType.DataDeclaration)
            {
                return Target as DataDeclarationBlock;
            }
            else
            {
                return null;
            }
        }

        public ASTNode(DataAssignmentBlock assignmentBlock)
        {
            NodeType = ASTNodeType.DataAssignment;
            Target = assignmentBlock;
        }

        public DataAssignmentBlock? GetAssignmentBlock()
        {
            if (NodeType == ASTNodeType.DataAssignment)
            {
                return Target as DataAssignmentBlock;
            }
            else
            {
                return null;
            }
        }

        public ASTNode(FunctionCallBlock functionCallBlock)
        {
            NodeType = ASTNodeType.FunctionCall;
            Target = functionCallBlock;
        }

        public FunctionCallBlock? GetFunctionCallBlock()
        {
            if (NodeType == ASTNodeType.FunctionCall)
            {
                return Target as FunctionCallBlock;
            }
            else
            {
                return null;
            }
        }

        public ASTNode(FunctionReturnBlock functionReturnBlock)
        {
            NodeType = ASTNodeType.FunctionReturn;
            Target = functionReturnBlock;
        }

        public FunctionReturnBlock? GetFunctionReturnBlock()
        {
            if (NodeType == ASTNodeType.ConditionalLoop)
            {
                return Target as FunctionReturnBlock;
            }
            else
            {
                return null;
            }
        }

        public ASTNode(ConditionalLoopBlock conditionalLoopBlock)
        {
            NodeType = ASTNodeType.ConditionalLoop;
            Target = conditionalLoopBlock;
        }

        public ConditionalLoopBlock? GetConditionalLoopBlock()
        {
            if (NodeType == ASTNodeType.ConditionalLoop)
            {
                return Target as ConditionalLoopBlock;
            }
            else
            {
                return null;
            }
        }

        public ASTNode(ConditionalExecBlock conditionalExecBlock)
        {
            NodeType = ASTNodeType.ConditionalExec;
            Target = conditionalExecBlock;
        }

        public ConditionalExecBlock? GetConditionalExecBlock()
        {
            if (NodeType == ASTNodeType.ConditionalLoop)
            {
                return Target as ConditionalExecBlock;
            }
            else
            {
                return null;
            }
        }

        public ASTNode(LoopBlock loopBlock)
        {
            NodeType = ASTNodeType.ConditionalExec;
            Target = loopBlock;
        }

        public LoopBlock? GetLoopBlock()
        {
            if (NodeType == ASTNodeType.UnconditionalLoop)
            {
                return Target as LoopBlock;
            }
            else
            {
                return null;
            }
        }
    }
}
