using Arc.Compiler.PackageGenerator.Base;
using Arc.Compiler.PackageGenerator.Generators;
using Arc.Compiler.PackageGenerator.Models;
using Arc.Compiler.PackageGenerator.Models.Builtin;
using Arc.Compiler.PackageGenerator.Models.Descriptors;
using Arc.Compiler.PackageGenerator.Models.Descriptors.Function;
using Arc.Compiler.PackageGenerator.Models.Descriptors.Group;
using Arc.Compiler.PackageGenerator.Models.Intermediate;
using Arc.Compiler.PackageGenerator.Models.Relocation;
using Microsoft.Extensions.Logging;

namespace Arc.Compiler.PackageGenerator.Encoders
{
    internal static class ArcSymbolTableEncoder
    {
        public static IEnumerable<byte> Encode(ArcGeneratorContext context)
        {
            var stringEncoder = new ArcStringEncoder();

            var result = new List<byte>();

            var validSymbolNodes = context.GlobalScopeTree.FlattenedNodes.Where(n => n.GetSymbols().Any());
            result.AddRange(BitConverter.GetBytes(validSymbolNodes.LongCount()));
            foreach (var node in validSymbolNodes)
            {
                foreach (var symbol in node.GetSymbols())
                {
                    var iterResult = new List<byte>();
                    iterResult.AddRange(BitConverter.GetBytes(symbol.Id));

                    switch (symbol)
                    {
                        case ArcFunctionDescriptor functionDescriptor:
                            {
                                iterResult.Add((byte)ArcSymbolType.Function);
                                iterResult.AddRange(BitConverter.GetBytes(functionDescriptor.EntrypointPos));
                                iterResult.AddRange(BitConverter.GetBytes(functionDescriptor.BlockLength));
                                iterResult.AddRange(stringEncoder.Encode(node.Signature));

                                // Return type descriptor
                                iterResult.AddRange(functionDescriptor.ReturnValueType.Encode(context.Symbols.Values));

                                // Parameter type descriptors
                                iterResult.AddRange(BitConverter.GetBytes(functionDescriptor.Parameters.LongCount()));
                                foreach (var parameter in functionDescriptor.Parameters)
                                {
                                    iterResult.AddRange(parameter.DataType.Encode(context.Symbols.Values));
                                }

                                // Annotation descriptors
                                iterResult.AddRange(BitConverter.GetBytes(functionDescriptor.Annotations.LongCount()));
                                foreach (var annotation in functionDescriptor.Annotations)
                                {
                                    iterResult.AddRange(BitConverter.GetBytes(annotation.Id));
                                }

                                // var entryPoint = context.Labels.FirstOrDefault(x => x.Type == ArcRelocationLabelType.BeginFunction && x.Name == functionDescriptor.RawFullName);
                                // iterResult.AddRange(BitConverter.GetBytes(entryPoint.Location));
                                break;
                            }
                        case ArcGroupDescriptor groupDescriptor:
                            {
                                iterResult.Add((byte)ArcSymbolType.Group);
                                iterResult.AddRange(stringEncoder.Encode(node.Signature));
                                iterResult.AddRange(ArcGroupSymbolEncoder.EncodeGroupSymbol(groupDescriptor));
                                break;
                            }
                        case ArcGroupFieldDescriptor fieldDescriptor:
                            {
                                iterResult.Add((byte)ArcSymbolType.GroupField);
                                iterResult.AddRange(stringEncoder.Encode(node.Signature));
                                iterResult.AddRange(fieldDescriptor.DataType.Encode(context.Symbols.Values));
                                break;
                            }
                        case ArcNamespaceDescriptor namespaceDescriptor:
                            {
                                iterResult.Add((byte)ArcSymbolType.Namespace);
                                iterResult.AddRange(stringEncoder.Encode(node.Signature));
                                break;
                            }
                        case ArcTypeBase typeBase:
                            {
                                iterResult.Add((byte)ArcSymbolType.DataType);
                                iterResult.Add((byte)((typeBase is ArcBaseType) ? 0x00 : 0x01));
                                iterResult.AddRange(stringEncoder.Encode(node.Signature));
                                if (typeBase is ArcComplexType complexType)
                                {
                                    iterResult.AddRange(BitConverter.GetBytes(complexType.GroupId));
                                }
                                break;
                            }
                        case ArcAnnotationDescriptor annotationDescriptor:
                            {
                                iterResult.Add((byte)ArcSymbolType.Annotation);
                                iterResult.AddRange(stringEncoder.Encode(node.Signature));
                                iterResult.AddRange(BitConverter.GetBytes(annotationDescriptor.TargetGroup.Id));
                                break;
                            }
                    }

                    // Print iterResult in hex
                    context.Logger.LogTrace("Symbol: {}", BitConverter.ToString([.. iterResult]).Replace("-", " "));

                    result.AddRange(iterResult);
                }
            }

            context.Logger.LogInformation("Generated {} symbols into symbol table", context.Symbols.Count);

            return result;
        }
    }
}
