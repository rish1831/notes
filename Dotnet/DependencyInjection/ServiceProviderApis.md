# `GetService<T>()` Vs `GetRequiredService<T>()`

## Quick Lookup

### `GetService<T>()`

Short version: Returns `null` if the service is not registered.

Use when: The dependency is optional.

### `GetRequiredService<T>()`

Short version: Throws `InvalidOperationException` if the service is not registered.

Use when: The dependency is mandatory.

## Most Important FAQ

### What is `GetService<T>()`?

It says: give me this service if it is registered.

```csharp
var service = provider.GetService<IEmailService>();
```

If it is not registered:

```text
null
```

So you must check:

```csharp
if (service != null)
{
    service.Send();
}
```

### What is `GetRequiredService<T>()`?

It says: I must have this service. If it does not exist, something is wrong.

```csharp
var service = provider.GetRequiredService<IEmailService>();
```

If it is not registered:

```text
InvalidOperationException:
No service for type 'IEmailService' has been registered.
```

## Which One Should We Use?

Almost always use:

```csharp
GetRequiredService<T>()
```

If your application expects a service to exist, it is better to fail fast than continue with a `null` value.

## Interview Answer

`GetService<T>()` returns the requested service if it is registered; otherwise it returns `null`. `GetRequiredService<T>()` also resolves the service, but if the service is not registered, it throws an `InvalidOperationException`. We generally use `GetRequiredService<T>()` when the dependency is mandatory because it fails fast and avoids null-related errors.

## Memory Trick

```text
GetService         -> optional  -> returns null if missing
GetRequiredService -> mandatory -> throws if missing
```

## Interview Follow-Up

Suppose:

```csharp
public class UserService
{
    public UserService(IEmailService emailService)
    {
    }
}
```

But you forgot to register:

```csharp
services.AddScoped<IEmailService, EmailService>();
```

Question: When will the exception be thrown?

1. During application startup?
2. When `UserService` is first resolved?
3. When `emailService.Send()` is called?

Answer: when `UserService` is first resolved.

The application can start successfully because the DI container does not create every service at startup. When a request resolves `IUserService`, the container tries to construct `UserService`, sees `IEmailService`, cannot resolve it, and throws.

## Advanced Note

ASP.NET Core can validate DI registrations at startup if validation is enabled, for example with `ValidateOnBuild` or `ValidateScopes`. For most interview answers, say the exception is thrown when the service is first resolved.
