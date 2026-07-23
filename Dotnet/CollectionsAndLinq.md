Perfect. For a **4 YOE Senior .NET interview (Wells Fargo, banks, product companies)**, these are the notes I'd keep. They focus on **interview answers + when to use + practical differences** rather than theory.

---

# 1. List<T>

### Definition

> Dynamic array that stores ordered elements and allows duplicates.

### When to use

- Ordered collection
- Need indexing
- Frequent iteration
- Mostly add at the end

Examples

- Employees
- Products
- API response
- Orders

### Why not Dictionary?

No unique key.

### Why not HashSet?

Duplicates allowed.

---

# 2. Dictionary<TKey,TValue>

### Definition

> Key-value collection optimized for fast lookup using a unique key.

### When to use

Need

> "Give me employee with Id = 101"

Examples

- Employee cache
- Product lookup
- Session storage
- Configuration

### Why not List?

List searches sequentially.

Dictionary jumps directly using hashing.

### Important

- Keys unique
- Values can repeat

---

# 3. HashSet<T>

### Definition

> Collection of unique values.

### When to use

Need uniqueness.

Examples

- Emails
- Tags
- Permissions
- Processed message IDs

### Why not Dictionary?

No associated value.

### Why not List?

List allows duplicates.

---

# 4. Queue<T>

### Definition

FIFO (First In First Out)

### Use

Arrival order matters.

Examples

- Print jobs
- RabbitMQ (conceptually)
- Ticket system
- Background jobs

Methods

```csharp
Enqueue()
Dequeue()
Peek()
```

---

# 5. Stack<T>

### Definition

LIFO (Last In First Out)

### Use

Most recent item first.

Examples

- Undo
- Browser Back
- Function calls
- DFS

Methods

```csharp
Push()
Pop()
Peek()
```

---

# 6. PriorityQueue<TElement,TPriority>

### Definition

Processes items based on priority.

### Use

- Hospital
- Job Scheduler
- Ticket Priority

### Important

.NET

Smallest priority value comes first.

Equal priorities

Order **not guaranteed**.

---

# 7. LinkedList<T>

### Definition

Node-based collection.

### Use

Frequent insert/remove in middle.

Examples

- Playlist
- LRU Cache

---

# 8. ConcurrentDictionary

### Definition

Thread-safe dictionary.

### Use

Shared cache

Session cache

Background workers

### Important methods

```csharp
TryAdd()

GetOrAdd()

AddOrUpdate()

TryRemove()
```

---

# 9. ConcurrentQueue

Thread-safe FIFO.

Producer Consumer.

---

# 10. ConcurrentBag

Thread-safe

No order.

Perfect for collecting parallel results.

---

# 11. ConcurrentStack

Thread-safe LIFO.

---

# 12. BlockingCollection

Producer waits

Consumer blocks until item available.

Used in Producer Consumer.

---

# IEnumerable

### Definition

Represents a sequence that can be iterated.

### Use

Read-only iteration.

Supports

```csharp
foreach
```

LINQ

Deferred execution.

---

# ICollection

### Definition

Modifiable collection.

Supports

```csharp
Add

Remove

Count

Contains
```

No indexing.

---

# IList

### Definition

Ordered collection with indexing.

Supports

```csharp
Add

Remove

Insert

RemoveAt

[]
```

---

# IEnumerable vs ICollection vs IList

```text
IEnumerable

↓

ICollection

↓

IList
```

Think

Read

↓

Read + Modify

↓

Read + Modify + Index

---

# IEnumerable vs IQueryable

## IEnumerable

Query runs

```text
Application Memory
```

LINQ to Objects

---

## IQueryable

Query runs

```text
Database (or another LINQ provider)
```

Expression Trees

---

Example

```csharp
_context.Employees.Where(...)
```

SQL generated.

---

Example

```csharp
employees.ToList().Where(...)
```

Runs in memory.

---

# Deferred Execution

Definition

Query executes only when enumerated.

Deferred

```csharp
Where()

Select()

OrderBy()

GroupBy()

Join()

Skip()

Take()
```

Immediate

```csharp
ToList()

First()

Count()

Any()

Single()

Max()

Min()
```

---

# Thread Safety

Definition

Multiple threads safely access shared data without corruption.

Not Thread Safe

```text
List

Dictionary

HashSet

Queue

Stack
```

Thread Safe

```text
ConcurrentDictionary

ConcurrentQueue

ConcurrentStack

ConcurrentBag
```

---

# LINQ

---

## Where

Definition

Filters records.

Example

```csharp
employees.Where(e=>e.IsActive)
```

SQL

```sql
WHERE IsActive=1
```

---

## Select

Definition

Projects data.

Example

```csharp
employees.Select(e=>e.Name)
```

SQL

```sql
SELECT Name
```

---

## GroupBy

Definition

Groups data.

Example

```csharp
employees.GroupBy(e=>e.Department)
```

SQL

```sql
GROUP BY Department
```

---

## Join

Definition

Combines two collections.

Example

Employee

Department

↓

Employee + Department Name

SQL

```sql
JOIN
```

---

# Deferred LINQ

These build queries

```text
Where

Select

Join

GroupBy

OrderBy

Skip

Take
```

These execute

```text
ToList

First

Any

Count

Single
```

---

# Buckets

Dictionary internally

```text
Key

↓

HashCode

↓

Bucket

↓

Find item
```

Lookup is O(1) average because it searches only one bucket.

---

# Collision

Two keys

↓

Same bucket.

Dictionary resolves internally.

---

# Common Interview Questions

## List vs Dictionary

List

Need ordered collection.

Dictionary

Need lookup by key.

---

## Dictionary vs HashSet

Dictionary

Key + Value

HashSet

Value only.

---

## Queue vs Stack

Queue

FIFO

Stack

LIFO

---

## Queue vs PriorityQueue

Queue

Arrival order.

PriorityQueue

Priority order.

---

## ConcurrentDictionary vs Dictionary

Dictionary

Single thread.

ConcurrentDictionary

Multiple threads.

---

## IEnumerable vs IQueryable

IEnumerable

Memory.

IQueryable

Database.

---

## Where vs Select

Where

Filters rows.

Select

Chooses columns / transforms data.

---

## GroupBy

Creates buckets.

---

## Join

Combines two collections.

---

# One-page Interview Cheat Sheet

| Topic                | Remember                              |
| -------------------- | ------------------------------------- |
| List                 | Ordered collection                    |
| Dictionary           | Lookup by key                         |
| HashSet              | Unique values                         |
| Queue                | FIFO                                  |
| Stack                | LIFO                                  |
| PriorityQueue        | Priority first                        |
| ConcurrentDictionary | Thread-safe dictionary                |
| IEnumerable          | Read & iterate                        |
| ICollection          | Add/Remove                            |
| IList                | Index access                          |
| IQueryable           | Database/provider query               |
| Deferred Execution   | Executes on `foreach`/`ToList`        |
| Where                | Filter                                |
| Select               | Project/transform                     |
| GroupBy              | Group into buckets                    |
| Join                 | Combine collections                   |
| Thread-safe          | Safe concurrent access                |
| Bucket               | Internal hash storage for fast lookup |

---

## One correction I want to make

Earlier in our discussion I simplified `IQueryable` as "database". For interview purposes that's fine, but the technically correct statement is:

- **`IQueryable`** → Query is executed by the **underlying LINQ provider** (commonly a database provider like EF Core, but it could also be MongoDB, Cosmos DB, etc.).
- **`IEnumerable`** → Query is executed in **application memory**.

This distinction is worth mentioning if the interviewer digs deeper.
