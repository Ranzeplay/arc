using Arc.Compiler.PackageGenerator.Models;
using Arc.Compiler.PackageGenerator.Models.Descriptors;
using Arc.Compiler.PackageGenerator.Models.Descriptors.Function;
using Arc.Compiler.PackageGenerator.Models.Descriptors.Group;
using Arc.Compiler.PackageGenerator.Models.Relocation;
using System.Text;

namespace Arc.Compiler.PackageGenerator
{
    internal static class ArcDescriptorSerializer
    {
        public static IEnumerable<byte> SerializeSymbolTable(ArcGeneratorContext context)
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
                Console.WriteLine(BitConverter.ToString(iterResult.ToArray()).Replace("-", " "));

                result.AddRange(iterResult);
            }

            Console.WriteLine($"Symbol table serialized, {context.Symbols.Count} in total");

            return result;
        }

        public static IEnumerable<byte> SerializeConstantTable(ArcGeneratorContext context)
        {
            var result = new List<byte>();
            result.AddRange(BitConverter.GetBytes(context.Constants.LongCount()));

            foreach (var constant in context.Constants)
            {
                var iterResult = new List<byte>();
                iterResult.AddRange(BitConverter.GetBytes(constant.Id));
                iterResult.AddRange(BitConverter.GetBytes(constant.TypeId));
                iterResult.AddRange(BitConverter.GetBytes(constant.RawData.LongCount()));
                iterResult.AddRange(constant.RawData);

                result.AddRange(iterResult);

                Console.WriteLine(BitConverter.ToString(iterResult.ToArray()).Replace("-", " "));
            }

            Console.WriteLine($"Constant table serialized, {context.Constants.Count()} in total");

            return result;
        }

        public static IEnumerable<byte> SerializePackageDescriptor(ArcGeneratorContext context)
        {
            var result = new List<byte>();

            result.Add((byte)context.PackageDescriptor.Type);

            var nameBytes = Encoding.UTF8.GetBytes(context.PackageDescriptor.Name);
            result.AddRange(BitConverter.GetBytes(nameBytes.LongLength));
            result.AddRange(nameBytes);

            result.AddRange(BitConverter.GetBytes(context.PackageDescriptor.Version));
            result.AddRange(BitConverter.GetBytes(context.PackageDescriptor.EntrypointFunctionId));
            result.AddRange(BitConverter.GetBytes(context.PackageDescriptor.DataAlignmentLength));
            result.AddRange(BitConverter.GetBytes(context.PackageDescriptor.RootFunctionTableEntryPos));
            result.AddRange(BitConverter.GetBytes(context.PackageDescriptor.RootConstantTableEntryPos));
            result.AddRange(BitConverter.GetBytes(context.PackageDescriptor.RootGroupTableEntryPos));
            result.AddRange(BitConverter.GetBytes(context.PackageDescriptor.RegionTableEntryPos));

            return result;
        }
    }
}
