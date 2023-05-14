using Arc.Compiler.Shared.CommandGeneration;
using Arc.Compiler.Shared.CommandGeneration.Relocation;
using Arc.CompilerCommandGenerator.Models;

namespace Arc.CompilerCommandGenerator.Managers
{
    internal class RelocationManager
    {
        public static void ApplyRelocation(ref PartialGenerationResult unrelocatedCode, PackageMetadata metadata)
        {
            ApplyAddressRelocation(ref unrelocatedCode, metadata);
            ApplyConstantRelocation(ref unrelocatedCode, metadata);
        }

        internal static void ApplyAddressRelocation(ref PartialGenerationResult unrelocatedCode, PackageMetadata metadata)
        {
            var addressRelocators = unrelocatedCode.RelocationTargets.Where(r => r.RelocationType == RelocationType.RelativeLocation);
            foreach (var addressRelocator in addressRelocators)
            {
                var relativeRelocator = addressRelocator.GetRelativeLocation()!;
                if (relativeRelocator.RelocatorType == RelativeRelocatorType.Address) {
                    var addrBytes = Utils.GenerateDataAligned(relativeRelocator.Parameter, metadata.AddressAlignment);

                    unrelocatedCode.Commands[(int)addressRelocator.CommandLocation] = (byte)(relativeRelocator.Parameter >= 0 ? 0x00 : 0xff);
                    unrelocatedCode.Commands.ReplaceRange(addrBytes, (int)addressRelocator.CommandLocation + 1);
                }
                
            }
            unrelocatedCode.RelocationTargets.RemoveAll(r => r.RelocationType == RelocationType.RelativeLocation);
        }

        internal static void ApplyConstantRelocation(ref PartialGenerationResult unrelocatedCode, PackageMetadata metadata)
        {
            var addressRelocators = unrelocatedCode.RelocationTargets.Where(r => r.RelocationType == RelocationType.Constant);
            foreach (var addressRelocator in addressRelocators)
            {
                var constantBytes = Utils.GenerateDataAligned(addressRelocator.ConstantId, metadata.DataSlotAlignment);

                unrelocatedCode.Commands.ReplaceRange(constantBytes, (int)addressRelocator.CommandLocation);
            }
            unrelocatedCode.RelocationTargets.RemoveAll(r => r.RelocationType == RelocationType.Constant);
        }
    }
}
