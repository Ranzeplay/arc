using Arc.Compiler.PackageGenerator.Models.Descriptors;
using Arc.Compiler.PackageGenerator.Models.Descriptors.Function;
using Arc.Compiler.PackageGenerator.Models.Scope;
using Arc.Compiler.SyntaxAnalyzer.Models.Components;
using Arc.Compiler.SyntaxAnalyzer.Models.Data;

namespace Arc.Compiler.PackageGenerator.Models.Builtin.Stdlib
{
    internal class ArcStdlibConsole
    {
        public static ArcScopeTreeNamespaceNode GetNamespace()
        {
            return (ArcScopeTreeNamespaceNode)
                new ArcScopeTreeNamespaceNode("Console")
                    .AddChildChained(
                        new ArcScopeTreeBuiltinFunction("PrintString")
                        {
                            Id = 0xa1,
                            Accessibility = ArcAccessibility.Public,
                            Parameters = [
                                    new ArcParameterDescriptor()
                                    {
                                        DataType = new ArcDataDeclarationDescriptor()
                                        {
                                            Type = ArcPersistentData.StringType,
                                            AllowNone = false,
                                            Dimension = 0,
                                            MemoryStorageType = ArcMemoryStorageType.Reference,
                                            Mutability = ArcMutability.Constant,
                                        },
                                        RawFullName = "s",
                                    }
                                ],
                            ReturnValueType = new ArcDataDeclarationDescriptor()
                            {
                                Type = ArcPersistentData.NoneType,
                                AllowNone = false,
                                Dimension = 0,
                                MemoryStorageType = ArcMemoryStorageType.Reference,
                                Mutability = ArcMutability.Constant,
                            }
                        }
                    )
                    .AddChildChained(
                        new ArcScopeTreeBuiltinFunction("PrintInteger")
                        {
                            Id = 0xa2,
                            Accessibility = ArcAccessibility.Public,
                            Parameters = [
                                    new ArcParameterDescriptor()
                                    {
                                        DataType = new ArcDataDeclarationDescriptor()
                                        {
                                            Type = ArcPersistentData.IntType,
                                            AllowNone = false,
                                            Dimension = 0,
                                            MemoryStorageType = ArcMemoryStorageType.Reference,
                                            Mutability = ArcMutability.Constant,
                                        },
                                        RawFullName = "i",
                                    }
                                ],
                            ReturnValueType = new ArcDataDeclarationDescriptor()
                            {
                                Type = ArcPersistentData.NoneType,
                                AllowNone = false,
                                Dimension = 0,
                                MemoryStorageType = ArcMemoryStorageType.Reference,
                                Mutability = ArcMutability.Constant,
                            }
                        }
                    )
                    .AddChildChained(
                        new ArcScopeTreeBuiltinFunction("ReadString")
                        {
                            Id = 0xaf,
                            Accessibility = ArcAccessibility.Public,
                            ReturnValueType = new ArcDataDeclarationDescriptor()
                            {
                                Type = ArcPersistentData.StringType,
                                AllowNone = false,
                                Dimension = 0,
                                MemoryStorageType = ArcMemoryStorageType.Value,
                                Mutability = ArcMutability.Constant,
                            }
                        }
                    );
        }
    }
}
