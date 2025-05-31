using Arc.Compiler.PackageGenerator.Models;
using Microsoft.Extensions.Logging;
using System.Diagnostics.CodeAnalysis;
using System.Text;

namespace Arc.Compiler.PackageGenerator.Encoders
{
    internal class ArcPackageDescriptorEncoder
    {
        [SuppressMessage("Clarity", "IDE0028", Justification = "We need to separate declaration to clarify code")]
        public static IEnumerable<byte> Encode(ArcGeneratorContext context)
        {
            context.Logger.LogDebug("Encoding package descriptor");

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
