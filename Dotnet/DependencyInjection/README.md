# Dependency Injection Lookup

Use this folder when revising DI interview topics. The old long DI note has been split into focused files so each topic is easier to find.

## Topics

| Topic | File | Must revise |
| --- | --- | --- |
| DI, IoC, and why DI exists | [Fundamentals.md](Fundamentals.md) | Tight coupling, IoC relationship, constructor injection |
| Lifetimes | [Lifetimes.md](Lifetimes.md) | Transient vs Scoped vs Singleton, scoped inside singleton, parent/dependency timing |
| Service resolution | [ServiceResolution.md](ServiceResolution.md) | `IServiceProvider`, object graph creation, missing registrations |
| Multiple implementations | [MultipleImplementations.md](MultipleImplementations.md) | Last registration wins, `IEnumerable<T>`, lifetime behavior |
| Open generics and factories | [OpenGenericsAndFactories.md](OpenGenericsAndFactories.md) | `IRepository<>`, factory selection vs DI creation |
| When to use or avoid DI | [WhenToUseDI.md](WhenToUseDI.md) | Services vs POCOs/DTOs/value objects |
| `GetService` vs `GetRequiredService` | [ServiceProviderApis.md](ServiceProviderApis.md) | Optional vs mandatory service resolution |

## Most Important FAQ

### What is the shortest interview answer for DI?

Dependency Injection removes object creation from a class. A class depends on abstractions and receives dependencies from outside, usually through constructor injection. This reduces coupling, improves testability, and lets ASP.NET Core manage object lifetimes through the DI container.

### What is the difference between IoC and DI?

IoC is the principle: object creation/control is moved outside the class.

DI is one technique to achieve IoC: dependencies are provided from the outside.

The DI container is the framework component that creates and injects registered services.

### What are the three lifetimes?

#### Transient

Short version: New instance every resolution.

Common use: Lightweight stateless services.

#### Scoped

Short version: One instance per request/scope.

Common use: `DbContext` and repositories.

#### Singleton

Short version: One instance for the whole application lifetime.

Common use: Configuration, caches, and shared stateless services.

### What is the most common lifetime mistake?

Injecting a scoped service into a singleton. The singleton lives forever, but the scoped service is valid only for one request/scope.

## Normal FAQ

### Should everything be registered in DI?

No. Register services that need dependencies, lifetime management, or abstraction. Do not register simple DTOs, entities, primitive values, short-lived collections, or objects that are easier to create directly.

### Is `IServiceProvider` bad?

Not always, but constructor injection is preferred because dependencies are explicit and testable. Use `IServiceProvider` carefully for factories or scope creation scenarios.

### When does DI create objects?

Usually when a service is first resolved, not when the app starts. Startup validation can catch some errors early if enabled.
