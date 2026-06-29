# Revision Notes Master Index

This is the handoff file for the whole repo. Start here, choose a topic, and jump directly to the note you need.

## Start Here

| Need | Open |
| --- | --- |
| Full .NET revision map | [Dotnet/README.md](Dotnet/README.md) |
| Dependency Injection deep dive | [Dotnet/DependencyInjection/README.md](Dotnet/DependencyInjection/README.md) |
| CLR/runtime deep dive | [Dotnet/CLR/README.md](Dotnet/CLR/README.md) |
| Interview question checklist | [Dotnet/Questions.md](Dotnet/Questions.md) |
| Low-level design examples | [LLD/README.md](LLD/README.md) |

## Revision Priority

| Priority | Topics | Why |
| --- | --- | --- |
| Must know | Async, DI, CLR memory, GC, OOP, equality | High-frequency interview topics with many follow-ups |
| Strong follow-up | Delegates/events, attributes, reflection, resource management, extension methods | Common in deeper .NET rounds |
| Advanced | Expression trees, open generics, detailed runtime flow | Useful for senior-level depth |
| LLD practice | Bulb/Room, Parking Lot | Helps explain modeling, ownership, and extensibility |

## Master Topic Index

### C# And .NET Core

| Topic | File | Revise for |
| --- | --- | --- |
| Async and concurrency | [Dotnet/AsyncAwait.md](Dotnet/AsyncAwait.md) | `async` internals, `Task` vs `Thread`, deadlocks, cancellation |
| Delegates and events | [Dotnet/DelegatesAndEvents.md](Dotnet/DelegatesAndEvents.md) | Delegates, events, `Action`, `Func`, `Predicate`, variance |
| Extension methods | [Dotnet/ExtensionMethods.md](Dotnet/ExtensionMethods.md) | Syntax, resolution rules, generic extensions, namespace scope |
| Attributes | [Dotnet/Attributes.md](Dotnet/Attributes.md) | Metadata, custom attributes, reflection usage |
| Reflection | [Dotnet/Reflection.md](Dotnet/Reflection.md) | Runtime metadata, dynamic invocation, cost/pitfalls |
| Expression trees | [Dotnet/ExpressionTrees.md](Dotnet/ExpressionTrees.md) | Code as data, LINQ providers, `IQueryable<T>` |
| Type system | [Dotnet/TypeSystem.md](Dotnet/TypeSystem.md) | Boxing, structs, classes, records, nullable references |
| OOP | [Dotnet/ObjectOrientedProgramming.md](Dotnet/ObjectOrientedProgramming.md) | Abstract/interface, virtual/abstract, `override` vs `new`, sealed |
| Equality | [Dotnet/Equality.md](Dotnet/Equality.md) | `==`, `Equals`, `GetHashCode`, `IEquatable<T>` |
| Resource management | [Dotnet/ResourceManagement.md](Dotnet/ResourceManagement.md) | `IDisposable`, finalizers, deterministic cleanup |

### Dependency Injection

| Topic | File | Revise for |
| --- | --- | --- |
| DI entry point | [Dotnet/DependencyInjection/DI.md](Dotnet/DependencyInjection/DI.md) | Quick DI lookup and revision order |
| Fundamentals | [Dotnet/DependencyInjection/Fundamentals.md](Dotnet/DependencyInjection/Fundamentals.md) | DI vs IoC, tight coupling, constructor injection |
| Lifetimes | [Dotnet/DependencyInjection/Lifetimes.md](Dotnet/DependencyInjection/Lifetimes.md) | Transient/scoped/singleton, singleton-to-scoped issue |
| Service resolution | [Dotnet/DependencyInjection/ServiceResolution.md](Dotnet/DependencyInjection/ServiceResolution.md) | `IServiceProvider`, object graph creation, missing dependency errors |
| Multiple implementations | [Dotnet/DependencyInjection/MultipleImplementations.md](Dotnet/DependencyInjection/MultipleImplementations.md) | Last registration wins, `IEnumerable<T>`, lifetime behavior |
| Open generics and factories | [Dotnet/DependencyInjection/OpenGenericsAndFactories.md](Dotnet/DependencyInjection/OpenGenericsAndFactories.md) | `IRepository<>`, factory vs DI container |
| When to use DI | [Dotnet/DependencyInjection/WhenToUseDI.md](Dotnet/DependencyInjection/WhenToUseDI.md) | Services vs DTOs/POCOs/value objects |
| Service provider APIs | [Dotnet/DependencyInjection/ServiceProviderApis.md](Dotnet/DependencyInjection/ServiceProviderApis.md) | `GetService<T>()` vs `GetRequiredService<T>()` |

### CLR And Runtime

| Topic | File | Revise for |
| --- | --- | --- |
| CLR entry point | [Dotnet/CLR/README.md](Dotnet/CLR/README.md) | Runtime revision order |
| CLR memory | [Dotnet/CLR/Memory.md](Dotnet/CLR/Memory.md) | Object allocation, heap vs stack, references |
| Runtime execution | [Dotnet/CLR/RuntimeExecution.md](Dotnet/CLR/RuntimeExecution.md) | Assemblies, IL, CLR loading, JIT |
| Garbage collection | [Dotnet/CLR/GarbageCollection.md](Dotnet/CLR/GarbageCollection.md) | Generations, LOH, leaks |
| Managed/unmanaged memory | [Dotnet/CLR/ManagedUnmanagedMemory.md](Dotnet/CLR/ManagedUnmanagedMemory.md) | `Dispose`, DB connections, connection pooling |

### Low-Level Design

| Topic | File | Revise for |
| --- | --- | --- |
| LLD entry point | [LLD/README.md](LLD/README.md) | How to explain LLD answers |
| Bulb and Room | [LLD/Bulb/README.md](LLD/Bulb/README.md) | Encapsulation, aggregation vs composition, state transitions |
| Parking Lot | [LLD/ParkingLot/README.md](LLD/ParkingLot/README.md) | Entities, strategy pattern, tickets, locking, fee calculation |

## Suggested Revision Path

1. [Dotnet/AsyncAwait.md](Dotnet/AsyncAwait.md)
2. [Dotnet/DependencyInjection/README.md](Dotnet/DependencyInjection/README.md)
3. [Dotnet/CLR/README.md](Dotnet/CLR/README.md)
4. [Dotnet/ObjectOrientedProgramming.md](Dotnet/ObjectOrientedProgramming.md)
5. [Dotnet/Equality.md](Dotnet/Equality.md)
6. [Dotnet/ResourceManagement.md](Dotnet/ResourceManagement.md)
7. [Dotnet/Questions.md](Dotnet/Questions.md)
8. [LLD/README.md](LLD/README.md)

## How To Use Each Topic File

1. Read `Quick Lookup`.
2. Read `Most Important FAQ`.
3. Explain the answer aloud without looking.
4. Recreate the smallest code snippet from memory.
5. Answer the `Interview Follow-Ups`.
6. Revisit `Normal FAQ` only after the must-know answers are clear.

## Maintenance Rules

| Rule | Reason |
| --- | --- |
| Keep one file per topic unless the note is tiny | Easier lookup during revision |
| Split large topics into a folder with its own `README.md` | Avoid giant scrolling files |
| Keep examples and follow-ups intact | They are the most useful part for interview prep |
| Put new links in this master README | This file should always be enough for handoff |
| Use `Quick Lookup`, `Most Important FAQ`, `Normal FAQ`, `Examples`, `Interview Follow-Ups` | Consistent revision rhythm |

## Current Repo Shape

```text
notes/
|-- README.md
|-- Dotnet/
|   |-- README.md
|   |-- Questions.md
|   |-- AsyncAwait.md
|   |-- DelegatesAndEvents.md
|   |-- ExtensionMethods.md
|   |-- Attributes.md
|   |-- Reflection.md
|   |-- ExpressionTrees.md
|   |-- TypeSystem.md
|   |-- ObjectOrientedProgramming.md
|   |-- Equality.md
|   |-- ResourceManagement.md
|   |-- CLR/
|   |   |-- README.md
|   |   |-- Memory.md
|   |   |-- RuntimeExecution.md
|   |   |-- GarbageCollection.md
|   |   |-- ManagedUnmanagedMemory.md
|   |-- DependencyInjection/
|       |-- README.md
|       |-- DI.md
|       |-- Fundamentals.md
|       |-- Lifetimes.md
|       |-- ServiceResolution.md
|       |-- MultipleImplementations.md
|       |-- OpenGenericsAndFactories.md
|       |-- WhenToUseDI.md
|       |-- ServiceProviderApis.md
|-- LLD/
    |-- README.md
    |-- Bulb/
    |   |-- README.md
    |   |-- Bulb.cs
    |-- ParkingLot/
        |-- README.md
        |-- ParkingLot.cs
```
