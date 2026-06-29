# Resource Management

## Quick Lookup

### `IDisposable`

Short version: Deterministic cleanup for external resources.

### `using`

Short version: Ensures `Dispose()` runs even when an exception occurs.

### Finalizer

Short version: Unpredictable fallback for direct unmanaged ownership.

### Common mistake

Short version: Waiting for GC to release external resources.

## Most Important FAQ

### Does GC call `Dispose()`?

No. The garbage collector reclaims managed memory. It does not guarantee timely cleanup of external resources such as files, sockets, database connections, or native handles.

### When should I implement `IDisposable`?

Implement `IDisposable` when your class owns disposable resources or directly owns unmanaged resources. Dispose only what your class owns.

## IDisposable

**Interview answer:** `IDisposable` provides predictable cleanup for resources such as files, database connections, and native handles. `using` ensures `Dispose()` runs even when an exception occurs.

**In simple words:** When we finish using an expensive resource, `Dispose` cleans it up immediately instead of waiting.

```csharp
using FileStream stream = File.OpenRead(path);
```

The garbage collector manages memory, but it does not guarantee timely release of external resources.

**Catch:** The garbage collector does not call `Dispose()` for you. Also, a class should dispose only resources it owns, not dependencies whose lifetime is managed elsewhere.

## Finalizer vs Dispose

**Interview answer:** `Dispose` is called deliberately for immediate cleanup. A finalizer is an automatic but unpredictable fallback used when a type directly owns an unmanaged resource.

**In simple words:** `Dispose` is cleaning up now; a finalizer is emergency cleanup sometime later.

```csharp
public void Dispose()
{
    ReleaseHandle();
    GC.SuppressFinalize(this);
}

~NativeResource() => ReleaseHandle();
```

Most classes should not have a finalizer. Prefer `SafeHandle` for native handles, and make `Dispose()` safe to call more than once.

**Catch:** A finalizer should not access other managed objects because their cleanup order is unknown. Defining a finalizer also makes garbage collection more expensive and delays object reclamation.
