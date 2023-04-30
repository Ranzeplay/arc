using Arc.Compiler.Shared.CommandGeneration;
using Arc.Compiler.Shared.CommandGeneration.Relocation;
using Arc.Compiler.Shared.Parsing.Components.Data;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

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

            GeneratedConstants.AddRange(other.GeneratedConstants);

            RelocationDescriptors.AddRange(other.RelocationDescriptors);
            RelocationDescriptors.ForEach(r => 
            {
                if (r.RelocationType == RelocationType.Constant)
                {
                    r.ConstantId = GeneratedConstants.IndexOf(GeneratedConstants[r.ConstantId]);
                }
            });

            GeneratedConstants = GeneratedConstants.Distinct().ToList();
        }
    }
}
