# C# Type System

## Quick Lookup

### Boxing

Short version: Value type wrapped as `object`, usually with an allocation.

### Struct

Short version: Value type, copied by value.

### Class

Short version: Reference type, copied by reference.

### Record

Short version: Data-focused type with value equality by default.

### Nullable reference types

Short version: Compile-time nullability warnings.

## Most Important FAQ

### What is the highest-frequency interview area here?

Boxing/unboxing, struct vs class, record vs class, and nullable reference types are the most common.

### What should I remember about records?

Record classes are still reference types, but they are designed for data and provide value-based equality by default.

## Boxing And Unboxing

**Interview answer:** Boxing wraps a value type as `object`, normally causing an allocation. Unboxing extracts the original value type.

**In simple words:** Boxing puts a value inside an object box; unboxing takes it back out.

```csharp
int number = 10;
object boxed = number;
int result = (int)boxed;
```

Generics such as `List<int>` help avoid boxing.

**Catch:** Unboxing must use the exact original value type. A boxed `int` cannot be directly unboxed as `long`, even though an `int` can normally be converted to `long`.

## Struct Vs Class

**Interview answer:** A struct is a value type, so assignment copies its value. A class is a reference type, so assignment copies a reference to the same object.

**In simple words:** Copying a struct creates another value; copying a class variable gives another pointer to the same object.

Use structs for small, preferably immutable values. Use classes for entities, larger objects, shared state, and inheritance.

**Catch:** Structs are not always stored on the stack. Also, copying a mutable struct creates a separate value, which can produce surprising behavior.

## Record Vs Class

**Interview answer:** A record is designed for value-like data and provides value equality by default. A normal class uses reference equality unless equality is implemented manually.

**In simple words:** Two records with the same data are considered equal; two normal class objects are usually different objects.

A record class is also a reference type, unless it is declared as `record struct`, but it is designed for data rather than behavior.

A class is generally used to model objects with behavior and mutable state. A record is designed for data-centric objects and provides value-based equality, immutability support, deconstruction, and non-destructive mutation through the `with` expression. Records are commonly used for DTOs, events, and messages, while classes are used for domain models and services.

```csharp
public record User(string Name);

Console.WriteLine(new User("Sam") == new User("Sam")); // True
```

Immutability:

```csharp
public record Person(string Name);

var p1 = new Person("Sam");
// p1.Name = "John"; // Not allowed for positional init-only property

var p2 = p1 with { Name = "John" };
```

The `with` expression creates a copy with selected values changed.

**Catch:** A record class is still a reference type. Its generated equality compares its data, but reference-type properties inside it may still use their own equality behavior.

Structs are value types, meaning they are copied when passed around. Records are reference types by default unless declared as `record struct`. The key difference is that records give built-in value-based equality and immutability patterns, which are useful for data-centric scenarios.

Use structs for small, frequently copied values like coordinates or colors. Use records for DTOs, messages, immutable models, and other data-oriented types where equality and expressiveness matter.

## Nullable Reference Types

**Interview answer:** Nullable reference types let the compiler warn about possible null-reference errors. `string` means null is not expected; `string?` means it is allowed.

**In simple words:** They help catch likely null mistakes before the program runs.

```csharp
string name = "Sam";
string? optionalName = null;
```

They are compile-time checks, not a new runtime type. `!` only suppresses a warning; it does not perform a null check.

**Catch:** Nullable warnings do not prevent null at runtime. Values from reflection, old libraries, deserialization, or incorrectly annotated code can still violate the declared contract.
