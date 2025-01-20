using Arc.Compiler.SyntaxAnalyzer.Generated.ANTLR;
using Arc.Compiler.SyntaxAnalyzer.Interfaces;
using Arc.Compiler.SyntaxAnalyzer.Models.Components;
using Arc.Compiler.SyntaxAnalyzer.Models.Components.CallChain;
using Arc.Compiler.SyntaxAnalyzer.Models.Function;

namespace Arc.Compiler.SyntaxAnalyzer.Models.Statements
{
    public class ArcStatementCall : ArcExecutionStepBase, IArcTraceable<ArcSourceCodeParser.Arc_stmt_callContext>
    {
        public enum CallType
        {
            FunctionCall,
            CallChain
        }

        public CallType Type { get; set; }

        public ArcFunctionCall? FunctionCall { get; set; }

        public ArcCallChain? CallChain { get; set; }

        public ArcSourceCodeParser.Arc_stmt_callContext Context { get; }

        public ArcStatementCall(ArcSourceCodeParser.Arc_stmt_callContext context)
        {
            Type = context.arc_function_call_base() != null ? CallType.FunctionCall : CallType.CallChain;

            if (Type == CallType.FunctionCall)
            {
                FunctionCall = new ArcFunctionCall(context.arc_function_call_base());
            }
            else
            {
                CallChain = new ArcCallChain(context.arc_call_chain());
            }

            Context = context;
        }
    }
}
