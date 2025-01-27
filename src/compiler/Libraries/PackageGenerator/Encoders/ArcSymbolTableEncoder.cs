using Arc.Compiler.PackageGenerator.Models.Descriptors.Function;
using Arc.Compiler.PackageGenerator.Models.Descriptors.Group;
using Arc.Compiler.PackageGenerator.Models.Descriptors;
using Arc.Compiler.PackageGenerator.Models.Relocation;
using Arc.Compiler.PackageGenerator.Models;
using Microsoft.Extensions.Logging;
using Arc.Compiler.PackageGenerator.Base;
using Arc.Compiler.PackageGenerator.Models.Builtin;
using Arc.Compiler.PackageGenerator.Models.Intermediate;

namespace Arc.Compiler.PackageGenerator.Encoders
{
    internal static class ArcSymbolTableEncoder
    {
        public static IEnumerable<byte> Encode(ArcGeneratorContext context)
        {
            var result = new List<byte>();

            var validSymbols = context.Symbols.Values.Where(x => x.Id > 6);
            result.AddRange(BitConverter.GetBytes(validSymbols.LongCount()));
            foreach (var symbol in validSymbols)
            {
                var iterResult = new List<byte>();
                iterResult.AddRange(BitConverter.GetBytes(symbol.Id));

                switch (symbol)
                {
                    case ArcFunctionDescriptor functionDescriptor:
                        {
                            iterResult.Add((byte)ArcSymbolType.Function);
                            iterResult.AddRange(BitConverter.GetBytes(functionDescriptor.EntrypointPos));
                            iterResult.AddRange(Utils.SerializeString(functionDescriptor.RawFullName));

                            // var entryPoint = context.Labels.FirstOrDefault(x => x.Type == ArcRelocationLabelType.BeginFunction && x.Name == functionDescriptor.RawFullName);
                            // iterResult.AddRange(BitConverter.GetBytes(entryPoint.Location));
                            break;
                        }
                    case ArcGroupDescriptor groupDescriptor:
                        {
                            iterResult.Add((byte)ArcSymbolType.Group);
                            iterResult.AddRange(GroupSymbolEncoder.EncodeGroupSymbol(groupDescriptor));
                            break;
                        }
                    case ArcGroupFieldDescriptor fieldDescriptor:
                        {
                            iterResult.Add((byte)ArcSymbolType.GroupField);
                            iterResult.AddRange(Utils.SerializeString(fieldDescriptor.Name));
                            iterResult.AddRange(fieldDescriptor.DataType.Encode(context.Symbols.Values));
                            break;
                        }
                    case ArcNamespaceDescriptor namespaceDescriptor:
                        {
                            iterResult.Add((byte)ArcSymbolType.Namespace);
                            iterResult.AddRange(Utils.SerializeString(namespaceDescriptor.Name));
                            break;
                        }
                    case ArcTypeBase typeBase:
                        {
                            iterResult.Add((byte)ArcSymbolType.DataType);
                            iterResult.Add((byte)((typeBase is ArcBaseType) ? 0x00 : 0x01));
                            iterResult.AddRange(Utils.SerializeString(typeBase.FullName));
                            if (typeBase is ArcDerivativeType derivativeType)
                            {
                                iterResult.AddRange(BitConverter.GetBytes(derivativeType.GroupId));
                            }
                            break;
                        }
                }

                // Print iterResult in hex
                context.Logger.LogTrace("Symbol: {}", BitConverter.ToString([.. iterResult]).Replace("-", " "));

                result.AddRange(iterResult);
            }

            context.Logger.LogInformation("Generated {} symbols into symbol table", context.Symbols.Count);

            return result;
        }
    }
}
