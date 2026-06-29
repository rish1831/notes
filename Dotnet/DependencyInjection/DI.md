# Dependency Injection

This file is kept as the main entry point for Dependency Injection notes. The long DI note has been split into one file per topic for easier revision.

## Quick Lookup

| Need to revise | Open |
| --- | --- |
| What DI solves, IoC, constructor injection | [Fundamentals.md](Fundamentals.md) |
| Transient, scoped, singleton, lifetime mismatch | [Lifetimes.md](Lifetimes.md) |
| `IServiceProvider` and object graph creation | [ServiceResolution.md](ServiceResolution.md) |
| Multiple implementations of same interface | [MultipleImplementations.md](MultipleImplementations.md) |
| Open generics and factory pattern | [OpenGenericsAndFactories.md](OpenGenericsAndFactories.md) |
| When to use or avoid DI | [WhenToUseDI.md](WhenToUseDI.md) |
| `GetService<T>()` vs `GetRequiredService<T>()` | [ServiceProviderApis.md](ServiceProviderApis.md) |

## Most Important FAQ

### What is DI?

Dependency Injection is a design pattern where a class receives its dependencies from outside instead of creating them with `new`. It reduces tight coupling, improves testability, and lets ASP.NET Core manage object creation and lifetimes.

### What is the interview-safe answer for lifetimes?

- Transient: new instance every resolution.
- Scoped: one instance per request/scope.
- Singleton: one instance for the whole application.

### What should I never forget?

Do not inject scoped services directly into singletons. If a singleton really needs scoped work, create a scope using `IServiceScopeFactory`.

## Revision Order

1. [Fundamentals.md](Fundamentals.md)
2. [Lifetimes.md](Lifetimes.md)
3. [ServiceResolution.md](ServiceResolution.md)
4. [MultipleImplementations.md](MultipleImplementations.md)
5. [OpenGenericsAndFactories.md](OpenGenericsAndFactories.md)
6. [WhenToUseDI.md](WhenToUseDI.md)
7. [ServiceProviderApis.md](ServiceProviderApis.md)
