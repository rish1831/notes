# Expression Trees

## Interview answer

An expression tree stores a lambda as data instead of immediately executable code. Libraries can inspect or translate it; Entity Framework uses expression trees to convert LINQ queries into SQL.

## In simple words

A delegate runs the instruction. An expression tree describes the instruction so another system can understand it first.

```csharp
Func<User, bool> function = user => user.IsActive;                 // Runs in C#
Expression<Func<User, bool>> tree = user => user.IsActive;         // Can be inspected
```

**Remember:** `IEnumerable<T>` normally executes .NET code; `IQueryable<T>` builds expression trees for a provider to translate.

**Catch:** Code can compile as an expression tree but still fail at runtime if the provider cannot translate a method into SQL. Calling `AsEnumerable()` switches the remaining work to in-memory execution and may load much more data.
