using Arc.Compiler.PackageGenerator.Models.Descriptors;
using Arc.Compiler.PackageGenerator.Models.Descriptors.Function;
using Arc.Compiler.PackageGenerator.Models.Scope;
using Arc.Compiler.SyntaxAnalyzer.Models.Components;
using Arc.Compiler.SyntaxAnalyzer.Models.Data;

namespace Arc.Compiler.PackageGenerator.Models.Builtin.Stdlib
{
    class ArcStdlibArray
    {
        public static ArcScopeTreeNamespaceNode GetNamespace()
        {
            return (ArcScopeTreeNamespaceNode)
                new ArcScopeTreeNamespaceNode("Array")
                    .AddChildChained(
                        new ArcScopeTreeBuiltinFunction("CreateIntArray")
                        {
                            Id = 0xc1,
                            Accessibility = ArcAccessibility.Public,
                            Parameters = [],
                            ReturnValueType = new ArcDataDeclarationDescriptor()
                            {
                                Type = ArcPersistentData.IntType,
                                AllowNone = false,
                                Dimension = 1,
                                MemoryStorageType = ArcMemoryStorageType.Value,
                                Mutability = ArcMutability.Variable,
                            }
                        }
                    )
                    .AddChildChained(
                        new ArcScopeTreeBuiltinFunction("PushIntArray")
                        {
                            Id = 0xc2,
                            Accessibility = ArcAccessibility.Public,
                            Parameters = [
                                    new ArcParameterDescriptor()
                                    {
                                        DataType = new ArcDataDeclarationDescriptor()
                                        {
                                            Type = ArcPersistentData.IntType,
                                            AllowNone = false,
                                            Dimension = 1,
                                            MemoryStorageType = ArcMemoryStorageType.Value,
                                            Mutability = ArcMutability.Constant,
                                        },
                                        RawFullName = "array",
                                    },
                                    new ArcParameterDescriptor()
                                    {
                                        DataType = new ArcDataDeclarationDescriptor()
                                        {
                                            Type = ArcPersistentData.IntType,
                                            AllowNone = false,
                                            Dimension = 0,
                                            MemoryStorageType = ArcMemoryStorageType.Value,
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
                        new ArcScopeTreeBuiltinFunction("RemoveElementFromIntArray")
                        {
                            Id = 0xc3,
                            Accessibility = ArcAccessibility.Public,
                            Parameters = [
                                    new ArcParameterDescriptor()
                                    {
                                        DataType = new ArcDataDeclarationDescriptor()
                                        {
                                            Type = ArcPersistentData.IntType,
                                            AllowNone = false,
                                            Dimension = 1,
                                            MemoryStorageType = ArcMemoryStorageType.Value,
                                            Mutability = ArcMutability.Constant,
                                        },
                                        RawFullName = "array",
                                    },
                                    new ArcParameterDescriptor()
                                    {
                                        DataType = new ArcDataDeclarationDescriptor()
                                        {
                                            Type = ArcPersistentData.IntType,
                                            AllowNone = false,
                                            Dimension = 0,
                                            MemoryStorageType = ArcMemoryStorageType.Value,
                                            Mutability = ArcMutability.Constant,
                                        },
                                        RawFullName = "index",
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
                        new ArcScopeTreeBuiltinFunction("GetIntArraySize")
                        {
                            Id = 0xc4,
                            Accessibility = ArcAccessibility.Public,
                            Parameters = [],
                            ReturnValueType = new ArcDataDeclarationDescriptor()
                            {
                                Type = ArcPersistentData.IntType,
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
