using Arc.Compiler.PackageGenerator.Models;
using Microsoft.Extensions.Logging;
using Arc.Compiler.PackageGenerator.Models.Descriptors;
using System.Diagnostics.CodeAnalysis;
using System.Text;

namespace Arc.Compiler.PackageGenerator.Encoders
{
    internal class ArcPackageDescriptorEncoder
    {
        public static IEnumerable<byte> Encode(ArcGeneratorContext context)
        {
            context.Logger.LogDebug("Encoding package descriptor");
            return Encode(context.PackageDescriptor);
        }

        [SuppressMessage("Clarity", "IDE0028", Justification = "We need to separate declaration to clarify code")]
        public static IEnumerable<byte> Encode(ArcPackageDescriptor descriptor)
        {
            var result = new List<byte>();

            result.Add((byte)descriptor.Type);

            var nameBytes = Encoding.UTF8.GetBytes(descriptor.Name);
            result.AddRange(BitConverter.GetBytes(nameBytes.LongLength));
            result.AddRange(nameBytes);

            result.AddRange(BitConverter.GetBytes(descriptor.Version));
            result.AddRange(BitConverter.GetBytes(descriptor.EntrypointFunctionId));
            result.AddRange(BitConverter.GetBytes(descriptor.DataAlignmentLength));
            result.AddRange(BitConverter.GetBytes(descriptor.RootFunctionTableEntryPos));
            result.AddRange(BitConverter.GetBytes(descriptor.RootConstantTableEntryPos));
            result.AddRange(BitConverter.GetBytes(descriptor.RootGroupTableEntryPos));
            result.AddRange(BitConverter.GetBytes(descriptor.RegionTableEntryPos));

            return result;
        }
    }
}
