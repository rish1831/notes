# .NET SSE Revision Notes

## Topics

1. [Async and Concurrency](AsyncAwait.md)
   - Async state machines, tasks, threads, ThreadPool, contexts, deadlocks, parallelism, cancellation, and common pitfalls.

2. [Delegates and Events](DelegatesAndEvents.md)
   - Delegates, events, `Action`, `Func`, `Predicate`, variance, multicast behavior, and event lifetime issues.

3. [Extension Methods](ExtensionMethods.md)
   - Syntax, resolution rules, generic extensions, API design guidance, limitations, and interview points.

4. [Attributes](Attributes.md)
   - Metadata, reflection, custom attributes, target rules, ASP.NET Core usage, performance, and interview pitfalls.

5. [Reflection](Reflection.md)
   - Runtime metadata inspection, dynamic invocation, use cases, costs, and alternatives.

6. [Expression Trees](ExpressionTrees.md)
   - Code as data, delegates vs expressions, LINQ providers, translation, and limitations.

7. [Type System](TypeSystem.md)
   - Boxing, structs, classes, records, and nullable reference types.

8. [Object-Oriented Programming](ObjectOrientedProgramming.md)
   - Abstract classes, interfaces, virtual and abstract methods, and sealed types.

9. [Equality](Equality.md)
   - `==`, `Equals`, hash codes, `IEquatable<T>`, and equality contracts.

10. [Resource Management](ResourceManagement.md)
    - `IDisposable`, finalizers, deterministic cleanup, and implementation patterns.

11. [CLR](CLR/README.md)
    - Object allocation, CLR execution flow, IL, JIT, garbage collection, LOH, memory leaks, managed/unmanaged resources, and connection pooling.

## Suggested revision method

1. Read the quick-revision table or concise answer first.
2. Explain the topic aloud without looking at the note.
3. Write the smallest example from memory.
4. Answer the follow-up questions and name at least two pitfalls.
5. Connect the concept to a production scenario you have encountered.
