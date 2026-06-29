# CLR Lookup

Use this folder for runtime, memory, and GC revision.

## Topics

| Topic | File | Must revise |
| --- | --- | --- |
| CLR Memory | [Memory.md](Memory.md) | Object allocation, heap vs stack, references, allocation failure |
| CLR Execution Flow | [RuntimeExecution.md](RuntimeExecution.md) | Assembly, IL, CLR loading, JIT, native code |
| Garbage Collection | [GarbageCollection.md](GarbageCollection.md) | Generations, LOH, promotion, leaks |
| Managed and unmanaged memory | [ManagedUnmanagedMemory.md](ManagedUnmanagedMemory.md) | Managed vs unmanaged resources, database connections, pooling |

## Most Important FAQ

### What should I say if asked "What does CLR do?"

CLR loads assemblies, verifies and executes IL, performs JIT compilation to native code, manages memory, runs garbage collection, handles exceptions, and provides runtime services such as type safety and security checks.

### What should I revise first?

Revise execution flow first, then memory allocation, then garbage collection, then managed vs unmanaged resources. That order makes the concepts build naturally.

## Normal FAQ

### Is stack vs heap a CLR topic or C# topic?

Both. C# code creates values and references, but the CLR runtime manages execution, object allocation, stack frames, heap memory, and garbage collection.

### Why are DB connections discussed with unmanaged memory?

Database connections use external resources such as sockets and OS handles. The managed object is controlled by .NET, but the underlying resource should be released deterministically using `Dispose`/`using`.

## Revision Order

1. [RuntimeExecution.md](RuntimeExecution.md)
2. [Memory.md](Memory.md)
3. [GarbageCollection.md](GarbageCollection.md)
4. [ManagedUnmanagedMemory.md](ManagedUnmanagedMemory.md)
