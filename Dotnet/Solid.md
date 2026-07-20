# SOLID Principles in C#

SOLID is a collection of five object-oriented design principles that help make software easier to maintain, extend, test, and understand.

- **S** — Single Responsibility Principle
- **O** — Open/Closed Principle
- **L** — Liskov Substitution Principle
- **I** — Interface Segregation Principle
- **D** — Dependency Inversion Principle

---

## 1. Single Responsibility Principle — SRP

> A class should have only one reason to change.

A class should own one responsibility. This does not mean a class can have only one method or cannot collaborate with other classes.

### Violation

```csharp
public class InvoiceService
{
    public decimal CalculateTotal(Invoice invoice)
    {
        // Calculation logic
        return 1_000;
    }

    public void Save(Invoice invoice)
    {
        // Database logic
    }

    public void SendEmail(Invoice invoice)
    {
        // Email logic
    }
}
```

`InvoiceService` has multiple reasons to change:

- Calculation rules change
- Database logic changes
- Email logic changes

### Following SRP

```csharp
public class InvoiceCalculator
{
    public decimal CalculateTotal(Invoice invoice)
    {
        // Calculation logic
        return 1_000;
    }
}

public class InvoiceRepository
{
    public void Save(Invoice invoice)
    {
        // Database logic
    }
}

public class InvoiceEmailService
{
    public void Send(Invoice invoice)
    {
        // Email logic
    }
}
```

An application service can coordinate these classes:

```csharp
public class InvoiceService
{
    private readonly InvoiceCalculator _calculator;
    private readonly InvoiceRepository _repository;
    private readonly InvoiceEmailService _emailService;

    public InvoiceService(
        InvoiceCalculator calculator,
        InvoiceRepository repository,
        InvoiceEmailService emailService)
    {
        _calculator = calculator;
        _repository = repository;
        _emailService = emailService;
    }

    public void CreateInvoice(Invoice invoice)
    {
        invoice.Total = _calculator.CalculateTotal(invoice);
        _repository.Save(invoice);
        _emailService.Send(invoice);
    }
}
```

Responsibilities:

- `InvoiceCalculator` changes when calculation rules change.
- `InvoiceRepository` changes when persistence logic changes.
- `InvoiceEmailService` changes when email delivery changes.
- `InvoiceService` changes when the workflow changes.

The classes can collaborate. SRP separates ownership of responsibilities; it does not prevent collaboration.

Interfaces are not required to achieve SRP.

### Interview answer

> SRP means a class should have one reason to change. I separate calculation, persistence, communication and workflow coordination so that changes to one responsibility remain isolated from the others.

---

## 2. Open/Closed Principle — OCP

> Software should be open for extension but closed for modification.

We should be able to add a new variation without repeatedly changing stable, tested business logic.

### Violation

```csharp
public class DiscountCalculator
{
    public decimal Calculate(CustomerType customerType, decimal amount)
    {
        if (customerType == CustomerType.Regular)
            return amount * 0.05m;

        if (customerType == CustomerType.Premium)
            return amount * 0.10m;

        throw new NotSupportedException();
    }
}
```

When `Vip` is added, `DiscountCalculator` must be modified:

```csharp
if (customerType == CustomerType.Vip)
    return amount * 0.20m;
```

Every new discount type adds another condition.

### Following OCP

Define an extension point:

```csharp
public interface IDiscountStrategy
{
    decimal Calculate(decimal amount);
}
```

Implement each variation separately:

```csharp
public class RegularDiscount : IDiscountStrategy
{
    public decimal Calculate(decimal amount)
    {
        return amount * 0.05m;
    }
}

public class PremiumDiscount : IDiscountStrategy
{
    public decimal Calculate(decimal amount)
    {
        return amount * 0.10m;
    }
}
```

The calculator uses the abstraction:

```csharp
public class DiscountCalculator
{
    public decimal Calculate(
        decimal amount,
        IDiscountStrategy strategy)
    {
        return strategy.Calculate(amount);
    }
}
```

Adding VIP requires only a new class:

```csharp
public class VipDiscount : IDiscountStrategy
{
    public decimal Calculate(decimal amount)
    {
        return amount * 0.20m;
    }
}
```

Usage:

```csharp
var calculator = new DiscountCalculator();

decimal discount = calculator.Calculate(
    1_000,
    new VipDiscount());
```

`DiscountCalculator` remains unchanged.

### What about selecting the strategy?

A switch-based factory may still need modification:

```csharp
IDiscountStrategy strategy = customerType switch
{
    CustomerType.Regular => new RegularDiscount(),
    CustomerType.Premium => new PremiumDiscount(),
    CustomerType.Vip => new VipDiscount(),
    _ => throw new NotSupportedException()
};
```

However, it isolates selection logic from calculation logic. For a more extensible approach, strategies can be registered with a DI container or stored in a dictionary.

OCP does not mean existing code can never change. It means expected variations should be handled through extension points so stable business logic does not change repeatedly.

Do not create abstractions for every imaginary future requirement. Apply OCP where variation is expected.

### Interview answer

> OCP means new behaviour should usually be introduced by adding an implementation rather than repeatedly modifying stable business logic. Interfaces, polymorphism, strategies, delegates and configuration can provide such extension points.

---

## 3. Liskov Substitution Principle — LSP

> A subtype should be safely usable wherever its base type is expected.

Simple meaning:

> If `Child` inherits from `Parent`, replacing the parent with the child should not break the program or violate expected behaviour.

### Violation

```csharp
public class Bird
{
    public virtual void Fly()
    {
        Console.WriteLine("Flying");
    }
}

public class Sparrow : Bird
{
    public override void Fly()
    {
        Console.WriteLine("Sparrow flying");
    }
}

public class Penguin : Bird
{
    public override void Fly()
    {
        throw new NotSupportedException();
    }
}
```

Caller:

```csharp
public void MakeBirdFly(Bird bird)
{
    bird.Fly();
}
```

This works:

```csharp
MakeBirdFly(new Sparrow());
```

But this breaks:

```csharp
MakeBirdFly(new Penguin());
```

`Bird` promises flying behaviour, but `Penguin` cannot satisfy that contract.

### Better design

```csharp
public abstract class Bird
{
    public abstract void Eat();
}

public interface IFlyingBird
{
    void Fly();
}
```

```csharp
public class Sparrow : Bird, IFlyingBird
{
    public override void Eat()
    {
        Console.WriteLine("Eating");
    }

    public void Fly()
    {
        Console.WriteLine("Flying");
    }
}

public class Penguin : Bird
{
    public override void Eat()
    {
        Console.WriteLine("Eating");
    }
}
```

Only flying birds are accepted:

```csharp
public void MakeBirdFly(IFlyingBird bird)
{
    bird.Fly();
}
```

Now passing a penguin results in a compile-time error rather than an unexpected runtime failure.

### Preconditions and postconditions

A **precondition** is something that must be true before calling a method.

```csharp
public void Withdraw(decimal amount)
```

Example precondition:

```text
amount > 0
```

A **postcondition** is something guaranteed after the method successfully finishes.

Example:

```text
New balance = old balance - amount
```

For LSP, a child should:

- Not strengthen preconditions
- Not weaken postconditions
- Preserve the parent’s invariants
- Avoid unexpected behavioural changes

#### Strengthening a precondition

Parent accepts every positive amount:

```text
amount > 0
```

Child accepts only:

```text
amount >= 1,000
```

The child demands more from the caller and may break code that validly passes `500`.

#### Weakening a postcondition

Parent promises:

```text
Returns a non-empty transaction ID
```

Child returns `null` or an empty value.

The child provides less than the parent promised.

A contract can be established through:

- Method signatures and types
- Documentation
- Business requirements
- Validation rules
- Tests
- Established expected behaviour

Different child behaviour is allowed—that is the point of polymorphism. It becomes an LSP violation only when it breaks the base contract.

### Interview answer

> LSP means a subtype must honour the behavioural contract of its base type. It should not demand more from callers, promise less, break invariants or reject operations that the parent supports. If it cannot satisfy the contract, inheritance is probably the wrong design.

---

## 4. Interface Segregation Principle — ISP

> Clients should not be forced to depend on methods they do not use.

Simple meaning:

> Prefer focused interfaces over large interfaces containing unrelated operations.

### Violation

```csharp
public interface IWorker
{
    void Work();
    void Eat();
    void Sleep();
}
```

A human supports everything:

```csharp
public class HumanWorker : IWorker
{
    public void Work() { }
    public void Eat() { }
    public void Sleep() { }
}
```

A robot is forced to implement meaningless operations:

```csharp
public class RobotWorker : IWorker
{
    public void Work()
    {
        Console.WriteLine("Working");
    }

    public void Eat()
    {
        throw new NotSupportedException();
    }

    public void Sleep()
    {
        throw new NotSupportedException();
    }
}
```

### Following ISP

Split the large interface:

```csharp
public interface IWorkable
{
    void Work();
}

public interface IFeedable
{
    void Eat();
}

public interface ISleepable
{
    void Sleep();
}
```

Implement only relevant contracts:

```csharp
public class HumanWorker :
    IWorkable,
    IFeedable,
    ISleepable
{
    public void Work() { }
    public void Eat() { }
    public void Sleep() { }
}

public class RobotWorker : IWorkable
{
    public void Work()
    {
        Console.WriteLine("Working");
    }
}
```

The compiler now prevents irrelevant operations:

```csharp
IWorkable robot = new RobotWorker();
robot.Work();

// robot.Eat(); // Compile-time error
```

### Client-focused example

Instead of:

```csharp
public interface IUserService
{
    User GetUser(int id);
    void CreateUser(User user);
    void DeleteUser(int id);
    void ExportUsers();
}
```

Use focused interfaces:

```csharp
public interface IUserReader
{
    User GetUser(int id);
}

public interface IUserWriter
{
    void CreateUser(User user);
    void DeleteUser(int id);
}

public interface IUserExporter
{
    void ExportUsers();
}
```

A report service depends only on reading:

```csharp
public class UserReportService
{
    private readonly IUserReader _reader;

    public UserReportService(IUserReader reader)
    {
        _reader = reader;
    }
}
```

One concrete class may still implement multiple focused interfaces.

Do not blindly create one interface for every method. Interfaces should be small and cohesive based on client requirements.

### ISP vs SRP

- **SRP:** A class should have one reason to change.
- **ISP:** A client should depend only on operations it needs.

### Interview answer

> ISP means clients and implementations should not be forced to depend on irrelevant operations. I prefer small, cohesive interfaces based on consumer needs, while avoiding unnecessary one-method interfaces when methods naturally belong together.

---

## 5. Dependency Inversion Principle — DIP

> High-level business logic should not depend directly on low-level implementation details. Both should depend on abstractions.

Simple meaning:

> Business logic should not directly choose or create a particular database, email provider, message broker or external service.

### Violation

```csharp
public class InvoiceService
{
    private readonly SqlInvoiceRepository _repository;

    public InvoiceService()
    {
        _repository = new SqlInvoiceRepository();
    }
}
```

`InvoiceService` is high-level workflow logic.

`SqlInvoiceRepository` is a low-level database implementation.

To switch to MongoDB, `InvoiceService` must be modified:

```csharp
_repository = new MongoInvoiceRepository();
```

### Merely using an interface field is insufficient

This still violates DIP:

```csharp
public class InvoiceService
{
    private readonly IInvoiceRepository _repository;

    public InvoiceService()
    {
        _repository = new SqlInvoiceRepository();
    }
}
```

Although the field uses an interface, the class still knows and creates `SqlInvoiceRepository`.

Its dependencies are effectively:

```text
InvoiceService → IInvoiceRepository
InvoiceService → SqlInvoiceRepository
```

### Following DIP

Define the capability needed by the business service:

```csharp
public interface IInvoiceRepository
{
    void Save(Invoice invoice);
}
```

Implement it:

```csharp
public class SqlInvoiceRepository : IInvoiceRepository
{
    public void Save(Invoice invoice)
    {
        Console.WriteLine("Saved in SQL");
    }
}

public class MongoInvoiceRepository : IInvoiceRepository
{
    public void Save(Invoice invoice)
    {
        Console.WriteLine("Saved in MongoDB");
    }
}
```

The service depends only on the abstraction:

```csharp
public class InvoiceService
{
    private readonly IInvoiceRepository _repository;

    public InvoiceService(IInvoiceRepository repository)
    {
        _repository = repository;
    }

    public void CreateInvoice(Invoice invoice)
    {
        _repository.Save(invoice);
    }
}
```

The concrete implementation is selected outside the business service.

SQL:

```csharp
IInvoiceRepository repository =
    new SqlInvoiceRepository();

var service = new InvoiceService(repository);
```

MongoDB:

```csharp
IInvoiceRepository repository =
    new MongoInvoiceRepository();

var service = new InvoiceService(repository);
```

`InvoiceService` remains unchanged.

### What is inverted?

Before:

```text
InvoiceService → SqlInvoiceRepository
```

The high-level module depends directly on the low-level module.

After:

```text
InvoiceService → IInvoiceRepository ← SqlInvoiceRepository
```

Both depend on the abstraction.

The concrete object must still be created somewhere. It should usually be created in the **composition root**, such as `Program.cs`, whose responsibility is wiring the application together.

### DIP vs DI vs DI container

#### Dependency Inversion Principle

A design principle:

```csharp
private readonly IInvoiceRepository _repository;
```

The business service depends on an abstraction.

#### Dependency Injection

A technique where the dependency is provided from outside:

```csharp
public InvoiceService(IInvoiceRepository repository)
{
    _repository = repository;
}
```

Passing it manually is still dependency injection:

```csharp
var service =
    new InvoiceService(new SqlInvoiceRepository());
```

#### DI container

A framework that automates object construction, dependency resolution and lifetime management:

```csharp
builder.Services.AddScoped<
    IInvoiceRepository,
    SqlInvoiceRepository>();

builder.Services.AddScoped<InvoiceService>();
```

A DI container is not mandatory for DIP. Manual constructor injection works.

It is also possible to use DI without fully following DIP:

```csharp
public InvoiceService(SqlInvoiceRepository repository)
```

The concrete repository is provided externally, so it is DI, but the business service still depends on a concrete implementation.

### Interview answer

> DIP means high-level business logic should depend on abstractions instead of concrete infrastructure. The concrete implementation should be selected outside the business class and injected into it. Dependency injection is a common way to achieve DIP, while a DI container merely automates the construction and wiring.

---

## Quick comparison

| Principle | Main question                                                         |
| --------- | --------------------------------------------------------------------- |
| SRP       | Does this class have more than one reason to change?                  |
| OCP       | Can new variations be added without repeatedly changing stable logic? |
| LSP       | Can the child safely replace the parent?                              |
| ISP       | Does this interface force clients to depend on irrelevant operations? |
| DIP       | Does business logic directly depend on concrete infrastructure?       |

## Final interview summary

> SRP separates responsibilities and reasons to change. OCP enables adding new behaviour through extension points. LSP ensures derived types preserve their base contract. ISP keeps interfaces focused on client needs. DIP makes high-level business logic depend on abstractions, with concrete implementations supplied externally.
