# Object-Oriented Programming

## Quick Lookup

### Abstract class

Short version: Shared base for related classes; can contain state and implementation.

### Interface

Short version: Capability/contract that many unrelated classes can implement.

### Virtual method

Short version: Has default implementation; child may override.

### Abstract method

Short version: Has no implementation; child must implement it.

### `override`

Short version: Runtime polymorphism based on actual object type.

### `new`

Short version: Method hiding based on declared variable type.

### Sealed class

Short version: Cannot be inherited.

## Most Important FAQ

### Abstract class vs interface?

Use an abstract class when related classes share identity, state, or common implementation. Use an interface when you want to describe a capability that different classes can implement.

### `override` vs `new`?

`override` replaces a virtual/abstract base method and uses runtime polymorphism. `new` hides a member and selection depends on the compile-time variable type.

## Abstract Class Vs Interface

**Interview answer:** An abstract class is a shared base for related classes and can contain state and implementation. An interface defines a capability that different classes can implement. A class gets one base class but can implement multiple interfaces.

**In simple words:** An abstract class says what related objects are; an interface says what an object can do.

```csharp
public abstract class Vehicle { public abstract void Start(); }
public interface ITrackable { void Track(); }
```

**Catch:** Interfaces can contain default implementations, but they still cannot hold per-object instance fields. Prefer composition when inheritance exists only to reuse code.

## Virtual Vs Abstract Methods

**Interview answer:** An abstract method has no implementation and must be implemented by a concrete child class. A virtual method has a default implementation that a child may override.

**In simple words:** Abstract means "you must provide it"; virtual means "use mine or replace it."

```csharp
public abstract void Start();
public virtual void Stop() => Console.WriteLine("Stopped");
```

### `override` Vs `new` Method Hiding

`override` replaces a virtual base implementation. At runtime, C# checks the actual object type and calls the most specific override.

```csharp
public abstract class Animal
{
    public virtual void Speak() => Console.WriteLine("Animal");
}

public class Dog : Animal
{
    public override void Speak() => Console.WriteLine("Dog");
}

Animal animal = new Dog();
animal.Speak(); // Dog
```

Even though the variable is declared as `Animal`, the actual object is a `Dog`, so the overridden `Dog.Speak()` runs. This is runtime polymorphism.

`new` does not override the method. It creates a separate method with the same name and hides the inherited one. C# chooses between them using the variable's declared type.

```csharp
public abstract class Animal
{
    public void Speak() => Console.WriteLine("Animal");
}

public class Dog : Animal
{
    public new void Speak() => Console.WriteLine("Dog");
}

Dog dog = new Dog();
Animal animal = dog;

dog.Speak();    // Dog
animal.Speak(); // Animal
```

Both variables point to the same `Dog` object, but they call different methods because method hiding is based on the compile-time variable type.

**In simple words:** `override` asks, "What object is this really?" `new` asks, "What type is this variable declared as?"

**Catch:** If a base method is not marked `virtual` or `abstract`, it cannot be overridden. Using `new` may be intentional, but it can cause confusing behavior when an object is referenced through its base type.

## Sealed Class

**Interview answer:** A sealed class cannot be inherited. It is used when extending the class is unsupported or could break its rules.

**In simple words:** It closes the inheritance door.

```csharp
public sealed class TokenValidator { }
```

**Catch:** A class can be open for inheritance while a specific overridden method is `sealed`, preventing only that method from being overridden again.
