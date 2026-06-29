# Equality in C#

## `==` vs `Equals()`

**Interview answer:** `==` is an operator and `Equals()` is a method. Both can be customized. Normal classes usually compare references; strings and records compare values.

== is an operator and Equals() is a method. For reference types, == typically compares object references unless the operator is overloaded. Equals() compares objects according to the type's equality implementation. Types like string and records implement value-based equality, so both often return the same result, but they can differ for boxed values and custom types.

That's the answer interviewers are looking for. The key idea is:

== answers "same according to the operator?"

Equals() answers "same according to the object's equality rules?"

**In simple words:** `==` and `Equals()` both ask “are these equal?”, but the type decides what equal means.

Use `ReferenceEquals(a, b)` when you specifically need to know whether two variables point to the same object.

**Catch:** Overloading `==` does not automatically change `Equals()`, and overriding `Equals()` does not automatically overload `==`. Keep them consistent when implementing value equality.

### Where can they differ?

#### 1. Boxed value types

```csharp
object a = 5;
object b = 5;

Console.WriteLine(a == b);      // False
Console.WriteLine(a.Equals(b)); // True
```

Each `5` is boxed into a separate object. Because both variables are declared as `object`, `==` compares their references. `Equals()` uses `int`'s value equality, so it compares the values inside the boxes.

This is the simplest example where they differ without writing a custom equality implementation.

#### 2. A normal class

By default, both compare object identity, so two separate objects are not equal even when their data matches.

```csharp
var first = new Person("Sam");
var second = new Person("Sam");

Console.WriteLine(first == second);      // False
Console.WriteLine(first.Equals(second)); // False
```

#### 3. A class overrides Equals but not `==`

This is the clearest case where the results differ.

```csharp
public class Person
{
    public string Name { get; }
    public Person(string name) => Name = name;

    public override bool Equals(object? obj) =>
        obj is Person other && Name == other.Name;

    public override int GetHashCode() => Name.GetHashCode();
}

var first = new Person("Sam");
var second = new Person("Sam");

Console.WriteLine(first == second);      // False: reference comparison
Console.WriteLine(first.Equals(second)); // True: compares Name
```

#### 4. Strings and records

Strings and records implement value equality, so both normally compare their content/data.

```csharp
string first = new string("Sam".ToCharArray());
string second = new string("Sam".ToCharArray());

Console.WriteLine(first == second);      // True
Console.WriteLine(first.Equals(second)); // True
```

#### 5. Null values

`==` can safely compare null references. Calling an instance `Equals()` on null throws `NullReferenceException`.

```csharp
Person? person = null;

Console.WriteLine(person == null);        // True
// person.Equals(null);                    // NullReferenceException
Console.WriteLine(object.Equals(person, null)); // True and null-safe
```

#### 6. Compile-time type can affect `==`

Operators are selected using the variables' compile-time types, while virtual `Equals()` can dispatch using the runtime type.

```csharp
object first = "Sam";
object second = new string("Sam".ToCharArray());

Console.WriteLine(first == second);      // False: object reference operator
Console.WriteLine(first.Equals(second)); // True: string's virtual Equals
```

**Interview summary:** `==` is a static operator selected from the declared operand types. `Equals()` is a virtual method, so its implementation can be selected from the actual runtime object. Their results differ when a type customizes one but not the other, when the compile-time types change operator selection, or when null is involved.

## Purpose of GetHashCode()

**Interview answer:** `Dictionary` and `HashSet` use a hash code to quickly find a possible location, then use equality to confirm the match.

**In simple words:** It gives collections a quick hint about where to look.

**Rule:** Equal objects must return the same hash code. Do not change equality-related values while an object is being used as a dictionary key.

**Catch:** Different objects may have the same hash code. A hash code is not a unique ID and should not be persisted because it may change between processes or runtime versions.

## IEquatable&lt;T&gt;

**Interview answer:** `IEquatable<T>` lets a type define strongly typed equality with another value of the same type. It also avoids boxing for value types in generic collections.

**In simple words:** The class or struct explains how to compare itself with another object of the same type.

```csharp
public bool Equals(ProductCode? other) =>
    other is not null && Value == other.Value;
```

Keep `Equals`, `GetHashCode`, `==`, and `!=` consistent.

**Catch:** Equality must be symmetric. Equality implementations across a base class and derived class can easily violate this rule, which is one reason value objects are often sealed.
