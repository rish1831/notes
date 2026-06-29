# When To Use Or Avoid DI

## Quick Lookup

### Use DI for

- Business services
- Repositories
- `DbContext`
- Logging
- Caching
- HTTP clients
- External services

### Avoid DI for

- DTOs
- Entities/value objects
- `List<T>` and `Dictionary<K,V>`
- `StringBuilder`
- Primitive values like `string`, `int`, and `bool`
- Objects with no dependencies
- Temporary objects

## Most Important FAQ

### When would you avoid DI?

Do not avoid DI entirely, but avoid using it for simple objects that have no dependencies, short-lived data objects, or objects whose creation depends heavily on runtime information. In those cases, create the object directly or use a factory.

### When should you use DI?

Use DI for services that benefit from dependency management, lifetime management, abstraction, or testability:

- Business services
- Repositories
- Database contexts
- Logging
- Caching
- HTTP clients
- External services such as email, SMS, and payment providers

## Normal FAQ

### Should POCOs be registered in DI?

Usually no.

```csharp
public class Customer
{
    public string Name { get; set; }
}
```

Instead of:

```csharp
services.AddTransient<Customer>();
```

Create it directly:

```csharp
var customer = new Customer();
```

### Should DTOs or request models be registered in DI?

No.

```csharp
public class CreateUserRequest
{
    public string Name { get; set; }
}
```

DTOs and request models are data objects. They should be created normally, usually by model binding or direct construction.

### What about primitive values?

Do not inject primitive values like this:

```csharp
public UserService(string name)
{
}
```

Use configuration/options, a typed settings object, or pass the value directly where it is needed.

### What about temporary objects?

Create these directly:

```csharp
var sb = new StringBuilder();
var list = new List<int>();
var dto = new UserDto();
```

They are lightweight, short-lived, and do not need lifetime management.

## Interview Answer

I generally use Dependency Injection for application services because it improves maintainability and testability. However, I avoid DI for simple POCOs, DTOs, value objects, primitive values, and temporary objects like lists or `StringBuilder`, since they do not have dependencies or require lifetime management. If object creation depends on runtime conditions, I prefer using a factory instead of trying to make the DI container decide dynamically.

## Interview Follow-Up

### Should `List<string>` be registered in the DI container?

No. `List<string>` is a simple collection with no dependencies or lifecycle to manage. It is lightweight and should be created where it is needed using `new List<string>()`.

DI is intended for services whose creation, lifetime, or dependencies need to be managed by the container.
