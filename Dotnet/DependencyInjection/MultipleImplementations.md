# Multiple Implementations Of The Same Interface

## Quick Lookup

### Inject `IMessageService`

Short version: The last registered implementation is injected.

### Inject `IEnumerable<IMessageService>`

Short version: All registered implementations are injected in registration order.

### Registered as scoped

Short version: Each implementation is one instance per scope.

### Registered as transient

Short version: Each implementation is created every resolution.

## Most Important FAQ

### What happens when multiple implementations are registered for the same interface?

ASP.NET Core allows multiple implementations of the same interface to be registered.

If a single instance of the interface is injected, the last registered implementation is injected.

If `IEnumerable<T>` is injected, the DI container resolves all registered implementations in the order they were registered.

While resolving them, the container still respects their registered lifetimes.

## Example

```csharp
public interface IMessageService { }

public class EmailService : IMessageService { }

public class SmsService : IMessageService { }
```

Registration:

```csharp
services.AddScoped<IMessageService, EmailService>();
services.AddScoped<IMessageService, SmsService>();
```

## Case 1: Inject A Single Implementation

```csharp
public class NotificationService
{
    public NotificationService(IMessageService service)
    {
    }
}
```

Injected service:

```text
SmsService
```

Reason: the last registered implementation wins.

## Case 2: Inject All Implementations

```csharp
public class NotificationService
{
    public NotificationService(IEnumerable<IMessageService> services)
    {
    }
}
```

Injected:

```text
[
    EmailService,
    SmsService
]
```

Reason: `IEnumerable<T>` resolves every registered implementation in registration order.

## Interview Follow-Ups

### Follow-up 1

```csharp
services.AddScoped<IMessageService, EmailService>();
services.AddScoped<IMessageService, SmsService>();
```

```csharp
public class NotificationService
{
    public NotificationService(IEnumerable<IMessageService> services)
    {
    }
}
```

Question: How many objects are created when `NotificationService` is resolved for the first time in a request?

Answer:

```text
EmailService -> 1
SmsService   -> 1

Total = 2 objects
```

Because both are scoped and this is their first resolution in the current request.

### Follow-up 2

Later in the same HTTP request:

```csharp
var services = provider.GetRequiredService<IEnumerable<IMessageService>>();
```

Question: How many new objects are created?

Answer:

```text
0
```

Because both scoped instances already exist in the current request, so the DI container reuses them.

## Key Rules

- Single interface injection (`IMessageService`) means last registered implementation.
- `IEnumerable<IMessageService>` means all implementations.
- Transient means new instance every resolution.
- Scoped means one instance per request/scope.
- Singleton means one instance per application lifetime.
- The DI container respects lifetime even when resolving multiple implementations.
