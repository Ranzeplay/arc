using Arc.Compiler.Shared.CommandGeneration;
using Arc.Compiler.Shared.CommandGeneration.Relocation;
using Arc.Compiler.Shared.Parsing.Components.Data;

namespace Arc.CompilerCommandGenerator.Models
{
    public class PartialGenerationResult
    {
        public List<byte> Commands { get; set; }

        public List<DataDeclarator> DataDeclarators { get; set; }

        public List<GeneratedConstant> GeneratedConstants { get; set; }

        public List<RelocationTarget> RelocationTargets { get; set; }

        public List<RelocationReference> RelocationReferences { get; set; }

        public PartialGenerationResult(List<byte> commands, IEnumerable<DataDeclarator>? dataDeclarators = null, IEnumerable<GeneratedConstant>? generatedConstants = null, IEnumerable<RelocationTarget>? relocationDescriptors = null, IEnumerable<RelocationReference>? relocationReferences = null)
        {
            Commands = commands;
            DataDeclarators = dataDeclarators == null ? new() : dataDeclarators.ToList();
            GeneratedConstants = generatedConstants == null ? new() : generatedConstants.ToList();
            RelocationTargets = relocationDescriptors == null ? new() : relocationDescriptors.ToList();
            RelocationReferences = relocationReferences == null ? new() : relocationReferences.ToList();
        }

        public PartialGenerationResult()
        {
            Commands = new();
            GeneratedConstants = new();
            RelocationTargets = new();
            DataDeclarators = new();
            RelocationReferences = new();
        }

        public void Combine(PartialGenerationResult other)
        {
            Commands.AddRange(other.Commands);
            DataDeclarators.AddRange(other.DataDeclarators);

            for (int i = 0; i < other.RelocationTargets.Count; i++)
            {
                var r = other.RelocationTargets[i];
                if (r.RelocationType == RelocationType.Constant)
                {
                    // Example: constant 2 and 2, we need to change the current ConstantId to the ConstantId of first "2"
                    var existingConstant = GeneratedConstants.FindIndex(d => d.GeneratedBytes.Equals(other.GeneratedConstants[r.ConstantId].GeneratedBytes));
                    if (existingConstant != -1)
                    {
                        r.ConstantId = existingConstant;
                    }
                    else
                    {
                        r.ConstantId += GeneratedConstants.Count;
                    }
                }
            }

            RelocationTargets.AddRange(other.RelocationTargets);

            // Remove duplicated constants, stay it clean
            GeneratedConstants.AddRange(other.GeneratedConstants);
            GeneratedConstants = GeneratedConstants.DistinctBy(d => d.GeneratedBytes).ToList();

            // Process RelocationReferences
            foreach (var r in other.RelocationReferences)
            {
                RelocationReferences.Add(new(r.CommandLocation + Commands.Count, r.ReferenceType));
            }
        }
    }
}
