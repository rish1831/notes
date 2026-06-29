# Delegates and Events in C#

## Most Important FAQ

### What is a delegate?

A delegate is a type-safe reference to a method. It lets methods be passed around as values.

### Why use an event instead of exposing a delegate?

An event restricts external code to subscribe/unsubscribe only. External code cannot directly invoke or overwrite the invocation list.

## Quick revision

### Delegate

Short version: Type-safe reference to one or more compatible methods.

### `Action`

Short version: Accepts inputs and returns `void`.

### `Func`

Short version: Accepts inputs and returns a value; the last type argument is the result.

### `Predicate<T>`

Short version: Accepts `T` and returns `bool`.

### Event

Short version: Restricted delegate used for publisher-subscriber notification.

### Covariance

Short version: `out`; a more derived result can be used as a base result.

### Contravariance

Short version: `in`; a base-type consumer can be used as a derived-type consumer.

## Question 1: What are delegates?

Delegates are type-safe references to methods. A delegate type defines the parameters and return type that a matching method must have. This allows methods to be assigned to variables, passed as arguments, and invoked later.

```csharp
public delegate int Operation(int left, int right);

static int Add(int left, int right) => left + right;

Operation operation = Add;
int result = operation(4, 3); // 7
```

Use a delegate when a method needs to receive behavior rather than fixed data. Common uses include callbacks, event handling, and configurable algorithms.

---

## Question 2: What are events?

Events signal that something has happened. They use delegates to define the signature that subscribers must implement. Events support the publisher-subscriber pattern while preventing subscribers from raising the event themselves.

```csharp
public class OrderService
{
    public event EventHandler? OrderCompleted;

    public void CompleteOrder()
    {
        Console.WriteLine("Order completed.");
        OrderCompleted?.Invoke(this, EventArgs.Empty);
    }
}

var service = new OrderService();
service.OrderCompleted += (_, _) => Console.WriteLine("Sending confirmation.");
service.CompleteOrder();
```

Events help one component notify others without being tightly coupled to them. Examples include button clicks, status changes, and domain notifications.

---

## Question 3: Action vs Func vs Predicate?

`Action`, `Func`, and `Predicate` are built-in generic delegate types.

### `Action<T>`

Returns: `void`.

Typical purpose: Perform an operation.

### `Func<T, TResult>`

Returns: A value.

Typical purpose: Transform data or calculate a result.

### `Predicate<T>`

Returns: `bool`.

Typical purpose: Test whether a value matches a condition.

```csharp
Action<string> print = message => Console.WriteLine(message);
Func<int, int, int> add = (left, right) => left + right;
Predicate<int> isEven = number => number % 2 == 0;

print("Hello");
Console.WriteLine(add(2, 3));  // 5
Console.WriteLine(isEven(4)); // True
```

Use these built-in types instead of declaring a custom delegate when their signatures describe the behavior you need. They are common in LINQ, callbacks, collection methods, and dependency injection.

---

## Question 4: What are covariance and contravariance?

Covariance and contravariance allow compatible generic types to be assigned to each other when their type arguments have an inheritance relationship.

Assume `Dog` inherits from `Animal`:

```csharp
public class Animal { }
public class Dog : Animal { }
```

### Covariance (`out`)

Covariance allows a more specific output type to be used where a more general output type is expected.

```csharp
IEnumerable<Dog> dogs = new List<Dog>();
IEnumerable<Animal> animals = dogs;

Func<Dog> createDog = () => new Dog();
Func<Animal> createAnimal = createDog;
```

`IEnumerable<T>` and the result type of `Func<TResult>` are covariant. They produce values of `T`, so their type parameters are declared with `out`.

```csharp
public interface IProducer<out T>
{
    T Produce();
}
```

### Contravariance (`in`)

Contravariance allows a method that accepts a more general input type to be used where one accepting a more specific input type is expected.

```csharp
Action<Animal> handleAnimal = animal => Console.WriteLine(animal);
Action<Dog> handleDog = handleAnimal;

handleDog(new Dog());
```

`Action<T>` is contravariant because it consumes values of `T`, so its type parameter is declared with `in`.

```csharp
public interface IConsumer<in T>
{
    void Consume(T value);
}
```

### Invariance

When a type parameter is neither `out` nor `in`, the generic type is invariant. Even though `Dog` inherits from `Animal`, the following assignment is invalid:

```csharp
List<Dog> dogs = new();
// List<Animal> animals = dogs; // Compile-time error
```

`List<T>` both accepts and returns `T`, so allowing that assignment would be unsafe. For example, someone could add a different kind of `Animal` to what is actually a list of dogs.

### When does it help?

Variance makes generic APIs more reusable while preserving type safety. A useful memory aid is:

- **Covariance:** `out` means the type is produced or returned.
- **Contravariance:** `in` means the type is consumed or accepted.
- **Invariance:** the type is both consumed and produced.

Variance applies to generic interfaces and delegates, and only to reference types.

---

## Common SSE interview follow-ups

### What is a multicast delegate?

A delegate can hold an invocation list containing multiple methods. Adding handlers with `+=` combines them; `-=` removes them. Methods run in subscription order. If the delegate returns a value, only the final handler's return value is available. If a handler throws, later handlers are not invoked unless the caller handles invocation manually.

```csharp
Action notify = () => Console.WriteLine("First");
notify += () => Console.WriteLine("Second");
notify();
```

### Why use an event instead of exposing a delegate?

An event restricts external code to subscription and unsubscription. Only the declaring type can normally raise or replace it. Exposing a public delegate would allow callers to invoke it or overwrite all existing handlers.

### How can events cause memory leaks?

The publisher stores references to subscribed delegates, and those delegates reference subscriber instances. A long-lived publisher can therefore keep a short-lived subscriber alive. Unsubscribe when lifetimes differ, use a disposable subscription pattern, or use an appropriate weak-event mechanism.

```csharp
publisher.Changed += HandleChanged;

// When the subscriber is no longer needed:
publisher.Changed -= HandleChanged;
```

### Delegate vs expression tree

A delegate is executable code. An `Expression<TDelegate>` is a data structure describing code, so a provider can inspect or translate it—for example, converting a LINQ predicate into SQL.

```csharp
Func<User, bool> compiled = user => user.IsActive;
Expression<Func<User, bool>> expression = user => user.IsActive;
```

### Common pitfalls

- Forgetting to unsubscribe from a longer-lived publisher.
- Assuming all multicast handlers run after one throws.
- Capturing mutable variables unexpectedly in lambdas.
- Using events when a direct method call or returned result would be clearer.
- Confusing covariance direction with contravariance direction.
