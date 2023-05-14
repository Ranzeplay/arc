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

        public List<RelocationDescriptor> RelocationDescriptors { get; set; }

        public PartialGenerationResult(List<byte> commands, IEnumerable<DataDeclarator>? dataDeclarators = null, IEnumerable<GeneratedConstant>? generatedConstants = null, IEnumerable<RelocationDescriptor>? relocationDescriptors = null)
        {
            Commands = commands;
            DataDeclarators = dataDeclarators == null ? new() : dataDeclarators.ToList();
            GeneratedConstants = generatedConstants == null ? new() : generatedConstants.ToList();
            RelocationDescriptors = relocationDescriptors == null ? new() : relocationDescriptors.ToList();
        }

        public PartialGenerationResult()
        {
            Commands = new();
            GeneratedConstants = new();
            RelocationDescriptors = new();
            DataDeclarators = new();
        }

        public void Combine(PartialGenerationResult other)
        {
            Commands.AddRange(other.Commands);
            DataDeclarators.AddRange(other.DataDeclarators);

            for (int i = 0; i < other.RelocationDescriptors.Count; i++)
            {
                var r = other.RelocationDescriptors[i];
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

            RelocationDescriptors.AddRange(other.RelocationDescriptors);

            // Remove duplicated constants, stay it clean
            GeneratedConstants.AddRange(other.GeneratedConstants);
            GeneratedConstants = GeneratedConstants.DistinctBy(d => d.GeneratedBytes).ToList();
        }
    }
}
