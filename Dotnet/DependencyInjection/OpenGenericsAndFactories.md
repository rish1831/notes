# Open Generics And Factories In DI

## Quick Lookup

### Open generic

Short version: Register `IRepository<>` once and let DI close it for `User`, `Order`, etc.

### Closed generic

Short version: A concrete requested type like `IRepository<User>`.

### Factory

Short version: Chooses which implementation is needed at runtime.

### DI container

Short version: Creates the implementation selected by the factory.

## Most Important FAQ

### What are open generics in DI?

Open generics let you register a generic service once and let .NET create the correct closed version when needed.

```csharp
public interface IRepository<T>
{
}

public class Repository<T> : IRepository<T>
{
}
```

Instead of this:

```csharp
services.AddScoped<IRepository<User>, Repository<User>>();
services.AddScoped<IRepository<Order>, Repository<Order>>();
services.AddScoped<IRepository<Product>, Repository<Product>>();
```

Register once:

```csharp
services.AddScoped(typeof(IRepository<>), typeof(Repository<>));
```

Now when a class asks for:

```csharp
IRepository<User>
```

DI gives:

```csharp
Repository<User>
```

```text
IRepository<>      = open generic
IRepository<User>  = closed generic
```

## Factory Pattern With DI

Use a factory when the implementation cannot be decided at application startup and depends on runtime information.

```csharp
public interface IMessageService
{
    void Send();
}

public class EmailService : IMessageService
{
    public void Send() => Console.WriteLine("Email");
}

public class SmsService : IMessageService
{
    public void Send() => Console.WriteLine("SMS");
}
```

Register concrete services and the factory:

```csharp
services.AddScoped<EmailService>();
services.AddScoped<SmsService>();
services.AddScoped<IMessageFactory, MessageFactory>();
```

Factory:

```csharp
public interface IMessageFactory
{
    IMessageService Get(string type);
}

public class MessageFactory : IMessageFactory
{
    private readonly IServiceProvider _provider;

    public MessageFactory(IServiceProvider provider)
    {
        _provider = provider;
    }

    public IMessageService Get(string type)
    {
        return type switch
        {
            "Email" => _provider.GetRequiredService<EmailService>(),
            "SMS" => _provider.GetRequiredService<SmsService>(),
            _ => throw new Exception("Invalid type")
        };
    }
}
```

Usage:

```csharp
var service = factory.Get("SMS");
service.Send();
```

Output:

```text
SMS
```

## What Is Happening?

```text
User says "SMS"
  -> Factory chooses SmsService
  -> DI container creates SmsService
  -> Factory returns it
```

## Interview Definition

Factory Pattern is used when the implementation cannot be decided at application startup and instead depends on runtime information. The factory chooses the correct implementation, while the DI container creates and injects the selected service.

## Interview Follow-Ups

### Who creates `SmsService`?

The DI container creates `SmsService`. The factory only chooses which service to request.

### Why not simply inject `IEnumerable<IMessageService>`?

Because `IEnumerable<IMessageService>` resolves all implementations:

```text
EmailService
SmsService
```

Even if the user only wants:

```text
SMS
```

A factory resolves only the required implementation and keeps selection logic in one place.

## Memory Trick

```text
DI answers:      How do I create this object?
Factory answers: Which object should I create?
```
