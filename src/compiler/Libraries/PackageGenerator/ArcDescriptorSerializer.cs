using Arc.Compiler.PackageGenerator.Models;
using Arc.Compiler.PackageGenerator.Models.Descriptors;
using Arc.Compiler.PackageGenerator.Models.Descriptors.Function;
using Arc.Compiler.PackageGenerator.Models.Descriptors.Group;
using Arc.Compiler.PackageGenerator.Models.Relocation;
using System.Text;

namespace Arc.Compiler.PackageGenerator
{
    internal class ArcDescriptorSerializer
    {
        public static IEnumerable<byte> SerializeSymbolTable(ArcGeneratorContext context)
        {
            var result = new List<byte>();

            foreach (var symbol in context.Symbols)
            {
                var iterResult = new List<byte>();
                iterResult.AddRange(BitConverter.GetBytes(symbol.Key));

                if (symbol.Value is ArcFunctionDescriptor functionDescriptor)
                {
                    iterResult.AddRange(BitConverter.GetBytes(functionDescriptor.Id));
                    iterResult.AddRange((byte)ArcSymbolType.Function);
                    iterResult.AddRange(Utils.SerializeString(functionDescriptor.RawFullName));

                    var entryPoint = context.Labels.FirstOrDefault(x => x.Type == ArcRelocationLabelType.BeginFunction && x.Name == functionDescriptor.RawFullName);
                    iterResult.AddRange(BitConverter.GetBytes(entryPoint.Location));
                }
                else if (symbol.Value is ArcGroupDescriptor groupDescriptor)
                {
                    iterResult.AddRange(BitConverter.GetBytes(groupDescriptor.Id));
                    iterResult.AddRange((byte)ArcSymbolType.Group);
                    iterResult.AddRange(Utils.SerializeString(groupDescriptor.Name));
                }
                else if (symbol.Value is ArcGroupFieldDescriptor fieldDescriptor)
                {
                    iterResult.AddRange(BitConverter.GetBytes(fieldDescriptor.Id));
                    iterResult.AddRange((byte)ArcSymbolType.GroupField);
                    iterResult.AddRange(Utils.SerializeString(fieldDescriptor.Name));
                }
                else if (symbol.Value is ArcDataTypeDescriptor dataTypeDescriptor)
                {
                    iterResult.AddRange(BitConverter.GetBytes(dataTypeDescriptor.Id));
                    iterResult.AddRange((byte)ArcSymbolType.DataType);
                    iterResult.AddRange(Utils.SerializeString(dataTypeDescriptor.Name));
                }
                else if (symbol.Value is ArcNamespaceDescriptor namespaceDescriptor)
                {
                    iterResult.AddRange(BitConverter.GetBytes(namespaceDescriptor.Id));
                    iterResult.AddRange((byte)ArcSymbolType.Namespace);
                    iterResult.AddRange(Utils.SerializeString(namespaceDescriptor.Name));
                }

                result.AddRange(iterResult);
            }

            return result;
        }

        public static IEnumerable<byte> SerializeConstantTable(ArcGeneratorContext context)
        {
            var result = new List<byte>();

            foreach (var constant in context.Constants)
            {
                var iterResult = new List<byte>();
                iterResult.AddRange(BitConverter.GetBytes(constant.Id));
                iterResult.AddRange(BitConverter.GetBytes(constant.TypeId));
                iterResult.AddRange(BitConverter.GetBytes(constant.RawData.Count()));
                iterResult.AddRange(constant.RawData);

                result.AddRange(iterResult);
            }

            return result;
        }

        public static IEnumerable<byte> SerializePackageDescriptor(ArcGeneratorContext context)
        {
            var result = new List<byte>();

            result.AddRange((byte)context.PackageDescriptor.Type);

            var nameBytes = Encoding.UTF8.GetBytes(context.PackageDescriptor.Name);
            result.AddRange(BitConverter.GetBytes(nameBytes.Length));
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
