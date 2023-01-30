using Arc.Compiler.Shared.LexicalAnalysis;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Arc.Compiler.Shared.Parsing.AST
{
    public class ASTNode
    {
        public ASTNodeType NodeType { get; }

        private object Target { get; }

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
            if (NodeType == ASTNodeType.FunctionCall)
            {
                return Target as FunctionReturnBlock;
            }
            else
            {
                return null;
            }
        }
    }
}
