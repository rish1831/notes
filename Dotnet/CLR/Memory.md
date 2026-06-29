# CLR Memory

Use this page for the core memory model: object creation, heap vs stack, object references, and allocation failure.

## Quick Map

| Concept | Short version |
| --- | --- |
| Object allocation | `new` creates the object on the managed heap and returns a reference. |
| Reference variable | The variable stores a reference to the heap object, usually on the stack when it is local. |
| Object lifetime | The object remains alive while it is reachable from GC roots. |
| Stack | Stores method frames, parameters, and short-lived local data. |
| Heap | Stores objects and reference-type instances managed by the GC. |

## What Happens When You Create An Object?

**Interview answer:** When `new` is executed in .NET, the CLR first determines the object's size from its type metadata. It allocates memory on the managed heap by advancing a heap pointer, writes the object header, zero-initializes all fields, and then executes the base and derived constructors. The variable receives a reference to the newly created object.

The object remains on the managed heap until it becomes unreachable from GC roots, at which point a future garbage collection can reclaim its memory.

### Allocation Flow

1. CLR reads the type metadata.
2. CLR calculates the object size.
3. CLR allocates memory on the managed heap, usually by advancing a heap pointer.
4. CLR writes the object header, including synchronization and type information.
5. CLR zero-initializes all fields.
6. CLR runs base constructors and then derived constructors.
7. The local variable receives a reference to the object.

### What If Allocation Fails?

If the managed heap does not have enough space:

1. The CLR attempts a garbage collection.
2. It compacts the heap for generations that are compacting.
3. It retries the allocation.
4. If there still is not enough memory, it throws an `OutOfMemoryException`.

## Heap, Stack, And References

**Class instance:** When you create an instance of a class, the class itself is a reference type, so it is allocated on the heap. Any fields, whether value types or other references, are stored alongside the object on the heap. The reference variable pointing to the object lives on the stack or wherever it is scoped, but the class's data lives in the heap.

**Everyday object data:** Since most everyday data is inside class instances, their fields, whether value types or references, end up on the heap with that class. The reference to the class, the pointer-like value, typically lives on the stack if it is a local variable. Once you create an object from a class, its actual data lives in the heap.

```csharp
Person p = new Person();
```

The actual `Person` object is stored on the heap. The variable `p` itself is just a reference, like a pointer, and that reference is typically stored on the stack if it is a local variable. In short: the object's data is on the heap, and the reference to it is on the stack.

In this context, when we say "reference," it is like a pointer: a small piece of data containing a memory address that tells the runtime where the actual object lives on the heap.

## What Lives On The Stack?

On the stack, you find local variables such as value types like integers, floats, or structs, as well as method parameters. The stack is also where the system keeps track of which method is currently running through call frames or stack frames.

Basically, the stack holds quick, short-lived data tied directly to method execution.

The stack is managed automatically as methods start and end. The garbage collector only manages the heap, where objects and reference types live. The GC's job is to reclaim memory from objects on the heap once they are no longer needed, while the stack cleans up on its own as methods finish executing.

## Related CLR Notes

- [CLR Execution Flow](RuntimeExecution.md)
- [Garbage Collection](GarbageCollection.md)
- [Managed, Unmanaged Memory, And Connection Pooling](ManagedUnmanagedMemory.md)
