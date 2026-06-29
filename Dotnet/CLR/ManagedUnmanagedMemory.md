# Managed, Unmanaged Memory, And Connection Pooling

Use this page for managed vs unmanaged resources, `Dispose`, database connections, and connection pooling.

## Quick Map

| Concept | Short version |
| --- | --- |
| Managed memory | Memory controlled by the CLR and reclaimed by the GC. |
| Unmanaged resource | External OS/runtime resource such as a handle, socket, native allocation, or database connection. |
| `IDisposable` | Pattern used to release unmanaged or expensive resources deterministically. |
| Connection pooling | Reuses database connections instead of opening a fresh one each time. |

## Managed Vs Unmanaged Memory

Managed memory is memory controlled by the .NET runtime. When you are done, the runtime will clean it up.

Unmanaged resources, on the other hand, are things outside the control of the runtime, such as file handles, database connections, or unmanaged memory. These need explicit cleanup, often with something like the `IDisposable` pattern, so you release them when you are done to avoid resource leaks.

## Why Database Connections Need Explicit Cleanup

While you write the code inside your .NET program, the database connection often uses resources outside the .NET runtime. The .NET runtime, or CLR, manages memory and objects within its environment. But when you talk to something external, like a database, the connection might involve operating system handles or unmanaged libraries.

The CLR does not automatically manage or release these external resources. That is why you have to explicitly close or dispose them, so those external resources are properly cleaned up.

A database connection typically relies on underlying system resources, like network sockets or file handles, which the operating system provides. Even though you write your code in .NET, these external resources are considered unmanaged. In other words, the CLR does not know when you are done with that connection. That is why you use patterns like `using` or explicitly call `Dispose` to signal that those system resources can be released when you are finished.

The MySQL provider, or similar database drivers, are separate libraries you add to your .NET project. While .NET provides the framework, these database drivers handle the low-level communication with the database. The .NET runtime cannot directly manage their connections. Your code, with the help of these libraries, requests and releases those resources.

```csharp
using var connection = new SqlConnection(connectionString);
await connection.OpenAsync();
```

## How Does Connection Pooling Work?

Connection pooling is a handy technique that keeps a pool of open database connections ready to use. When your application requests a connection, it can reuse one from the pool instead of opening a brand-new one.

When you are done, instead of fully closing the connection, it is returned to the pool for another future use. This avoids the overhead of constantly opening and closing connections. If a connection is not used for a while, the pool will close it. Pooling makes database access faster and more efficient.

By default, connection pooling is enabled in .NET, so you do not have to set it up manually. When you close or dispose a connection, it automatically returns to the pool instead of truly closing. You can adjust pool size and other settings through connection string parameters, but the basic lifecycle, reuse and eventual timeout, is managed for you. In most cases, it just works out of the box.

The connection pool will keep connections open and ready as long as they remain active. If a connection is still in the pool and has not timed out, the next request can reuse it. In practice, the pool aims to keep connections alive and reused throughout your app's lifetime as long as activity continues. If there is a long period of no use, it will clean up idle connections. Otherwise, the same connection can be reused.

The pool is more flexible than a single reusable connection. The connection pool can hold multiple connections at once. If multiple requests come in at the same time, the pool provides more connections up to a limit. By default, the pool might have a maximum of around 100 connections, but you can configure that. It is not just one connection; it scales as needed, giving you multiple connections if your app is handling many simultaneous requests.

### If My App Handles 10,000 Concurrent Requests, Will I Open 10,000 Connections?

No, you would not open 10,000 connections. The connection pool has a maximum size, commonly around 100 by default, but configurable. If you have 10,000 requests, those requests do not all get a connection at once. Instead, they wait or queue if no connection is available. As soon as a connection is released back to the pool, another request can use it. This way, the application avoids opening an uncontrolled number of database connections.

You typically configure the maximum and minimum pool size settings, usually in the connection string, and the connection pool manages the rest. It will automatically open new connections when needed, reuse them when possible, and close them when they have been idle too long, all behind the scenes.

## Related CLR Notes

- [CLR Memory](Memory.md)
- [CLR Execution Flow](RuntimeExecution.md)
- [Garbage Collection](GarbageCollection.md)
