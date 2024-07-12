using Arc.Compiler.CommandGenerator.Models;
using Arc.Compiler.Shared.CommandGeneration.Relocation;
using Arc.Compiler.Shared.Parsing.AST;
using Arc.Compiler.Shared.Parsing.Components.Data;

namespace Arc.Compiler.CommandGenerator.Builders
{
    public class FunctionCommandGroup
    {
        public static PartialGenerationResult Build(GenerationContext<FunctionBlock> source)
        {
            var dataDeclaractors = new List<DataDeclarator>();
            foreach (var param in source.Component.Declarator.Parameters)
            {
                dataDeclaractors.Add(new DataDeclarator(param.DataType, param.Identifier, param.IsConstant));
            }

            var context = new GenerationContext<ActionBlock>(source.Component.Actions,
                dataDeclaractors,
                source.GlobalData,
                source.AvailableFunctions,
                source.GeneratedConstants, new(), source.PackageMetadata);

            var actionBlockResult = ActionBlockCommand.Build(context);

            var functionId = source.AvailableFunctions.FindIndex(f => f.Identifier.Equals(source.Component.Declarator.Identifier));
            actionBlockResult.RelocationReferences.Insert(0, new(0, RelocationReferenceType.FunctionEntrance, functionId));
            actionBlockResult.RelocationReferences.Add(new(actionBlockResult.Commands.Count, RelocationReferenceType.EndFunction, functionId));

            return actionBlockResult;
        }
    }
}
