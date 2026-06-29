# Garbage Collection

Use this page for .NET GC fundamentals, generations, the Large Object Heap, and memory leaks.

## Most Important FAQ

### What does GC do?

Garbage Collection automatically reclaims managed heap memory for objects that are no longer reachable.

### What causes memory leaks in .NET?

Objects stay alive when something still references them, such as static collections, events, timers, caches, or long-lived closures.

## Quick Map

### GC

Short version: Automatically reclaims heap memory from unreachable objects.

### Gen 0

Short version: New, usually short-lived objects.

### Gen 1

Short version: Middle generation for objects that survived one collection.

### Gen 2

Short version: Long-lived objects. Gen 2 collections are more expensive because they scan more memory.

### LOH

Short version: Large Object Heap. Used for large objects, usually 85,000 bytes or more, and collected with Gen 2.

### Memory leak

Short version: An object remains reachable longer than intended, so the GC cannot collect it.

## What Is GC?

The garbage collector (GC) in .NET automatically reclaims memory from objects that your code no longer needs. Gen 0 is for short-lived objects; if they survive, they move up to Gen 1, and long-lived objects end up in Gen 2. The GC tries to be efficient: Gen 0 collections happen often but are fast. Gen 2 collections happen less frequently because they involve more memory. The GC keeps things tidy so you do not have to manually free memory.

Objects move up through generations based on how long they survive. When the GC runs a Gen 0 collection, it reclaims short-lived objects. Any object that survives, meaning it is still in use, gets promoted to Gen 1. Similarly, if it is still alive after a Gen 1 collection, it moves to Gen 2.

Gen 2 is considered for long-lived objects, like application-level data or large caches. The reason Gen 2 is not collected as often is that Gen 2 collections are more expensive: they need to scan more memory and check objects that have been around longer. By collecting Gen 0 frequently and Gen 2 less often, the GC balances performance and efficiency.

There are no hard rules based on object type. It is not about what type something is; it is about how long it lives. Everything starts in Gen 0. If an object survives a garbage collection because something is still using it, it gets promoted step-by-step. Whether it is a class, an array, or any object, it starts in Gen 0. If it is short-lived, it is collected early; if it sticks around through multiple collections, it eventually reaches Gen 2.

The garbage collector only manages the heap, where objects and reference types live. The stack is simpler: it stores value types and method call frames, and it is managed automatically as methods start and end. The GC's job is to reclaim memory from objects on the heap once they are no longer needed, while the stack cleans up on its own as methods finish executing.

## Gen 0, Gen 1, Gen 2

Gen 0 is for the newest objects, typically short-lived. Gen 1 is a buffer zone for objects that survive a bit longer. Gen 2 is for long-lived objects, things like static data or large caches. The GC collects Gen 0 frequently, and Gen 2 less often, because it is assumed that objects in Gen 2 are more stable.

The big picture is that this generational approach makes memory management efficient by focusing more on cleaning up what is likely to be short-lived.

### Static Data And Application Lifetime

When we mention "static data," we mean objects or data tied to the lifetime of your application rather than individual methods or instances. In C#, static fields or properties belong to a type itself, not to a specific object. They stay in memory as long as the program runs. So when the garbage collector manages long-lived objects, it often means those that stick around for the entire lifetime of the application.

By the lifetime of your application, we simply mean from the moment your program starts running until it ends. Some data, like static fields or singletons, stay in memory for that entire period. In other words, these kinds of objects often live as long as your program is running and are only cleaned up when your whole app shuts down.

## Large Object Heap

The Large Object Heap, or LOH, is a special part of the managed heap in .NET where large objects, typically 85,000 bytes or more, are allocated. Since these large objects are more expensive to move around, the LOH does not compact memory as often as the rest of the heap. Instead, large objects stay put until they are no longer needed. The LOH is collected during full Gen 2 garbage collections, so large objects can persist a bit longer before cleanup.

It is more of a practical threshold: if an object is large enough, the runtime allocates it on the LOH so it does not need to be moved around often. It is a strategy to handle large chunks of memory efficiently, and it is a conceptual boundary the runtime uses behind the scenes.

The Large Object Heap is part of the managed heap, not a separate heap. Objects larger than approximately 85 KB are allocated directly on the LOH instead of Gen 0. The LOH is treated as part of Generation 2, so these objects are collected during full Gen 2 garbage collections rather than being promoted through Gen 0 and Gen 1.

The Large Object Heap is part of the managed heap, not a separate heap. Objects larger than approximately 85 KB are allocated directly on the LOH instead of Gen 0. The LOH is treated as part of Generation 2, so these objects are collected during full Gen 2 garbage collections rather than being promoted through Gen 0 and Gen 1.

## What Causes Memory Leaks In .NET?

In .NET, memory leaks happen when objects stay alive longer than intended because something is still referencing them. The garbage collector only frees memory when no references remain. Common causes include event handlers or delegates that are not unsubscribed, static references that persist, or long-lived collections holding onto objects. In other words, it is not that the GC fails; it is that the objects still appear to be in use, so the GC leaves them alone.

One key practice is to always unsubscribe from event handlers when you no longer need them, especially if the event source lives longer than the subscriber. Be mindful of static references or long-lived objects, and ensure they do not hold onto data unnecessarily. Use weak references if you need to reference something without preventing it from being collected. Lastly, be careful with large object graphs or caches: clear or release them when they are no longer needed. In short, track object lifetimes, break unneeded references, and let the GC do its job.

## Related CLR Notes

- [CLR Memory](Memory.md)
- [CLR Execution Flow](RuntimeExecution.md)
- [Managed, Unmanaged Memory, And Connection Pooling](ManagedUnmanagedMemory.md)
