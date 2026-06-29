using System;
using System.Collections.Generic;
using System.Linq;


// In interviews, don't get too hung up on the terminology. If they ask:

// Is composition used?

// You can say:

// Room has a collection of Bulb objects, so it uses a has-a relationship. Since the bulbs are created outside and injected into the room, this is technically aggregation rather than strict composition.

// That's a strong senior-level answer because most people stop at "composition".

// Composition will look something like below(has a relation)

// public Room(int bulbCount)
// {
//     _bulbs = new List<Bulb>();

//     for (int i = 0; i < bulbCount; i++)
//     {
//         _bulbs.Add(new Bulb());
//     }
// }

public class Program
{
    public static void Main()
    {
        Bulb bulb = new Bulb();
        Bulb bulb2 = new Bulb();
        var room = new Room(new List<Bulb> { bulb, bulb2 });
        Console.WriteLine(bulb.IsOn ? "Bulb is On" : "Bulb is Off"); // Bulb is Off
        Console.WriteLine($"Current Brightness: {bulb.Brightness}"); // Bulb is Off
        bulb.SwitchOn();
        Console.WriteLine(bulb.IsOn ? "Bulb is On" : "Bulb is Off"); // Bulb is On
        Console.WriteLine($"Current Brightness: {bulb.Brightness}"); // Current Brightness: 50
        bulb.SetBrightness(80);
        Console.WriteLine($"Current Brightness: {bulb.Brightness}"); // Current Bright
        bulb.SwitchOff();
        Console.WriteLine(bulb.IsOn ? "Bulb is On" : "Bulb is Off"); // Bulb is Off
        Console.WriteLine($"Current Brightness: {bulb.Brightness}"); // Current Brightness: 0
        room.TurnOnAllBulbs();
        Console.WriteLine($"Is Any Bulb on: {room.IsAnyBulbOn}");
        Console.WriteLine($"Are all Bulbs on: {room.AreAllBulbsOn}");
        Console.WriteLine($"Are all Bulb off: {room.AreAllBulbsff}");
    }
}

public class Bulb
{
    public bool IsOn { get; private set; }
    public int Brightness { get; private set; }
    public void SwitchOn()
    {
        if (IsOn)
        {
            return;
        }
        IsOn = true;
        Brightness = 50;
    }
    public void SwitchOff()
    {
        IsOn = false;
        Brightness = 0;
    }
    public void SetBrightness(int brightness)
    {
        if (!IsOn)
        {
            throw new InvalidOperationException("Cannot set brightness when the bulb is off.");
        }
        if (brightness < 0 || brightness > 100)
        {
            throw new ArgumentOutOfRangeException(nameof(brightness), "Brightness must be between 0 and 100.");
        }
        Brightness = brightness;
    }
}

public class Room
{
    private readonly List<Bulb> _bulbs;
    public bool IsAnyBulbOn => _bulbs.Any(bulb => bulb.IsOn);
    public bool AreAllBulbsOn => _bulbs.All(bulb => bulb.IsOn);
    public bool AreAllBulbsff => _bulbs.All(bulb => !bulb.IsOn);
    public void TurnOnAllBulbs()
    {
        foreach (var bulb in _bulbs)
        {
            bulb.SwitchOn();
        }
    }
    public void TurnOffAllBulbs()
    {
        foreach (var bulb in _bulbs)
        {
            bulb.SwitchOff();
        }
    }

    public Room(List<Bulb> bulbs)
    {
        if (bulbs == null || bulbs.Count == 0)
            throw new ArgumentException("Room must have at least one bulb.");
        _bulbs = bulbs;
    }
}