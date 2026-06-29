# Bulb And Room LLD

## Quick Lookup

### Encapsulation

Where in code: `Bulb.IsOn`, `Bulb.Brightness`.

Short version: Setters are private, so behavior changes state through methods.

### State transitions

Where in code: `SwitchOn`, `SwitchOff`, `SetBrightness`.

Short version: Brightness is valid only when the bulb is on.

### Aggregation

Where in code: `Room(List<Bulb> bulbs)`.

Short version: Bulbs are created outside and passed into `Room`.

### Collection behavior

Where in code: `TurnOnAllBulbs`, `TurnOffAllBulbs`.

Short version: `Room` coordinates multiple bulbs.

## Most Important FAQ

### Is this composition or aggregation?

`Room` has a collection of `Bulb` objects, so it is a has-a relationship. Since the bulbs are created outside and injected into the room, this is aggregation rather than strict composition.

Strict composition would mean `Room` creates and owns the bulbs internally.

```csharp
public Room(int bulbCount)
{
    _bulbs = new List<Bulb>();

    for (int i = 0; i < bulbCount; i++)
    {
        _bulbs.Add(new Bulb());
    }
}
```

### Why are setters private?

Private setters protect invariants. External code cannot directly put the bulb into an invalid state like `IsOn = false` with `Brightness = 80`.

## Normal FAQ

### What invariant does `Bulb` protect?

Brightness should be between `0` and `100`, and brightness should not be set while the bulb is off.

### What does `Room` do?

`Room` coordinates a group of bulbs and exposes useful aggregate state:

```csharp
public bool IsAnyBulbOn => _bulbs.Any(bulb => bulb.IsOn);
public bool AreAllBulbsOn => _bulbs.All(bulb => bulb.IsOn);
```

## Code

See [Bulb.cs](Bulb.cs).

## Interview Follow-Ups

### How would you improve this design?

- Rename `AreAllBulbsff` to `AreAllBulbsOff`.
- Accept `IEnumerable<Bulb>` instead of `List<Bulb>` if the constructor does not need list-specific behavior.
- Consider a `Brightness` value object if more rules are added later.
