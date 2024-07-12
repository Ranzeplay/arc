using Arc.Compiler.Shared.Extensions;
using Arc.Compiler.Shared.Parsing.Components.Data;
using Arc.Compiler.Shared.Parsing.Components.Function;

namespace Arc.Compiler.Shared.CommandGeneration.Relocation
{
    public class FinalRelocationContext
    {
        public required byte[] Commands { get; set; }

        public required RelocationReference[] RelocationReferences { get; set; }

        public required RelocationTarget[] RelocationTargets { get; set; }

        public required PackageMetadata PackageMetadata { get; set; }

        public required FunctionDeclarator[] GeneratedFunctions { get; set; }

        public required GeneratedConstant[] GeneratedConstants { get; set; }

        public required DataDeclarator[] GlobalData { get; set; }

        public void ConvertRelocationTargets()
        {
            for (var i = 0; i < RelocationTargets.Length; i++)
            {
                var target = RelocationTargets[i];

                switch (target.RelocationType)
                {
                    case RelocationType.RelativeLocation:
                        var relocator = target.GetRelativeLocation()!;
                        switch (relocator.RelocatorType)
                        {
                            case RelativeRelocatorType.Address:
                                continue;
                            case RelativeRelocatorType.LoopEntry:
                                {
                                    var matches = RelocationReferences
                                        .AsQueryable()
                                        .Where(r => r.ReferenceType == RelocationReferenceType.LoopEntrance || r.ReferenceType == RelocationReferenceType.WhileEntrance)
                                        .Where(r => r.CommandLocation <= target.CommandLocation)
                                        .OrderBy(r => r.CommandLocation);
                                    var reference = matches.Last();

                                    var relativeLocation = reference.CommandLocation - target.CommandLocation;
                                    RelocationTargets[i].Relative = new RelativeRelocator(RelativeRelocatorType.Address, relativeLocation);
                                }
                                break;
                            case RelativeRelocatorType.EndCondition:
                                {
                                    var matches = RelocationReferences
                                        .AsQueryable()
                                        .Where(r => r.ReferenceType == RelocationReferenceType.EndElse || r.ReferenceType == RelocationReferenceType.EndElif || r.ReferenceType == RelocationReferenceType.EndIf)
                                        .Where(r => r.CommandLocation >= target.CommandLocation)
                                        .OrderBy(r => r.CommandLocation);
                                    var reference = matches.First();

                                    var relativeLocation = reference.CommandLocation - target.CommandLocation;
                                    RelocationTargets[i].Relative = new RelativeRelocator(RelativeRelocatorType.Address, relativeLocation);
                                }
                                break;
                            case RelativeRelocatorType.IterationEntry:
                                {
                                    var matches = RelocationReferences
                                        .AsQueryable()
                                        .Where(r => r.ReferenceType == RelocationReferenceType.WhileEntrance)
                                        .Where(r => r.CommandLocation <= target.CommandLocation)
                                        .OrderBy(r => r.CommandLocation);
                                    var reference = matches.Last();

                                    var relativeLocation = reference.CommandLocation - target.CommandLocation;
                                    RelocationTargets[i].Relative = new RelativeRelocator(RelativeRelocatorType.Address, relativeLocation);
                                }
                                break;
                            case RelativeRelocatorType.IterationEnd:
                                {
                                    var matches = RelocationReferences
                                        .AsQueryable()
                                        .Where(r => r.ReferenceType == RelocationReferenceType.EndWhile)
                                        .Where(r => r.CommandLocation >= target.CommandLocation)
                                        .OrderBy(r => r.CommandLocation);
                                    var reference = matches.First();

                                    var relativeLocation = reference.CommandLocation - target.CommandLocation;
                                    RelocationTargets[i].Relative = new RelativeRelocator(RelativeRelocatorType.Address, relativeLocation);
                                }
                                break;
                            case RelativeRelocatorType.IgnoreActionBlock:
                                {
                                    var matches = RelocationReferences
                                        .AsQueryable()
                                        .Where(r => r.ReferenceType == RelocationReferenceType.EndWhile)
                                        .Where(r => r.CommandLocation >= target.CommandLocation)
                                        .OrderBy(r => r.CommandLocation);
                                    var reference = matches.First();

                                    var relativeLocation = reference.CommandLocation - target.CommandLocation;
                                    RelocationTargets[i].Relative = new RelativeRelocator(RelativeRelocatorType.Address, relativeLocation);
                                }
                                break;
                            default:
                                throw new NotSupportedException();
                        }

                        break;
                    case RelocationType.Constant:
                        break;
                    case RelocationType.Function:
                        break;
                }
            }
        }

        public void ApplyAllRelocation()
        {
            RelocationTargets.AsQueryable()
                .Where(r => r.RelocationType == RelocationType.RelativeLocation)
                .Where(r => r.Relative!.RelocatorType == RelativeRelocatorType.Address)
                .ToList()
                .ForEach(r =>
                {
                    var placeLocation = r.CommandLocation + r.PlaceholderOffset;

                    Commands.ReplaceRange(PackageMetadata.GenerateJumpAddress(r.Relative!.Parameter), (int)placeLocation);
                });
        }

        public byte[] WriteFunctionTable()
        {
            var result = new List<byte>();

            var count = PackageMetadata.GenerateFunctionIdData(GeneratedFunctions.Length);
            result.AddRange(count);

            for (var i = 0; i < GeneratedFunctions.Length; i++)
            {
                var func = GeneratedFunctions[i];
                var funcEntryRef = RelocationReferences.FirstOrDefault(r => r.ReferenceType == RelocationReferenceType.FunctionEntrance && r.Parameter == i)
                    ?? throw new Exception("Couldn't find the function to be written to function table");

                var currentIterResult = new List<byte>();
                currentIterResult.AddRange(PackageMetadata.GenerateFunctionIdData(funcEntryRef.Parameter));
                currentIterResult.AddRange(PackageMetadata.GenerateDataAligned(funcEntryRef.CommandLocation, PackageMetadata.AddressAlignment));

                result.AddRange(currentIterResult);
            }

            return result.ToArray();
        }

        public byte[] WriteConstantsTable()
        {
            var result = new List<byte>();

            var count = PackageMetadata.GenerateSlotData(GeneratedConstants.Length);
            result.AddRange(count);

            foreach (var constant in GeneratedConstants)
            {
                var slot = PackageMetadata.GenerateSlotData(constant.Slot);
                var data = PackageMetadata.BuildDataBlock(constant.GeneratedBytes);

                result.AddRange(slot);
                result.AddRange(data);
            }

            return result.ToArray();
        }
    }
}
