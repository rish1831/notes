# DI Fundamentals

## Quick Lookup

### What problem does DI solve?

Short version: It removes tight coupling by moving dependency creation outside the class.

### What is IoC?

Short version: A principle where control of object creation is moved outside the class.

### Is DI the only way to do IoC?

Short version: No. Factories and Service Locator can also do it, but constructor DI is preferred in modern .NET.

### Why is DI testable?

Short version: Tests can inject mocks/fakes instead of real databases, email providers, or payment gateways.

## Most Important FAQ

### What problem does DI solve?

Dependency Injection solves tight coupling. Instead of a class creating dependencies with `new`, it receives them from outside, usually through constructor injection.

This means business logic can depend on abstractions instead of concrete implementations. Implementations can be swapped without changing the consumer, and tests can inject mocks or fakes without invoking real infrastructure like databases, email providers, or payment gateways.

### What is IoC?

Inversion of Control is a design principle where the responsibility of creating and managing dependencies is moved outside the class.

Without IoC:

```csharp
public class UserService
{
    private readonly EmailService _emailService = new EmailService();
}
```

With DI:

```csharp
public class UserService
{
    private readonly IEmailService _emailService;

    public UserService(IEmailService emailService)
    {
        _emailService = emailService;
    }
}
```

## Normal FAQ

### How do I remember IoC vs DI vs DI container?

```text
IoC answers: Who controls object creation?
DI answers: How do we provide dependencies?
DI container answers: Who creates and injects them in ASP.NET Core?
```

### Is DI the only way to achieve IoC?

No. Dependency Injection is the most common way to achieve Inversion of Control, but it is not the only one. Other approaches include factories and Service Locator.

In modern .NET applications, constructor-based Dependency Injection is preferred because it makes dependencies explicit, improves testability, and keeps code loosely coupled.

```text
IoC
|
+-- Dependency Injection  (preferred)
+-- Service Locator       (usually avoided)
+-- Factory Pattern       (useful when runtime choice is needed)
```

## Example: Factory As Another IoC Approach

```csharp
public class UserService
{
    private readonly IMessageFactory _factory;

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
```

The factory decides which implementation to create. Factories are useful when implementation choice depends on runtime conditions.

## Interview Answer

Dependency Injection is a design pattern that reduces tight coupling by making classes depend on abstractions rather than concrete implementations. Instead of creating dependencies with `new`, dependencies are injected from outside, usually through constructor injection. In ASP.NET Core, the built-in DI container manages object creation and injects required dependencies based on registered services. This improves maintainability, extensibility, and testability, and it helps achieve Inversion of Control.
