using Arc.Compiler.Shared.CommandGeneration.Mappings;
using Arc.Compiler.Shared.Parsing.AST;
using Arc.CompilerCommandGenerator.Models;

namespace Arc.CompilerCommandGenerator.Builders
{
    internal class FunctionCallCommand
    {
        public static PartialGenerationResult Build(GenerationContext<FunctionCallBlock> source)
        {
            var result = new PartialGenerationResult();

            var target = source.AvailableFunctions.FirstOrDefault(f => f.Identifier.Equals(source.Component.TargetFunctionIdentifier))
                ?? throw new Exception("Function unavailable");

            // Build each parameter
            foreach (var arg in source.Component.Arguments.Reverse())
            {
                var expr = ExpressionCommand.Build(source.TransferToNewComponent(arg.EvaluateExpression));
                result.Combine(expr);
            }

            var call = Utils.CombineLeadingCommand((byte)RootCommand.Function, (byte)FunctionCommand.Enter);
            var callCommand = call.ToList();
            callCommand.AddRange(source.PackageMetadata.GenerateFunctionIdData(source.AvailableFunctions.IndexOf(target)));

            result.Commands.AddRange(callCommand);

            return result;
        }
    }
}
