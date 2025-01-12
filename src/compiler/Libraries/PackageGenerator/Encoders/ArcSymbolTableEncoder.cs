using Arc.Compiler.PackageGenerator.Models.Descriptors.Function;
using Arc.Compiler.PackageGenerator.Models.Descriptors.Group;
using Arc.Compiler.PackageGenerator.Models.Descriptors;
using Arc.Compiler.PackageGenerator.Models.Relocation;
using Arc.Compiler.PackageGenerator.Models;
using Microsoft.Extensions.Logging;

namespace Arc.Compiler.PackageGenerator.Encoders
{
    internal static class ArcSymbolTableEncoder
    {
        public static IEnumerable<byte> Encode(ArcGeneratorContext context)
        {
            context.Symbols.Remove(0);
            context.Symbols.Remove(1);
            context.Symbols.Remove(2);
            context.Symbols.Remove(3);
            context.Symbols.Remove(4);
            context.Symbols.Remove(5);
            context.Symbols.Remove(6);

            var result = new List<byte>();
            result.AddRange(BitConverter.GetBytes((long)context.Symbols.Count));

            foreach (var symbol in context.Symbols.Values)
            {
                var iterResult = new List<byte>();
                iterResult.AddRange(BitConverter.GetBytes(symbol.Id));

                if (symbol is ArcFunctionDescriptor functionDescriptor)
                {
                    iterResult.Add((byte)ArcSymbolType.Function);
                    iterResult.AddRange(BitConverter.GetBytes(functionDescriptor.EntrypointPos));
                    iterResult.AddRange(Utils.SerializeString(functionDescriptor.RawFullName));

                    // var entryPoint = context.Labels.FirstOrDefault(x => x.Type == ArcRelocationLabelType.BeginFunction && x.Name == functionDescriptor.RawFullName);
                    // iterResult.AddRange(BitConverter.GetBytes(entryPoint.Location));
                }
                else if (symbol is ArcGroupDescriptor groupDescriptor)
                {
                    iterResult.Add((byte)ArcSymbolType.Group);
                    iterResult.AddRange(Utils.SerializeString(groupDescriptor.Name));
                }
                else if (symbol is ArcGroupFieldDescriptor fieldDescriptor)
                {
                    iterResult.Add((byte)ArcSymbolType.GroupField);
                    iterResult.AddRange(Utils.SerializeString(fieldDescriptor.Name));
                }
                else if (symbol is ArcDataTypeDescriptor dataTypeDescriptor)
                {
                    iterResult.Add((byte)ArcSymbolType.DataType);
                    iterResult.AddRange(Utils.SerializeString(dataTypeDescriptor.Name));
                }
                else if (symbol is ArcNamespaceDescriptor namespaceDescriptor)
                {
                    iterResult.Add((byte)ArcSymbolType.Namespace);
                    iterResult.AddRange(Utils.SerializeString(namespaceDescriptor.Name));
                }

                // Print iterResult in hex
                context.Logger.LogTrace("Symbol: {}", BitConverter.ToString([.. iterResult]).Replace("-", " "));

                result.AddRange(iterResult);
            }

            context.Logger.LogInformation("Generated {} symbols into symbol table", result.Count);

            return result;
        }
    }
}
