using Arc.Compiler.Shared.CommandGeneration.Relocation;
using Arc.Compiler.Shared.CommandGeneration;
using Arc.CompilerCommandGenerator.Models;

namespace Arc.CompilerCommandGenerator.Extensions
{
    public static class ContextTableWriter
    {
        public static byte[] WriteFunctionTable<T>(this GenerationContext<T> context)
        {
            var metadata = context.PackageMetadata;
            var result = new List<byte>();

            var count = metadata.GenerateFunctionIdData(context.AvailableFunctions.Count);
            result.AddRange(count);

            for (var i = 0; i < context.AvailableFunctions.Count; i++)
            {
                var func = context.AvailableFunctions[i];
                var funcEntryRef = context.RelocationReferences.FirstOrDefault(r => r.ReferenceType == RelocationReferenceType.FunctionEntrance && r.Parameter == i);
                if (funcEntryRef == null)
                {
                    throw new Exception("Couldn't find the function to be written to function table");
                }

                var currentIterResult = new List<byte>();
                currentIterResult.AddRange(metadata.GenerateFunctionIdData(funcEntryRef.Parameter));
                currentIterResult.AddRange(PackageMetadata.GenerateDataAligned(funcEntryRef.CommandLocation, metadata.AddressAlignment));

                result.AddRange(currentIterResult);
            }

            return result.ToArray();
        }

        public static byte[] WriteConstantsTable<T>(this GenerationContext<T> context)
        {
            var metadata = context.PackageMetadata;
            var result = new List<byte>();

            var count = metadata.GenerateSlotData(context.ConstantBeginIndex);
            result.AddRange(count);

            foreach (var constant in context.GeneratedConstants)
            {
                var slot = metadata.GenerateSlotData(constant.Slot);
                var data = metadata.BuildDataBlock(constant.GeneratedBytes);

                result.AddRange(slot);
                result.AddRange(data);
            }

            return result.ToArray();
        }
    }
}
