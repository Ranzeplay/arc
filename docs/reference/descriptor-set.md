---
title: Descriptor set
---

# Descriptor set

This file contains the collection of descriptors used in an Arc package.

## Type descriptor

- RawName

- TypeId

- IsArray

## Package descriptor

- Name

- Version

- PackageType

- EntrypointFunctionId

- DataAlignmentLength

- RootFunctionTableEntryPos

- RootConstantTableEntryPos

- RootGroupTableEntryPos

- Regions (RegionDescriptor)

## RegionDescriptor

- RegionId

- RawFullName

- FunctionTableEntryPos

- ConstantTableEntryPos

- GroupTableEntryPos

## Function descriptor

- InternalId

- RawFullName

- ArgumentTypeList (array of DataType)

> DataType
> 
> - TypeId
> 
> - IsArray
> 
> - IsAllowNone

- ReturnValueType (DataType)
- AppliedAnnotations
- EntryPoint
- BlockLength
- Accessibility

## Constant descriptor

- InternalId

- RawFullName

- TypeDescriptor

- RawData

- Accessibility

## Group descriptor

- InternalId

- RawFullName

- Fields (FieldDescriptor)

> FieldDescriptor
> 
> Index
> 
> RawName
> 
> TypeDescriptor

- Functions (FunctionDescriptor)

- Constructors (FunctionDescriptor)

- Destructors (FunctionDescriptor)
