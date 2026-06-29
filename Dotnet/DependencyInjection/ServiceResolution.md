# Service Resolution

## Quick Lookup

### What resolves services?

Short version: `IServiceProvider`.

### What does the container build?

Short version: A complete object graph.

### Are all services created at startup?

Short version: Usually no. Services are created when first resolved.

### What is preferred over direct `IServiceProvider` calls?

Short version: Constructor injection.

## Most Important FAQ

### How does `IServiceProvider` work?

`IServiceProvider` is the interface implemented by the .NET DI container to resolve registered services at runtime.

When a service is requested, the container:

1. Looks up the registration.
2. Checks the lifetime.
3. Inspects the implementation constructor.
4. Recursively resolves constructor dependencies.
5. Builds the complete object graph.
6. Returns the requested service.

Although ASP.NET Core uses `IServiceProvider` internally, constructor injection is usually preferred because it makes dependencies explicit and improves testability.

### Walk me through service resolution for `UserService`.

When `GetRequiredService<IUserService>()` is called, the DI container checks the registration for `IUserService` and finds `UserService`. It inspects the `UserService` constructor and sees that it needs `IRepository` and `IEmailService`.

The container resolves those dependencies recursively using their own registrations. While resolving them, it respects lifetimes:

- Scoped instances are reused within the same request/scope.
- Singleton instances are reused for the application lifetime.
- Transient instances are created every time they are resolved.

Once all dependencies are ready, it creates and returns the `UserService` instance.

## Normal FAQ

### If `IUserService` is resolved twice in the same request, what happens?

Assume:

- `IUserService` is scoped.
- `IRepository` is scoped.
- `IEmailService` is transient.

The first resolution creates `UserService`, reuses or creates the scoped `IRepository`, and creates a transient `IEmailService`.

The second resolution in the same request returns the same scoped `UserService`. It does not recreate constructor dependencies because the parent service is not being constructed again.

### When is a missing dependency exception thrown?

Usually when the service is first resolved, not when the app starts.

```csharp
services.AddScoped<IUserService, UserService>();

// Forgot this:
// services.AddScoped<IEmailService, EmailService>();
```

```csharp
public class UserService : IUserService
{
    public UserService(IEmailService emailService)
    {
    }
}
```

When the first request resolves `IUserService`, the container tries to build `UserService`, sees `IEmailService`, cannot find a registration, and throws.

```text
InvalidOperationException:
Unable to resolve service for type 'IEmailService'
while attempting to activate 'UserService'.
```

## Advanced Note

ASP.NET Core can validate DI registrations at startup if validation is enabled, for example with `ValidateOnBuild` or `ValidateScopes`. In interviews, the common expected answer is still: the exception is thrown when the service is first resolved.
