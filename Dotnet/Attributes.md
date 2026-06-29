# Attributes in C# and .NET

## Quick revision

| Topic | Remember |
|---|---|
| Attribute | Declarative metadata attached to a program element |
| Storage | Written into assembly metadata at compilation |
| Base type | Every attribute derives from `System.Attribute` |
| Reading | Usually inspected through reflection by frameworks or tools |
| Naming | `[Serializable]` is shorthand for `[SerializableAttribute]` |
| Parameters | Constructor arguments are positional; properties are named |
| Behavior | An attribute does nothing unless code or tooling reads it |

## What are attributes?

Attributes are labels we attach to code to provide extra information. A compiler or framework can read that information and decide what to do—for example, mark a method as obsolete, define an API route, or validate a property.

```csharp
[Obsolete("Use ProcessAsync instead.")]
public void Process()
{
}
```

Here, `ObsoleteAttribute` tells the compiler that callers should no longer use `Process`. The compiler reads the metadata and produces a warning.

Attributes are classes derived from `System.Attribute`. The `Attribute` suffix can be omitted when applying one.

```csharp
[Obsolete]
// Equivalent to [ObsoleteAttribute]
```

## Why are attributes useful?

Attributes separate metadata from execution logic. Frameworks can inspect that metadata and apply behavior consistently.

Common uses include:

- Compiler instructions such as `[Obsolete]`.
- Test discovery such as `[Fact]` or `[Test]`.
- ASP.NET routing and authorization.
- Model validation.
- Serialization configuration.
- ORM mapping.
- Dependency injection and code generation.

## Applying attributes

```csharp
[Serializable]
public class User
{
    [Required]
    [StringLength(100)]
    public string Name { get; set; } = string.Empty;

    [Obsolete("Use GetDisplayName instead.")]
    public string GetName() => Name;
}
```

Multiple attributes can be written separately or in one attribute list.

```csharp
[Required, StringLength(100)]
public string Name { get; set; } = string.Empty;
```

## Positional and named arguments

Constructor arguments are positional and must be supplied in constructor order. Public settable properties or fields can be supplied as named arguments.

```csharp
[Audit("OrderCreated", IncludeRequestBody = true)]
public void CreateOrder()
{
}
```

- `"OrderCreated"` is a positional constructor argument.
- `IncludeRequestBody = true` is a named property argument.

Attribute arguments must be compile-time-compatible metadata values, such as primitives, strings, `Type`, enums, or arrays of supported types. Arbitrary runtime objects cannot be passed.

## Creating a custom attribute

```csharp
[AttributeUsage(
    AttributeTargets.Method,
    AllowMultiple = false,
    Inherited = true)]
public sealed class AuditAttribute : Attribute
{
    public string EventName { get; }
    public bool IncludeRequestBody { get; set; }

    public AuditAttribute(string eventName)
    {
        EventName = eventName;
    }
}
```

Usage:

```csharp
public class OrderService
{
    [Audit("OrderCreated", IncludeRequestBody = true)]
    public void CreateOrder()
    {
    }
}
```

`AttributeUsage` defines how the custom attribute can be applied:

- `AttributeTargets.Method` restricts it to methods.
- `AllowMultiple` controls whether it can appear more than once on one target.
- `Inherited` controls whether derived types or overriding members can inherit it during attribute lookup.

## Reading attributes with reflection

An attribute does not execute itself. A compiler, framework, library, source generator, or your own code must inspect it and act on it.

```csharp
using System.Reflection;

MethodInfo method = typeof(OrderService)
    .GetMethod(nameof(OrderService.CreateOrder))!;

AuditAttribute? audit = method.GetCustomAttribute<AuditAttribute>();

if (audit is not null)
{
    Console.WriteLine(audit.EventName);
    Console.WriteLine(audit.IncludeRequestBody);
}
```

In production, middleware, an interceptor, or a framework component would normally inspect the metadata rather than each business method reading its own attribute.

## Practical ASP.NET Core examples

### Routing and authorization

```csharp
[ApiController]
[Route("api/orders")]
public class OrdersController : ControllerBase
{
    [HttpGet("{id:int}")]
    [Authorize(Roles = "Admin")]
    public IActionResult Get(int id)
    {
        return Ok(id);
    }
}
```

ASP.NET Core reads these attributes to discover controllers, build routes, and enforce authorization.

### Model validation

```csharp
public class CreateUserRequest
{
    [Required]
    [EmailAddress]
    public string Email { get; set; } = string.Empty;
}
```

The validation system reads the attributes and adds errors when the model does not satisfy them.

## Attribute targets

An attribute can explicitly target a particular generated element when the location might be ambiguous.

```csharp
[assembly: System.Reflection.AssemblyMetadata("Environment", "Production")]

public record User(
    [property: Required] string Name,
    [param: Required] string Email);
```

Common target specifiers include `assembly`, `module`, `field`, `property`, `method`, `param`, `return`, and `type`.

## Reflection cost and alternatives

Repeated reflection can add overhead in hot paths. Frameworks commonly scan metadata once and cache the result. For highly performance-sensitive or ahead-of-time compiled applications, source generators can inspect attributes during compilation and generate code that avoids runtime reflection.

Use attributes when metadata is declarative and cross-cutting. Prefer normal methods, interfaces, or configuration when behavior must be explicit, dynamic, dependency-driven, or easy to trace through ordinary control flow.

## Attributes vs other approaches

| Approach | Best suited for |
|---|---|
| Attribute | Static declarative metadata attached to code |
| Interface | A behavioral contract implemented by a type |
| Configuration | Values that may vary by environment or deployment |
| Convention | Applying behavior based on naming or structure |
| Method call | Explicit behavior visible in normal control flow |

## Common pitfalls

- Assuming an attribute performs behavior by itself.
- Using reflection repeatedly instead of caching metadata.
- Hiding important business flow behind too much attribute-driven magic.
- Putting environment-specific or secret values in attributes.
- Forgetting to constrain a custom attribute with `AttributeUsage`.
- Misunderstanding `Inherited`; inheritance behavior also depends on how reflection is queried.
- Treating attributes as a replacement for interfaces or dependency injection.

## Common SSE interview follow-ups

### Are attributes instantiated when the assembly loads?

Not normally. Attribute data is stored in assembly metadata. Attribute objects are generally created when reflection APIs request them. APIs such as `CustomAttributeData` can inspect constructor and named arguments without creating the attribute instance.

### Can attributes contain business logic?

An attribute class can contain methods, but the runtime will not call them automatically. A framework or application component must discover the attribute and invoke the related behavior. Attributes should usually remain lightweight metadata containers.

### Can attribute values change at runtime?

The metadata compiled into the assembly is fixed. Reflection may return attribute instances whose mutable properties can be changed in memory, but that does not modify the assembly metadata or other attribute instances returned later.

### How are attributes different from annotations in other languages?

They serve a similar metadata purpose. In .NET, attributes are actual classes derived from `System.Attribute`, encoded in assembly metadata and exposed through the reflection APIs.

## Concise interview answer

An attribute is declarative metadata attached to a .NET program element and stored in assembly metadata. Attributes derive from `System.Attribute` and are commonly consumed by compilers, frameworks, reflection code, or source generators. They do not perform behavior automatically; some component must inspect them and act on their data. Custom attributes are created by deriving from `Attribute` and should normally use `AttributeUsage` to define valid targets, multiplicity, and inheritance.
