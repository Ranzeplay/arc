---
title: Array
---

`Arc::Std::Array`

# Array

## Groups

None.

## Functions

### CreateIntArray

```csharp
public func CreateIntArray(): val int[]
```

#### Parameters

None.

#### Returns

The array just created.

### PushIntArray

```csharp
public func PushIntArray(const i: val int): ref none
```

#### Parameters

`i`: The integer to push into the array.

#### Returns

None.

### RemoveElementFromIntArray

```csharp
public func RemoveElementFromIntArray(const index: ref int): ref none
```

#### Parameters

- `index`: The index to remove.

#### Returns

None.

### GetIntArraySize

```csharp
public func GetIntArraySize(const arr: ref int[]): val int
```

#### Parameters

- `arr`: The array to be evaluated.

#### Returns

The size of the array.