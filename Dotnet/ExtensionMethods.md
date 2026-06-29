# Extension Methods in C#

## What are extension methods?

Extension methods are static methods that can be called using instance-method syntax. They let you add operations to an existing type without modifying that type, creating a derived type, or recompiling its source code.

They do not actually change the target type. The compiler translates the instance-style call into a normal static method call.

```csharp
public static class StringExtensions
{
    public static bool IsNullOrEmpty(this string? value)
    {
        return string.IsNullOrEmpty(value);
    }
}

bool empty = name.IsNullOrEmpty();

// Conceptually compiled as:
bool empty = StringExtensions.IsNullOrEmpty(name);
```

## How do we create one?

An extension method must:

1. Be declared inside a non-generic, non-nested static class.
2. Be a static method.
3. Use `this` before its first parameter.
4. Be in scope through its namespace.

The first parameter identifies the type being extended.

```csharp
namespace MyApplication.Extensions;

public static class DateTimeExtensions
{
    public static bool IsWeekend(this DateTime date)
    {
        return date.DayOfWeek is DayOfWeek.Saturday or DayOfWeek.Sunday;
    }
}

DateTime date = DateTime.Today;
bool isWeekend = date.IsWeekend();
```

## Why do extension methods exist?

Extension methods let developers add useful behavior when they do not own the target type. They are especially useful for framework types, third-party library types, interfaces, and fluent APIs.

LINQ is the most important framework example. Methods such as `Where`, `Select`, and `OrderBy` are extension methods on `IEnumerable<T>` and related interfaces.

```csharp
IEnumerable<string> names = users
    .Where(user => user.IsActive)
    .OrderBy(user => user.Name)
    .Select(user => user.Name);
```

This style makes pipelines readable while keeping the behavior outside collection classes.

## Practical example: validating a domain value

```csharp
public static class EmailExtensions
{
    public static bool HasValidEmailFormat(this string? email)
    {
        if (string.IsNullOrWhiteSpace(email))
            return false;

        return System.Net.Mail.MailAddress.TryCreate(email, out _);
    }
}

if (!request.Email.HasValidEmailFormat())
{
    return Results.BadRequest("Invalid email address.");
}
```

This can improve readability when the operation naturally describes the value. However, business rules that require dependencies, database access, or substantial domain logic usually belong in a service or domain object instead.

## Extending an interface

Extending an interface makes the method available to every implementation of that interface.

```csharp
public interface IOrder
{
    decimal Subtotal { get; }
    decimal Tax { get; }
}

public static class OrderExtensions
{
    public static decimal GetTotal(this IOrder order)
    {
        ArgumentNullException.ThrowIfNull(order);
        return order.Subtotal + order.Tax;
    }
}
```

This is useful when many implementations share an operation that can be expressed entirely through the interface contract.

## Generic extension methods

Extension methods can declare generic type parameters and constraints.

```csharp
public static class EnumerableExtensions
{
    public static bool None<T>(
        this IEnumerable<T> source,
        Func<T, bool> predicate)
    {
        ArgumentNullException.ThrowIfNull(source);
        ArgumentNullException.ThrowIfNull(predicate);

        return !source.Any(predicate);
    }
}

bool hasNoAdmins = users.None(user => user.IsAdmin);
```

## Method resolution rules

### Instance methods take priority

If the target type has an applicable instance method with the same name, the compiler uses it instead of the extension method.

```csharp
public class Report
{
    public string Format() => "Instance method";
}

public static class ReportExtensions
{
    public static string Format(this Report report) => "Extension method";
}

var report = new Report();
Console.WriteLine(report.Format()); // Instance method
```

An extension method cannot override or replace an instance method. This also means adding a new instance method to a dependency can silently change which method existing source code binds to after recompilation.

### Namespace scope matters

An extension method is available only when its containing namespace is imported or when the static method is called directly.

```csharp
using MyApplication.Extensions;
```

If multiple imported namespaces contain equally applicable extension methods, the call can become ambiguous. Use the static-call form to disambiguate.

```csharp
string result = FirstExtensions.Format(value);
```

## Null behavior

Because an extension method is a static method, it can be invoked when the receiver is `null`. Whether that is safe depends on its implementation.

```csharp
public static bool IsMissing(this string? value)
{
    return string.IsNullOrWhiteSpace(value);
}

string? name = null;
bool missing = name.IsMissing(); // Safe: true
```

For methods that require a non-null receiver, validate it explicitly.

```csharp
public static string FirstCharacter(this string value)
{
    ArgumentNullException.ThrowIfNull(value);
    return value[..1];
}
```

## When should we use extension methods?

Good uses include:

- Adding focused operations to types you cannot modify.
- Providing reusable operations for an interface.
- Building fluent APIs and readable transformation pipelines.
- Keeping small, stateless conversion or helper behavior near the relevant type.
- Adding convenience methods around framework or third-party types.

Avoid them when:

- You own the type and the behavior belongs naturally inside it.
- The method hides expensive I/O or surprising side effects behind innocent syntax.
- The behavior requires injected dependencies or mutable global state.
- The extension class becomes an unstructured collection of unrelated helpers.
- The method exposes or depends on private members; extension methods can access only the target's accessible members.

## Extension methods vs alternatives

| Approach | Best suited for |
|---|---|
| Instance method | Core behavior of a type you own |
| Extension method | Stateless convenience behavior around an existing contract |
| Inheritance | A true substitutable specialization with overridable behavior |
| Service | Operations involving dependencies, orchestration, I/O, or business workflows |
| Decorator | Adding runtime behavior while preserving an interface |

## Important interview points

- Extension methods are compile-time syntactic sugar over static method calls.
- They do not modify the target type or participate in virtual dispatch.
- An applicable instance method always wins over an extension method.
- The containing namespace must be in scope.
- They can extend interfaces, generic types, value types, and sealed classes.
- They cannot access private members of the extended type.
- They can technically receive `null` because the receiver is an ordinary parameter.
- Overusing them can pollute IntelliSense and make ownership of behavior unclear.

## Concise interview answer

An extension method is a static method whose first parameter uses the `this` modifier. It allows the method to be called with instance syntax without modifying or inheriting from the target type. The compiler resolves it at compile time and emits a static method call. Extension methods are useful for interfaces, third-party types, LINQ-style APIs, and small stateless helpers, but instance methods take precedence and extensions cannot access private state or override behavior.
