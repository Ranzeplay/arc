using Arc.Compiler.SyntaxAnalyzer.Generated.ANTLR;
using Arc.Compiler.SyntaxAnalyzer.Interfaces;
using Arc.Compiler.SyntaxAnalyzer.Models.Components;
using Arc.Compiler.SyntaxAnalyzer.Models.Components.CallChain;
using Arc.Compiler.SyntaxAnalyzer.Models.Function;

namespace Arc.Compiler.SyntaxAnalyzer.Models.Statements
{
    public union ArcStatementCallValue(ArcFunctionCall, ArcCallChain);

    public class ArcStatementCall : ArcExecutionStepBase, IArcTraceable<ArcSourceCodeParser.Arc_stmt_callContext>
    {
        public ArcStatementCallValue Value { get; }

        public ArcSourceCodeParser.Arc_stmt_callContext Context { get; }

        public ArcStatementCall(ArcSourceCodeParser.Arc_stmt_callContext context)
        {
            Value = context.arc_function_call_base() is { } call
                ? new ArcFunctionCall(call)
                : new ArcCallChain(context.arc_call_chain());

            Context = context;
        }
    }
}
