# OOP Principles in C#

OOP—Object-Oriented Programming—organizes software around objects that combine state and behaviour.

The four main OOP principles are:

- Encapsulation
- Abstraction
- Inheritance
- Polymorphism

---

## 1. Encapsulation

> Encapsulation means protecting an object’s internal state and controlling how that state can be accessed or modified.

It is not simply making fields private. The main goal is to prevent external code from putting an object into an invalid state.

### Without encapsulation

```csharp
public class BankAccount
{
    public decimal Balance;
}
```

External code can directly assign an invalid value:

```csharp
var account = new BankAccount();

account.Balance = -10_000;
```

### With encapsulation

```csharp
public class BankAccount
{
    public decimal Balance { get; private set; }

    public void Deposit(decimal amount)
    {
        if (amount <= 0)
            throw new ArgumentException(
                "Deposit amount must be positive.");

        Balance += amount;
    }

    public void Withdraw(decimal amount)
    {
        if (amount <= 0)
            throw new ArgumentException(
                "Withdrawal amount must be positive.");

        if (amount > Balance)
            throw new InvalidOperationException(
                "Insufficient balance.");

        Balance -= amount;
    }
}
```

External code can read the balance:

```csharp
Console.WriteLine(account.Balance);
```

But it cannot directly modify it:

```csharp
account.Balance = -500; // Compilation error
```

Changes must happen through controlled business operations:

```csharp
account.Deposit(1_000);
account.Withdraw(500);
```

### Private field vs property

A private field is purely internal:

```csharp
private decimal _balance;
```

Outside code cannot read or modify it.

A property can provide controlled public access:

```csharp
public decimal Balance { get; private set; }
```

Outside code can read it, but only the class can assign it.

These two approaches provide almost identical external protection:

```csharp
private decimal _balance;
public decimal Balance => _balance;
```

```csharp
public decimal Balance { get; private set; }
```

For a simple property, prefer the auto-property:

```csharp
public decimal Balance { get; private set; }
```

Use an explicit backing field when custom property behaviour is required:

```csharp
private string _name = "";

public string Name
{
    get => _name;
    private set => _name = value.Trim();
}
```

Examples of custom backing-field logic include:

- Validation
- Value transformation
- Lazy initialization
- Caching
- Change notifications
- Different internal and external representations

Business operations are usually better expressed through methods:

```csharp
account.Withdraw(500);
```

rather than directly assigning state:

```csharp
account.Balance -= 500;
```

### When to use a property or private field

| Requirement                                          | Choice                                   |
| ---------------------------------------------------- | ---------------------------------------- |
| Value is entirely internal                           | Private field                            |
| Callers need read access but not direct modification | Public getter with private setter        |
| Callers may directly read and write                  | Public property, usually with validation |
| Custom storage or access logic is needed             | Private backing field plus property      |
| Change represents a business operation               | Readable property plus domain methods    |

### Interview answer

> Encapsulation means an object controls access to its own state and protects its invariants. In C#, I achieve it with access modifiers, properties and domain methods. For example, a bank account exposes `Balance` for reading but requires changes through `Deposit` and `Withdraw`.

---

## 2. Abstraction

> Abstraction means exposing what an object can do while hiding how it does it.

The caller should know only the required operations, not all implementation details.

### Example

```csharp
public interface IPaymentProcessor
{
    Task<bool> PayAsync(decimal amount);
}
```

Implementations hide their internal details:

```csharp
public class UpiPaymentProcessor : IPaymentProcessor
{
    public async Task<bool> PayAsync(decimal amount)
    {
        // UPI authentication
        // API communication
        // Response validation

        await Task.Delay(100);
        return true;
    }
}

public class CardPaymentProcessor : IPaymentProcessor
{
    public async Task<bool> PayAsync(decimal amount)
    {
        // Card validation
        // Payment gateway call
        // Transaction processing

        await Task.Delay(100);
        return true;
    }
}
```

The caller uses only the abstraction:

```csharp
public class CheckoutService
{
    private readonly IPaymentProcessor _paymentProcessor;

    public CheckoutService(
        IPaymentProcessor paymentProcessor)
    {
        _paymentProcessor = paymentProcessor;
    }

    public Task<bool> CheckoutAsync(decimal amount)
    {
        return _paymentProcessor.PayAsync(amount);
    }
}
```

The caller does not need to know:

- Which payment API is used
- How authentication works
- How the HTTP request is created
- How the gateway response is processed

### Why use abstraction?

- Hides implementation complexity
- Reduces coupling
- Makes implementations replaceable
- Makes testing easier
- Exposes only necessary operations
- Allows callers to work with a stable contract

### How abstraction is achieved in C#

Common mechanisms include:

- Interfaces
- Abstract classes
- Public methods that hide private implementation details
- Encapsulated service APIs

Abstraction does not always require an interface. Even this method provides abstraction:

```csharp
public void Withdraw(decimal amount)
{
    ValidateAmount(amount);
    CheckBalance(amount);
    DeductBalance(amount);
    RecordTransaction(amount);
}
```

The caller only invokes:

```csharp
account.Withdraw(500);
```

The internal steps remain hidden.

### Abstraction vs encapsulation

| Encapsulation                                   | Abstraction                              |
| ----------------------------------------------- | ---------------------------------------- |
| Protects internal state                         | Hides implementation complexity          |
| Controls access and mutation                    | Exposes only essential operations        |
| Focuses on protecting data and invariants       | Focuses on simplifying usage             |
| Example: preventing direct balance modification | Example: hiding payment processing steps |

### Interview answer

> Abstraction exposes essential behaviour while hiding implementation details. In C#, I achieve it through interfaces, abstract classes and public APIs. For example, a checkout service calls `PayAsync` without knowing how UPI or card processing works internally.

---

## 3. Inheritance

> Inheritance allows a derived class to reuse and specialize accessible members of a base class.

It represents an **is-a** relationship.

```text
Car is a Vehicle
Bike is a Vehicle
```

### Example

```csharp
public class Vehicle
{
    public string RegistrationNumber { get; }

    protected Vehicle(string registrationNumber)
    {
        RegistrationNumber = registrationNumber;
    }

    public void DisplayRegistration()
    {
        Console.WriteLine(RegistrationNumber);
    }
}
```

```csharp
public class Car : Vehicle
{
    public int NumberOfDoors { get; }

    public Car(
        string registrationNumber,
        int numberOfDoors)
        : base(registrationNumber)
    {
        NumberOfDoors = numberOfDoors;
    }
}
```

Usage:

```csharp
var car = new Car("TS09AB1234", 4);

Console.WriteLine(car.RegistrationNumber);
car.DisplayRegistration();
Console.WriteLine(car.NumberOfDoors);
```

`Car` inherits accessible members from `Vehicle` and adds its own members.

### Constructors

Constructors are not inherited, but the derived constructor invokes a base constructor:

```csharp
public Car(string registrationNumber)
    : base(registrationNumber)
{
}
```

The base constructor runs before the derived constructor.

### Access modifiers and inheritance

A derived class can access:

- `public` members
- `protected` members
- Applicable `internal` and `protected internal` members

It cannot directly access `private` base members:

```csharp
public class Vehicle
{
    private string _engineNumber = "";
    protected string Model = "";
}

public class Car : Vehicle
{
    public void Print()
    {
        Console.WriteLine(Model); // Allowed

        // Console.WriteLine(_engineNumber);
        // Compilation error
    }
}
```

The private field remains part of the object, but only the base class can directly access it.

### Types of inheritance

#### Single inheritance

```csharp
public class Car : Vehicle
{
}
```

#### Multilevel inheritance

```csharp
public class Vehicle
{
}

public class Car : Vehicle
{
}

public class ElectricCar : Car
{
}
```

#### Hierarchical inheritance

```csharp
public class Car : Vehicle
{
}

public class Bike : Vehicle
{
}

public class Truck : Vehicle
{
}
```

C# does not allow multiple class inheritance:

```csharp
// Not allowed
public class FlyingCar : Car, Aircraft
{
}
```

But a class can implement multiple interfaces:

```csharp
public class FlyingCar :
    Car,
    IFlyable,
    ITrackable
{
}
```

### Same method in multiple interfaces

If two interfaces declare the same method signature, one public method can satisfy both:

```csharp
public interface IFlyable
{
    void Start();
}

public interface ITrackable
{
    void Start();
}
```

```csharp
public class FlyingCar : IFlyable, ITrackable
{
    public void Start()
    {
        Console.WriteLine("Starting");
    }
}
```

If different behaviour is required, use explicit interface implementation:

```csharp
public class FlyingCar : IFlyable, ITrackable
{
    void IFlyable.Start()
    {
        Console.WriteLine("Starting flight system");
    }

    void ITrackable.Start()
    {
        Console.WriteLine("Starting tracking system");
    }
}
```

Usage:

```csharp
var car = new FlyingCar();

((IFlyable)car).Start();
((ITrackable)car).Start();
```

### Inheritance vs composition

Inheritance represents **is-a**:

```csharp
public class Car : Vehicle
{
}
```

Composition represents **has-a**:

```csharp
public class Car
{
    private readonly Engine _engine;
}
```

Use inheritance when the child genuinely satisfies the parent’s contract.

Prefer composition when:

- You only want behaviour reuse
- The relationship is “has-a”
- Behaviour needs to change independently
- Inheritance would create a rigid hierarchy

### Interview answer

> Inheritance allows a derived class to reuse and specialize accessible behaviour from a base class. I use it for a genuine is-a relationship where the derived class can honour the base contract. I prefer composition when the relationship is has-a or when I only need functionality reuse.

---

## 4. Polymorphism

> Polymorphism means using different implementations through one common type or contract.

The same method call can produce different behaviour depending on the implementation.

### Runtime polymorphism using overriding

```csharp
public abstract class Vehicle
{
    public abstract decimal CalculateFee(int hours);
}
```

```csharp
public class Car : Vehicle
{
    public override decimal CalculateFee(int hours)
    {
        return hours * 50;
    }
}

public class Bike : Vehicle
{
    public override decimal CalculateFee(int hours)
    {
        return hours * 20;
    }
}
```

A caller works with the base type:

```csharp
public void PrintFee(Vehicle vehicle)
{
    Console.WriteLine(vehicle.CalculateFee(2));
}
```

Different objects provide different behaviour:

```csharp
PrintFee(new Car());  // 100
PrintFee(new Bike()); // 40
```

This avoids type-specific conditions:

```csharp
if (vehicle is Car)
{
    // ...
}
else if (vehicle is Bike)
{
    // ...
}
```

### Reference type vs runtime type

```csharp
Vehicle vehicle = new Car();
```

- Reference/compile-time type: `Vehicle`
- Actual/runtime type: `Car`

When an overridden method is called:

```csharp
vehicle.CalculateFee(2);
```

the runtime selects `Car.CalculateFee()`.

### Runtime polymorphism using interfaces

```csharp
public interface IPaymentProcessor
{
    void Pay(decimal amount);
}
```

```csharp
public class CardPaymentProcessor : IPaymentProcessor
{
    public void Pay(decimal amount)
    {
        Console.WriteLine("Paid using card");
    }
}

public class UpiPaymentProcessor : IPaymentProcessor
{
    public void Pay(decimal amount)
    {
        Console.WriteLine("Paid using UPI");
    }
}
```

```csharp
public void ProcessPayment(
    IPaymentProcessor processor,
    decimal amount)
{
    processor.Pay(amount);
}
```

```csharp
ProcessPayment(new CardPaymentProcessor(), 500);
ProcessPayment(new UpiPaymentProcessor(), 500);
```

### Compile-time polymorphism using overloading

Overloading means using the same method name with different parameter lists:

```csharp
public class Calculator
{
    public int Add(int a, int b)
    {
        return a + b;
    }

    public double Add(double a, double b)
    {
        return a + b;
    }

    public int Add(int a, int b, int c)
    {
        return a + b + c;
    }
}
```

The compiler chooses the method based on the arguments:

```csharp
calculator.Add(1, 2);
calculator.Add(1.5, 2.5);
calculator.Add(1, 2, 3);
```

Methods can be overloaded by changing:

- Number of parameters
- Parameter types
- Parameter order

Changing only the return type is not enough:

```csharp
int Calculate(int hours);

// Not allowed
decimal Calculate(int hours);
```

### Overloading vs overriding

| Overloading                         | Overriding                                   |
| ----------------------------------- | -------------------------------------------- |
| Same name, different parameter list | Same signature in base and derived classes   |
| Usually within the same class       | Requires inheritance                         |
| Resolved at compile time            | Resolved at runtime                          |
| Compile-time polymorphism           | Runtime polymorphism                         |
| No `virtual` or `override` required | Requires `virtual`/`abstract` and `override` |

---

## `virtual`, `abstract`, `override` and `new`

### `virtual`

Provides a base implementation that children may override:

```csharp
public class Vehicle
{
    public virtual void Start()
    {
        Console.WriteLine("Vehicle started");
    }
}
```

### `abstract`

Provides no implementation and forces concrete child classes to implement it:

```csharp
public abstract class Vehicle
{
    public abstract void Start();
}
```

### `override`

Replaces inherited virtual or abstract behaviour polymorphically:

```csharp
public class Car : Vehicle
{
    public override void Start()
    {
        Console.WriteLine("Car started");
    }
}
```

```csharp
Vehicle vehicle = new Car();
vehicle.Start(); // Car started
```

The runtime object determines the implementation.

### `new`

Hides an inherited member instead of overriding it:

```csharp
public class Vehicle
{
    public void Start()
    {
        Console.WriteLine("Vehicle started");
    }
}

public class Car : Vehicle
{
    public new void Start()
    {
        Console.WriteLine("Car started");
    }
}
```

```csharp
Car car = new Car();
car.Start(); // Car started

Vehicle vehicle = car;
vehicle.Start(); // Vehicle started
```

With method hiding, the reference type determines the method.

Use `new` when intentionally hiding a base member, commonly when the base method is non-virtual. It is relatively uncommon and can make behaviour confusing.

If the base method is virtual and the child is providing a specialized version of the same behaviour, normally use `override`.

### `override` vs `new`

| `override`                                  | `new`                            |
| ------------------------------------------- | -------------------------------- |
| Replaces polymorphic base behaviour         | Hides the base member            |
| Base member must be `virtual` or `abstract` | Can hide non-virtual members     |
| Runtime object determines method            | Reference type determines method |
| Usually preferred for specialization        | Used only for intentional hiding |

---

## Why use polymorphism?

Without polymorphism:

```csharp
public decimal CalculateFee(
    object vehicle,
    int hours)
{
    if (vehicle is Car)
        return hours * 50;

    if (vehicle is Bike)
        return hours * 20;

    throw new NotSupportedException();
}
```

Every new type requires modifying the condition.

With polymorphism, each type owns its behaviour:

```csharp
public class Truck : Vehicle
{
    public override decimal CalculateFee(int hours)
    {
        return hours * 100;
    }
}
```

The existing caller remains unchanged.

Benefits include:

- Reduces type-specific `if/else` logic
- Makes implementations replaceable
- Keeps behaviour with the responsible type
- Supports extension
- Allows code to depend on common contracts
- Improves testing

### Interview answer

> Polymorphism means different objects can be used through one common base type or interface while providing their own behaviour. In C#, overloading provides compile-time polymorphism, while overriding and interface implementations provide runtime polymorphism.

---

## Quick comparison

| Principle     | Simple meaning                     | Common C# mechanisms                         |
| ------------- | ---------------------------------- | -------------------------------------------- |
| Encapsulation | Protect and control object state   | Access modifiers, properties, domain methods |
| Abstraction   | Hide implementation complexity     | Interfaces, abstract classes, public APIs    |
| Inheritance   | Reuse and specialize a base type   | `class Car : Vehicle`                        |
| Polymorphism  | Same contract, different behaviour | Overriding, interfaces, overloading          |

## Final interview summary

> Encapsulation protects state and ensures changes happen through controlled operations. Abstraction exposes required behaviour while hiding implementation details. Inheritance models a genuine is-a relationship and allows derived classes to reuse or specialize base behaviour. Polymorphism allows multiple implementations to be used through one common contract, with overloading resolved at compile time and overriding resolved at runtime.
