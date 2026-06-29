# Reflection

## Interview answer

Reflection lets a program inspect type information at runtime, such as classes, methods, properties, and attributes. Frameworks use it for dependency injection, serialization, and test discovery.

## In simple words

It is code looking at other code while the application is running.

```csharp
Type type = typeof(User);
foreach (PropertyInfo property in type.GetProperties())
    Console.WriteLine(property.Name);
```

**Remember:** Reflection is flexible but slower and less type-safe than direct code. Cache results when used repeatedly.

**Catch:** Reflection errors appear at runtime, and heavy reflection can cause issues with trimming or native AOT because required members may not be visible to the build tools.
