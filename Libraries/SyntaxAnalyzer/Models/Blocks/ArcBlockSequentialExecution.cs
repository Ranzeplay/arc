using Arc.Compiler.SyntaxAnalyzer.Models.Components;

namespace Arc.Compiler.SyntaxAnalyzer.Models.Blocks
{
    internal class ArcBlockSequentialExecution
    {
        public IEnumerable<ArcExecutionStepBase> ExecutionSteps { get; set; }
    }
}
