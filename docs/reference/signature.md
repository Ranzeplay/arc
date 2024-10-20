---
title: Signature
---

# Signature

## Scope signature

Consider the scope `s1::s2::s3::s4::...`.

The signature will be `s1:s2:s3:s4:...`.

**Example**

`a::b::c` --> `a:b:c`

## Function signature

Consider the function `func fnName(var a1: val t1, var a2: ref t2, const a3: val t3[]): val tr` in scope `s1::s2`.

The signature will be `s1:s2+FfnName@VSt1&RSt2&VAt3*VStr`, with `F` indicating function.

Also, it will create a new function scope.

### Components

- Scope quantifier

- `+`

- Function name

- `@`

- Arg1

- `&`

- Arg2

- `&`

- ...

- `&`

- Argn

- `*`

## Variable signature

Variables are declared in a function

Consider the variable declared by `var a: val int` in the function mentioned before.

The signature will be `s1:s2+FfnName@VSt1&RSt2&VAt3*VStr+VSint@a`

## Type signature

### Base type

The signature of base types are their name.

## Group signature

### Group scope signature

Consider a group named `Foo` under namespace `NS1::NS2`

The signature will be `NS1:NS2+GFoo`, with `G` indicating the group.

### Field signature

Consider the group mentioned above has a field `const bar: val string`.

The signature will be `NS1:NS2+GFoo+DVSstring@bar`, with `D` indicating field.

### Function signature

Consider the group mentioned above has a function `func eval(): val bool`, the signature will be `NS1:NS2+GFoo+Feval@*VSbool`
