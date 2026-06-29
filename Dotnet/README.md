# .NET SSE Revision Notes

Use this README as the lookup page. For revision, open the topic file, read the Most Important FAQ first, then use the examples and follow-ups.

## Fast Lookup

| Topic | File | Key questions |
| --- | --- | --- |
| Async and concurrency | [AsyncAwait.md](AsyncAwait.md) | `async` internals, `Task` vs `Thread`, deadlocks, `ConfigureAwait`, cancellation |
| Delegates and events | [DelegatesAndEvents.md](DelegatesAndEvents.md) | Delegates, events, `Action`, `Func`, variance, multicast delegates |
| Dependency Injection | [DependencyInjection/README.md](DependencyInjection/README.md) | DI vs IoC, lifetimes, factories, multiple implementations, service resolution |
| CLR | [CLR/README.md](CLR/README.md) | JIT, IL, stack/heap, GC, LOH, managed/unmanaged memory |
| Extension methods | [ExtensionMethods.md](ExtensionMethods.md) | Syntax, resolution rules, namespace scope, generic extensions |
| Attributes | [Attributes.md](Attributes.md) | Metadata, custom attributes, reflection, ASP.NET Core attributes |
| Reflection | [Reflection.md](Reflection.md) | Runtime metadata, dynamic invocation, costs, alternatives |
| Expression trees | [ExpressionTrees.md](ExpressionTrees.md) | Code as data, LINQ providers, expression vs delegate |
| Type system | [TypeSystem.md](TypeSystem.md) | Boxing, structs/classes, records, nullable reference types |
| OOP | [ObjectOrientedProgramming.md](ObjectOrientedProgramming.md) | Abstract class vs interface, virtual/abstract, override vs new, sealed |
| Equality | [Equality.md](Equality.md) | `==`, `Equals`, `GetHashCode`, `IEquatable<T>` |
| Resource management | [ResourceManagement.md](ResourceManagement.md) | `IDisposable`, finalizers, deterministic cleanup |
| Question bank | [Questions.md](Questions.md) | Topic-wise interview question checklist |

## Most Important FAQ

### What should I revise first for .NET interviews?

Start with async, DI lifetimes, CLR memory/GC, equality, and OOP basics. These are the highest-frequency topics and they connect to many follow-up questions.

### How should I revise one topic?

1. Read the quick lookup table.
2. Answer the Most Important FAQ without looking.
3. Recreate the smallest code snippet from memory.
4. Answer the follow-ups.
5. Say one production pitfall aloud.

### How should I handle large topics?

Large topics are split into folders or separate files. For example, Dependency Injection now has one file per subtopic under [DependencyInjection](DependencyInjection/README.md).

## Normal FAQ

### Where are CLR notes?

Open [CLR/README.md](CLR/README.md). It links memory, runtime execution, garbage collection, and managed/unmanaged memory notes.

### Where are DI notes?

Open [DependencyInjection/README.md](DependencyInjection/README.md). The original [DependencyInjection/DI.md](DependencyInjection/DI.md) file is now a short entry point.

### Where should new notes go?

Create one file per topic unless the note is very small. If a file becomes long and covers unrelated questions, split it into a folder with its own `README.md`.

## Revision Priority

| Priority | Topics |
| --- | --- |
| Must know | Async, DI, CLR memory, GC, equality, OOP |
| Strong follow-up topics | Reflection, attributes, extension methods, resource management |
| Advanced/nice to know | Expression trees, deep runtime details |

## Suggested Revision Method

1. Read the quick-revision table or concise answer first.
2. Explain the topic aloud without looking at the note.
3. Write the smallest example from memory.
4. Answer the follow-up questions and name at least two pitfalls.
5. Connect the concept to a production scenario you have encountered.
