# Async and Concurrency in .NET

## Most Important FAQ

### Does `async` create a new thread?

No. `async` creates a state machine. For I/O-bound work, the thread is released while the operation is pending and the continuation runs later when the operation completes.

### What causes the most common async deadlock?

Blocking on async code with `.Result` or `.Wait()` while a captured synchronization context is waiting for the same continuation.

## Quick revision

### `async`/`await`

Short version: A compiler-generated state machine; it does not automatically create a thread.

### `Task`

Short version: Represents an operation, not a thread.

### Async I/O

Short version: Frees the thread while waiting for external work.

### `Task.Run`

Short version: Queues CPU-bound work to the ThreadPool.

### `Task.WhenAll`

Short version: Concurrently waits for several independent tasks.

### `Parallel.ForEach`

Short version: Uses parallel threads for synchronous CPU-bound iterations.

### `ConfigureAwait(false)`

Short version: Does not require resuming on the captured context.

### `ValueTask<T>`

Short version: Allocation optimization for frequently synchronous completion.

### `CancellationToken`

Short version: Cooperative cancellation request, not forced termination.

> Core distinction: concurrency is handling multiple operations in overlapping periods; parallelism is executing work simultaneously, usually on multiple CPU cores.

## Question 1: What happens internally when you call an async method?

The C# compiler transforms a method marked with `async` into a hidden state machine that implements `IAsyncStateMachine`. The state machine preserves parameters, local variables, execution state, and continuations without blocking an OS thread while an incomplete operation is awaited.

1. **State machine creation:** Parameters and local variables that must survive an `await` become fields in the generated state machine.
2. **Synchronous start:** Calling an async method begins executing it synchronously on the calling thread.
3. **Awaiter check:** At an `await`, the state machine asks the awaiter whether the operation is complete.
4. **Suspension:** If incomplete, it saves its state, registers a continuation, and returns an incomplete `Task` or `Task<T>` to the caller.
5. **Resumption:** When the operation completes, the continuation resumes from the saved state. It may return to a captured context, depending on the environment and await configuration.

```csharp
static async Task<string> FetchDataAsync(HttpClient client, string url)
{
    Console.WriteLine("Request starting");
    string content = await client.GetStringAsync(url);
    Console.WriteLine("Request completed");
    return content;
}

string content = await FetchDataAsync(httpClient, "https://example.com");
```

This helps applications remain responsive and scalable during I/O. While the request is in progress, the calling thread can process other work.

---

## Question 2: Task vs Thread vs ThreadPool?

### Thread

A `Thread` represents an OS thread with its own execution stack. Creating one is relatively expensive, but it offers direct control over lifetime, priority, and foreground or background status.

```csharp
Thread thread = new(() => Console.WriteLine("Dedicated work"));
thread.Start();
thread.Join();
```

Use a dedicated thread only for specialized, long-running work that requires thread-level control.

### ThreadPool

The `ThreadPool` is a runtime-managed collection of reusable worker threads. Reusing threads avoids the cost of creating and destroying a thread for every short operation.

```csharp
ThreadPool.QueueUserWorkItem(_ => Console.WriteLine("Pooled work"));
```

Most application code should use `Task` rather than queue work directly because tasks provide results, exceptions, cancellation, and composition.

### Task

A `Task` represents an operation and its eventual completion, result, or failure. It does not necessarily represent a thread. CPU work commonly runs on a ThreadPool thread, while true async I/O normally occupies no thread while waiting.

```csharp
Task<int> calculation = Task.Run(() => Enumerable.Range(1, 100).Sum());
int result = await calculation;
```

### `Thread`

Represents: An OS execution thread.

Managed by: Developer and OS.

Typical use: Dedicated work needing direct control.

### `ThreadPool`

Represents: Reusable worker threads.

Managed by: .NET runtime.

Typical use: Short background CPU work.

### `Task`

Represents: An asynchronous operation.

Managed by: Task Parallel Library.

Typical use: Composable async or parallel work.

In short, a `Thread` is an execution resource, the `ThreadPool` manages reusable execution resources, and a `Task` describes work.

---

## Question 3: What is SynchronizationContext?

`SynchronizationContext` coordinates where an asynchronous continuation should run. UI applications normally use it to return continuations to the UI thread. ASP.NET Core typically has no custom synchronization context, so continuations can run on available ThreadPool threads.

```csharp
private async void LoadButton_Click(object sender, EventArgs e)
{
    string data = await httpClient.GetStringAsync("https://example.com");
    resultLabel.Text = data; // Back on the UI context
}
```

It helps code satisfy thread-affinity rules, particularly in WPF and WinForms. It also explains how blocking a context thread can cause a deadlock.

---

## Question 4: What is ConfigureAwait(false)?

`ConfigureAwait(false)` says that a continuation does not need to resume on the captured context. It does not guarantee a different thread; it only removes the requirement to return to the original context.

```csharp
public async Task<string> DownloadAsync(string url)
{
    using HttpClient client = new();
    return await client.GetStringAsync(url).ConfigureAwait(false);
}
```

It is useful in reusable library code that does not access UI controls or context-bound state. ASP.NET Core usually gains little from it because it does not install a custom `SynchronizationContext`.

---

## Question 5: What causes deadlocks in async code?

An async deadlock can occur when a thread blocks on `.Result` or `.Wait()` while the awaited continuation needs that same thread. The thread waits for the task, and the task waits for the thread.

```csharp
// Can deadlock in a UI or context-bound application.
string result = FetchDataAsync().Result;
FetchDataAsync().Wait();

// Preferred: async all the way.
string result = await FetchDataAsync();
```

Using `await` keeps threads available, avoids context deadlocks, and improves server scalability. Modern C# supports `async Task Main()`, so console applications rarely need to block on async work. Synchronous bridging should be limited to unavoidable legacy boundaries.

---

## Question 6: Task.WhenAll vs Parallel.ForEach?

`Task.WhenAll` asynchronously waits for independent tasks and is ideal for concurrent I/O. `Parallel.ForEach` partitions synchronous work across threads and is intended for CPU-bound processing. For asynchronous iteration with controlled concurrency, use `Parallel.ForEachAsync`.

### I/O-bound work

```csharp
string[] urls = ["https://example.com", "https://example.org"];
Task<string>[] downloads = urls
    .Select(url => httpClient.GetStringAsync(url))
    .ToArray();

string[] pages = await Task.WhenAll(downloads);
```

### CPU-bound work

```csharp
Parallel.ForEach(images, image =>
{
    ResizeImage(image);
});
```

`Task.WhenAll` reduces total waiting time without blocking threads. `Parallel.ForEach` can use multiple CPU cores to reduce processing time, but parallel overhead means it should be benchmarked.

---

## Question 7: Task.Run vs async I/O?

`Task.Run` queues synchronous CPU-bound work to a ThreadPool thread. Async I/O waits for an external operation without keeping a thread blocked.

```csharp
// CPU-bound: useful for keeping a desktop UI responsive.
int result = await Task.Run(() => HeavyCalculation());

// I/O-bound: use the naturally asynchronous API directly.
string content = await httpClient.GetStringAsync(url);
```

Use `Task.Run` in client applications when expensive CPU work would block the UI. In ASP.NET Core, it generally does not improve scalability because the CPU work still consumes a server thread. Do not wrap naturally asynchronous I/O in `Task.Run`.

---

## Question 8: What is ValueTask?

`ValueTask<T>` can contain an immediately available result or an asynchronous operation. It can avoid allocating a `Task<T>` when a frequently called method usually completes synchronously.

```csharp
private readonly Dictionary<int, Product> cache = new();

public ValueTask<Product> GetProductAsync(int id)
{
    if (cache.TryGetValue(id, out Product? product))
        return ValueTask.FromResult(product);

    return new ValueTask<Product>(LoadProductAsync(id));
}
```

Use it only in performance-sensitive code after measurement shows that task allocations matter. A `ValueTask` should normally be awaited once and should not be casually stored or shared. For most application code, `Task` is the simpler default.

---

## Question 9: How do CancellationTokens work?

Cancellation in .NET is cooperative. A `CancellationTokenSource` creates and controls a token. The caller passes the token through the call chain, and operations forward or periodically check it. Calling `Cancel()` requests cancellation; it does not forcibly terminate a thread.

```csharp
using CancellationTokenSource timeout = new(TimeSpan.FromSeconds(10));

try
{
    string content = await httpClient.GetStringAsync(url, timeout.Token);
}
catch (OperationCanceledException) when (timeout.IsCancellationRequested)
{
    Console.WriteLine("The operation was cancelled or timed out.");
}
```

CPU-bound code must check the token explicitly:

```csharp
static long Calculate(CancellationToken cancellationToken)
{
    long total = 0;

    for (int i = 0; i < 1_000_000; i++)
    {
        cancellationToken.ThrowIfCancellationRequested();
        total += i;
    }

    return total;
}
```

Cancellation stops work that is no longer useful after a user action, client disconnect, or timeout. This saves CPU, memory, network capacity, and database connections. Pass the token to every API that accepts one and treat `OperationCanceledException` as the normal cancellation path.

---

## Common SSE interview follow-ups

### Does `async` create a new thread?

No. An async method begins synchronously. Awaiting incomplete I/O normally releases the current thread; CPU work uses another thread only when explicitly scheduled, for example with `Task.Run`.

### Why should async methods return Task instead of void?

`Task` lets the caller await completion, observe exceptions, compose operations, and test the method. `async void` should normally be limited to event handlers because its completion and exceptions cannot be observed through a task.

### When does an async method throw?

Exceptions before a returned task is obtained can be thrown synchronously in some non-async wrappers. Exceptions inside an `async Task` method are stored in the returned task and rethrown when it is awaited.

### Is awaiting tasks one at a time concurrent?

Not if each operation is started only after the previous await completes.

```csharp
// Sequential
string first = await DownloadAsync(firstUrl);
string second = await DownloadAsync(secondUrl);

// Concurrent
Task<string> firstTask = DownloadAsync(firstUrl);
Task<string> secondTask = DownloadAsync(secondUrl);
string[] results = await Task.WhenAll(firstTask, secondTask);
```

### How should concurrency be limited?

Unbounded concurrency can exhaust sockets, database connections, memory, or downstream capacity. Use `Parallel.ForEachAsync`, `SemaphoreSlim`, channels, or a rate limiter to apply backpressure.

```csharp
using SemaphoreSlim gate = new(initialCount: 10);

Task[] tasks = urls.Select(async url =>
{
    await gate.WaitAsync(cancellationToken);
    try
    {
        await DownloadAsync(url, cancellationToken);
    }
    finally
    {
        gate.Release();
    }
}).ToArray();

await Task.WhenAll(tasks);
```

### Common pitfalls

- Using `.Result`, `.Wait()`, or `.GetAwaiter().GetResult()` in an async flow.
- Using `async void` outside event handlers.
- Wrapping naturally asynchronous I/O in `Task.Run`.
- Starting unlimited concurrent requests.
- Ignoring or failing to propagate cancellation tokens.
- Assuming `await` always changes threads.
- Using `ValueTask<T>` without measuring a real allocation problem.
- Forgetting to observe all exceptions from concurrent operations.
