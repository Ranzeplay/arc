using Arc.Compiler.PackageGenerator.Models.Builtin;
using Arc.Compiler.PackageGenerator.Models.Descriptors;
using Arc.Compiler.PackageGenerator.Models.Generation;
using Arc.Compiler.PackageGenerator.Models.Intermediate;
using Arc.Compiler.PackageGenerator.Models.PrimitiveInstructions;
using Arc.Compiler.PackageGenerator.Models.Scope;
using Arc.Compiler.SyntaxAnalyzer.Models.Components.CallChain;

namespace Arc.Compiler.PackageGenerator.Generators
{
    internal class ArcCallChainGenerator
    {
        public static ArcPartialGenerationResult Generate(ArcGenerationSource source, ArcCallChain callChain)
        {
            var result = new ArcPartialGenerationResult();
            ArcDataDeclarationDescriptor lastTermTypeDecl = null!;
            var locator = new ArcDataLocator(ArcDataSourceType.Invalid, -1, [], []);

            // First term maybe be variant, so handle it separately
            if (callChain.Terms.First().Type == ArcCallChainTermType.Identifier)
            {
                locator.Source = ArcDataSourceType.DataSlot;

                var identifier = callChain.Terms.First().Identifier!;
                var slot = source.LocalDataSlots.First(s => s.Name == identifier.Name);
                locator.LocationId = slot.SlotId;
            }
            else
            {
                var call = callChain.Terms.First().FunctionCall!;
                result.Append(ArcFunctionCallGenerator.Generate(source, call));

                var targetFunctonId = Utils.GetFunctionId(source, call);
                var function = source.CurrentNode.Root.GetSpecificChild<ArcScopeTreeFunctionNodeBase>(f => f.Id == targetFunctonId, true);
                lastTermTypeDecl = function.Descriptor.ReturnValueType;
            }

            foreach (var call in callChain.Terms.Skip(1))
            {
                if (lastTermTypeDecl.Type is ArcBaseType)
                {
                    throw new InvalidDataException("Cannot call a primitive data type");
                }

                var dataType = (lastTermTypeDecl.Type as ArcDerivativeType)!;
                var group = source.CurrentNode.Root.GetSpecificChild<ArcScopeTreeGroupNode>(g => g.Id == dataType.Id, true)!;

                if (call.Type == ArcCallChainTermType.FunctionCall)
                {
                    // Batch select fields
                    if (locator.FieldChain.Count > 0)
                    {
                        result.Append(new ArcLoadDataToStackInstruction(locator).Encode(source));
                    }

                    // Handle the function call of this term
                    result.Append(ArcFunctionCallGenerator.Generate(source, call.FunctionCall!));

                    // Reset locator
                    locator = new ArcDataLocator(ArcDataSourceType.StackTop, 0, [], []);
                }
                else if (call.Type == ArcCallChainTermType.Identifier)
                {
                    var field = group.Descriptor.Fields.First(f => f.Name == call.Identifier!.Name);
                    locator.FieldChain.Add(field);
                    lastTermTypeDecl = field.DataType;
                }
            }

            // Process the remaining fields
            if (locator.FieldChain.Count > 0)
            {
                result.Append(new ArcLoadDataToStackInstruction(locator).Encode(source));
            }

            return result;
        }
    }
}
