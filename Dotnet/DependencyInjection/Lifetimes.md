# DI Lifetimes

## Quick Lookup

### Transient

Short version: New instance every resolution.

Use for: Lightweight, stateless services.

### Scoped

Short version: One instance per request/scope.

Use for: `DbContext`, repositories, and request-level services.

### Singleton

Short version: One instance for the whole application.

Use for: Cache, configuration, and shared stateless services.

## Most Important FAQ

### What is the difference between Transient, Scoped, and Singleton?

Transient creates a new instance every time the service is requested.

Scoped creates one instance per request or scope.

Singleton creates one instance for the entire application lifetime.

### What happens if a singleton depends on a scoped service?

A singleton should not directly depend on a scoped service because the singleton lives for the entire application lifetime, while the scoped service lives only for one request/scope.

If a singleton captures a scoped dependency, it may keep using a disposed request object. ASP.NET Core detects this lifetime mismatch and can throw an exception when scope validation is enabled.

```text
Singleton = same object forever
Scoped    = new object per request/scope
```

So if a singleton stores a scoped object, it stores one request's object forever, which is wrong.

## Normal FAQ

### What does scoped mean?

Scoped means valid only inside one scope. In ASP.NET Core web apps, one HTTP request usually equals one scope.

```text
Request 1 starts
  -> DbContext #1 is created
Request 1 ends
  -> DbContext #1 is disposed

Request 2 starts
  -> DbContext #2 is created
```

### What if a singleton really needs a scoped service?

Do not inject the scoped service directly into the singleton constructor. Create a scope only when needed.

```csharp
public class CacheService
{
    private readonly IServiceScopeFactory _scopeFactory;

    public CacheService(IServiceScopeFactory scopeFactory)
    {
        _scopeFactory = scopeFactory;
    }

    public void RefreshCache()
    {
        using var scope = _scopeFactory.CreateScope();

        var dbContext = scope.ServiceProvider.GetRequiredService<AppDbContext>();
        var users = dbContext.Users.ToList();

        // Use data here.
    }
}
```

Here `CacheService` lives forever, but `AppDbContext` is created inside a temporary scope and disposed when that scope ends.

### What is a better design if possible?

Move database work into a scoped service and let scoped/request-level code coordinate with the singleton.

```csharp
public class UserDataService
{
    private readonly AppDbContext _db;

    public UserDataService(AppDbContext db)
    {
        _db = db;
    }
}

services.AddScoped<UserDataService>();
services.AddSingleton<CacheService>();
```

## Interview Follow-Ups

### If `IRepository` is scoped and injected twice into the same constructor, same object or different objects?

Same object.

```csharp
services.AddScoped<IRepository, Repository>();

public class UserService
{
    public UserService(IRepository repository1, IRepository repository2)
    {
        Console.WriteLine(ReferenceEquals(repository1, repository2));
    }
}
```

When the container creates `UserService`, it checks the current scope. The first `IRepository` creates `Repository #1`; the second `IRepository` reuses the same scoped instance.

```text
Output: True
```

### What if `IRepository` is transient?

Different objects.

```csharp
services.AddTransient<IRepository, Repository>();
```

The DI container resolves constructor parameters one by one. Since transient means new instance every resolution, `repository1` gets `Repository #1` and `repository2` gets `Repository #2`.

```text
Output: False
```

### What if a scoped parent has a transient dependency and the parent is resolved twice in the same request?

```csharp
services.AddScoped<IUserService, UserService>();
services.AddTransient<IRepository, Repository>();

var user1 = provider.GetRequiredService<IUserService>();
var user2 = provider.GetRequiredService<IUserService>();
```

Result:

```text
UserService -> 1 instance
Repository  -> 1 instance
```

Why? The transient dependency is created only when the scoped parent object is constructed. Returning the same scoped parent later does not recreate its constructor dependencies.

## Memory Tricks

```text
Transient -> every resolution
Scoped    -> every request/scope
Singleton -> once for the app
```

The lifetime of a dependency matters only when the parent object is being created.
