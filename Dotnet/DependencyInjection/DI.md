# What problem does DI solve?

Dependency Injection solves the problem of tight coupling by removing object creation from a class. Instead of creating dependencies using new, a class receives them from the outside, usually through constructor injection. This allows implementations to be swapped without changing business logic and makes the code easier to maintain and extend.

A major benefit is testability. Although mocking frameworks can create mock objects, a class that creates its own dependencies with new cannot use those mocks. Dependency Injection allows tests to inject mock or fake implementations, making true unit testing possible without invoking real services like email providers, payment gateways, or databases.

Dependency Injection is a design pattern that reduces tight coupling by making classes depend on abstractions rather than concrete implementations. Instead of creating dependencies using new, the dependencies are injected from the outside, usually through constructor injection. In ASP.NET Core, the built-in DI container manages object creation and injects the required dependencies based on the registered services. This makes the code easier to maintain, easier to extend, and much easier to unit test because we can inject mock or fake implementations. It also helps achieve Inversion of Control, where object creation is managed externally instead of by the class itself.

# What is IoC?

Inversion of Control is a design principle where the responsibility of creating and managing dependencies is moved outside the class. Instead of a class controlling its own dependencies using new, an external component provides those dependencies. Dependency Injection is the most common way to implement IoC, and in ASP.NET Core, the built-in DI container automates this process by creating and injecting the required objects.

Easy way to remember

Think of it like this:

IoC answers: Who controls object creation?
DI answers: How do we provide the dependencies?
DI Container answers: Who actually creates and injects them in ASP.NET Core?

Is DI the only way to achieve IoC?

A good answer is:

No. Dependency Injection is the most common way to achieve Inversion of Control, but it's not the only one. Other approaches include the Service Locator pattern and Factory pattern. In modern .NET applications, constructor-based Dependency Injection is preferred because it makes dependencies explicit, improves testability, and results in loosely coupled code.

One thing to remember

Many interviewers ask:

"What is IoC?"

Most candidates answer:

"Dependency Injection."

That's not quite correct.

The correct relationship is:

IoC (Principle)
│
├── Dependency Injection ✅ (Preferred)
├── Service Locator ❌ (Usually avoided)
└── Factory Pattern (Sometimes used)

So IoC is the goal, and Dependency Injection is one technique to achieve that goal.

Another way - Factory Pattern

public class UserService
{
private readonly IMessageFactory \_factory;

    public UserService(IMessageFactory factory)
    {
        _factory = factory;
    }

    public void Register()
    {
        var service = _factory.Create();

        service.Send("Welcome");
    }

}

The factory decides which implementation to create.

Factories are useful when the implementation depends on runtime conditions.

# Transient vs Scoped vs Singleton?

Transient creates a new instance every time the service is requested and is suitable for lightweight, stateless services. Scoped creates one instance per request (or per scope), which is why services like DbContext are typically scoped. Singleton creates a single instance for the entire application lifetime and is used for shared services like caching or configuration. A key rule is to avoid injecting a scoped service into a singleton because the singleton outlives the scoped service, leading to lifetime mismatches and potential errors.

Quick memory trick
Lifetime Number of Instances

# Transient Every resolution (new each time)

# Scoped One per request/scope

# Singleton One per application

---

# What happens if Singleton depends on Scoped?

A singleton should not directly depend on a scoped service because the singleton lives for the entire application lifetime, whereas a scoped service lives only for the duration of a request. If the singleton captures a scoped service, it could end up using a disposed instance after the request ends. ASP.NET Core detects this lifetime mismatch and throws an exception during startup. If a singleton needs access to a scoped service, it should create a scope using IServiceScopeFactory and resolve the scoped service within that scope.

Think of it like this:

## Scoped means “valid only for one request”

`DbContext` is usually scoped.

So for **Request 1**:

```text
Request 1 starts
    ↓
DbContext #1 is created
    ↓
Request 1 ends
    ↓
DbContext #1 is disposed/dead
```

Now if a **Singleton** captured that same `DbContext #1`, the singleton still lives:

```text
CacheService singleton
    ↓
still holding DbContext #1
```

But `DbContext #1` is already dead.

So in **Request 2**, the singleton does **not automatically get a new DbContext**.

It still has the old one:

```text
Request 2 starts
    ↓
CacheService is reused
    ↓
CacheService still has DbContext #1
    ↓
But DbContext #1 was disposed after Request 1
```

So this fails:

```csharp
_dbContext.Users.ToList();
```

Because `_dbContext` points to the old disposed object.

Correct mental model:

```text
Singleton = same object forever
Scoped = new object per request
```

So if singleton stores scoped object, it stores **one request’s object forever**, which is wrong.

That is why ASP.NET Core blocks it.

# what if its really needed

Then don’t inject the scoped service directly into the singleton constructor.

Use one of these options:

Best option: create a scope when needed
public class CacheService
{
private readonly IServiceScopeFactory \_scopeFactory;

    public CacheService(IServiceScopeFactory scopeFactory)
    {
        _scopeFactory = scopeFactory;
    }

    public void RefreshCache()
    {
        using var scope = _scopeFactory.CreateScope();

        var dbContext = scope.ServiceProvider.GetRequiredService<AppDbContext>();

        var users = dbContext.Users.ToList();

        // use data here
    }

}

Here:

Singleton CacheService lives forever

But AppDbContext is created only inside this method

After method ends, scope is disposed

DbContext is also disposed safely
Better design if possible

Instead of singleton depending on DbContext, create a scoped service:

public class UserDataService
{
private readonly AppDbContext \_db;

    public UserDataService(AppDbContext db)
    {
        _db = db;
    }

}

Register:

services.AddScoped<UserDataService>();
services.AddSingleton<CacheService>();

Then let request-level/scoped code call the singleton when needed, not the other way around.

Interview answer

If a singleton really needs a scoped service, we should not inject it directly. Instead, we can inject IServiceScopeFactory, create a scope inside the method, resolve the scoped service from that scope, use it, and dispose the scope. This ensures the scoped dependency is created and disposed correctly.

---

# How does IServiceProvider work?

IServiceProvider is the interface implemented by the .NET Dependency Injection container to resolve registered services at runtime. When a service is requested, it looks up the registration, checks its lifetime (Transient, Scoped, or Singleton), recursively resolves any constructor dependencies, builds the complete object graph, and returns the requested service. Although ASP.NET Core uses IServiceProvider internally, we generally prefer constructor injection over calling it directly because it makes dependencies explicit and improves testability

Question:

# Walk me through exactly what happens inside the DI container from this line until the UserService object is returned.

When GetRequiredService<IUserService>() is called, the DI container first checks the registration for IUserService and finds that it maps to UserService. Then it inspects the constructor of UserService and sees that it needs IRepository and IEmailService. The container resolves those dependencies recursively using their own registrations. While resolving them, it also respects their lifetimes — for example, it reuses scoped instances within the same request, reuses singleton instances for the application lifetime, and creates new transient instances every time. Once all dependencies are ready, it creates the UserService instance and returns it.

# If IRepository is Scoped and IEmailService is Transient, and IUserService is resolved twice in the same HTTP request, what happens to each instance?

The DI container first checks whether an instance of IUserService already exists in the current scope. Since it is scoped, if it has already been created during the current request, the same instance is returned. Otherwise, it creates one. While creating it, it resolves its constructor dependencies. IRepository is also scoped, so the same repository instance is reused within the request. IEmailService is transient, so a new instance is created each time it is resolved.

# Interview follow-up (this is a real one)

Suppose you have:

public class UserService
{
public UserService(
IRepository repository1,
IRepository repository2)
{
}
}

IRepository is registered as Scoped.

Question:

Will repository1 and repository2 be the same object or different objects? Why?

Not quite. This is a very common interview trick.

The correct answer is:

> **They will be the same object.**

Here's why.

Suppose:

```csharp
services.AddScoped<IRepository, Repository>();
```

and

```csharp
public class UserService
{
    public UserService(
        IRepository repository1,
        IRepository repository2)
    {
        Console.WriteLine(ReferenceEquals(repository1, repository2));
    }
}
```

When the DI container starts creating `UserService`:

1. It sees `repository1`.
2. It checks the current scope.
3. No `IRepository` exists yet.
4. It creates **Repository #1**.

Now it moves to `repository2`.

5. It again checks the current scope.
6. It finds that **Repository #1** already exists for this request.
7. It injects the **same instance**.

So internally:

```text
Request 1

Scope
│
├── IRepository → Repository #1
└── IUserService → UserService
                  │
                  ├── repository1 ───┐
                  └── repository2 ───┘
```

Both point to the same object.

If you do:

```csharp
Console.WriteLine(object.ReferenceEquals(repository1, repository2));
```

Output:

```text
True
```

---

### Now the follow-up

# What if `IRepository` were registered as **Transient**?

```csharp
services.AddTransient<IRepository, Repository>();
```

Would `repository1` and `repository2` now be the same or different? Why?

Now they will be **different objects**.

Here's why.

Suppose you register:

```csharp
services.AddTransient<IRepository, Repository>();
```

and your constructor is:

```csharp
public class UserService
{
    public UserService(
        IRepository repository1,
        IRepository repository2)
    {
        Console.WriteLine(ReferenceEquals(repository1, repository2));
    }
}
```

The DI container resolves the constructor parameter by parameter.

### Resolving `repository1`

- Looks up `IRepository`.
- Lifetime = **Transient**.
- Creates **Repository #1**.

### Resolving `repository2`

- Looks up `IRepository` again.
- Lifetime = **Transient**.
- Creates **Repository #2**.

So:

```text
UserService
     │
     ├── repository1 → Repository #1
     │
     └── repository2 → Repository #2
```

`ReferenceEquals(repository1, repository2)` returns:

```text
False
```

because every resolution of a transient service creates a new instance.

---

## Here's a table to remember

| Lifetime      | `repository1` vs `repository2` (same constructor) |
| ------------- | ------------------------------------------------- |
| **Singleton** | Same instance ✅                                  |
| **Scoped**    | Same instance (within the same request) ✅        |
| **Transient** | Different instances ❌                            |

---

## One more interview trick (very common)

Suppose:

```csharp
services.AddTransient<IRepository, Repository>();
services.AddScoped<IUserService, UserService>();
```

and you call:

```csharp
var user1 = provider.GetRequiredService<IUserService>();
var user2 = provider.GetRequiredService<IUserService>();
```

**Question:**

How many `UserService` objects are created?
How many `Repository` objects are created?

Think about it before answering. It's a favorite interview question because it tests whether you understand that **the lifetime of the parent service affects when its dependencies are resolved**.

The key rule

A transient service is created every time it is resolved.

But if it's a dependency of a scoped service, it's only resolved when the scoped service is first constructed. Returning the same scoped service later does not recreate its transient dependencies.

This distinction is another favorite interview topic because it tests whether you understand dependency resolution timing, not just service lifetimes.

### Summary Rule (Easy to Remember)

The lifetime of a dependency matters **only when the parent object is being created**.

Example:

```csharp
services.AddScoped<IUserService, UserService>();
services.AddTransient<IRepository, Repository>();
```

```csharp
var user1 = provider.GetRequiredService<IUserService>();
var user2 = provider.GetRequiredService<IUserService>();
```

**What happens?**

1. `user1` is requested.
2. No scoped `UserService` exists, so the DI container creates one.
3. While creating it, it resolves `IRepository` → creates **Repository #1** (Transient).
4. `user2` is requested.
5. Since `UserService` is scoped, the container returns the **same `UserService` instance**.
6. It does **not** create another `Repository` because it isn't constructing a new `UserService`.

**Result:**

- ✅ `UserService` → **1 instance**
- ✅ `Repository` → **1 instance**

---

## Interview Rule

- **Singleton** → Created once for the application.
- **Scoped** → Created once per request/scope.
- **Transient** → Created every time the DI container **needs to resolve it**.

**Important:** A transient dependency is **not recreated** every time you use the parent service. It is recreated **only when the parent service itself is being constructed**.

That's a subtle distinction that interviewers often test.
