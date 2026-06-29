using System;
using System.Collections.Generic;
using System.Linq;

public enum VehicleType
{
    Bike,
    Car,
    Truck
}

public enum SpotType
{
    BikeSpot,
    CarSpot,
    LargeSpot
}

public abstract class Vehicle
{
    public string Number { get; }
    public VehicleType VehicleType { get; }

    protected Vehicle(string number, VehicleType vehicleType)
    {
        Number = number;
        VehicleType = vehicleType;
    }
}

public class Bike : Vehicle
{
    public Bike(string number) : base(number, VehicleType.Bike) { }
}

public class Car : Vehicle
{
    public Car(string number) : base(number, VehicleType.Car) { }
}

public class Truck : Vehicle
{
    public Truck(string number) : base(number, VehicleType.Truck) { }
}

public class ParkingSpot
{
    public int Id { get; }
    public int FloorNumber { get; }
    public SpotType SpotType { get; }
    public bool IsAvailable { get; private set; } = true;
    public Vehicle? ParkedVehicle { get; private set; }

    public ParkingSpot(int id, int floorNumber, SpotType spotType)
    {
        Id = id;
        FloorNumber = floorNumber;
        SpotType = spotType;
    }

    public bool CanFitVehicle(Vehicle vehicle)
    {
        return vehicle.VehicleType switch
        {
            VehicleType.Bike => SpotType == SpotType.BikeSpot,
            VehicleType.Car => SpotType == SpotType.CarSpot,
            VehicleType.Truck => SpotType == SpotType.LargeSpot,
            _ => false
        };
    }

    public void Park(Vehicle vehicle)
    {
        if (!IsAvailable)
            throw new InvalidOperationException("Spot is already occupied.");

        if (!CanFitVehicle(vehicle))
            throw new InvalidOperationException("Vehicle cannot fit in this spot.");

        ParkedVehicle = vehicle;
        IsAvailable = false;
    }

    public void Vacate()
    {
        ParkedVehicle = null;
        IsAvailable = true;
    }
}

public class ParkingFloor
{
    public int FloorNumber { get; }
    private readonly List<ParkingSpot> _spots;

    public ParkingFloor(int floorNumber, List<ParkingSpot> spots)
    {
        FloorNumber = floorNumber;
        _spots = spots;
    }

    public ParkingSpot? FindAvailableSpot(Vehicle vehicle)
    {
        return _spots.FirstOrDefault(spot =>
            spot.IsAvailable && spot.CanFitVehicle(vehicle));
    }
}

public class ParkingTicket
{
    public string TicketId { get; }
    public Vehicle Vehicle { get; }
    public ParkingSpot Spot { get; }
    public DateTime EntryTime { get; }
    public DateTime? ExitTime { get; private set; }

    public ParkingTicket(Vehicle vehicle, ParkingSpot spot)
    {
        TicketId = Guid.NewGuid().ToString();
        Vehicle = vehicle;
        Spot = spot;
        EntryTime = DateTime.UtcNow;
    }

    public void MarkExit()
    {
        ExitTime = DateTime.UtcNow;
    }
}

public interface IFeeCalculator
{
    decimal CalculateFee(ParkingTicket ticket);
}

public class HourlyFeeCalculator : IFeeCalculator
{
    public decimal CalculateFee(ParkingTicket ticket)
    {
        var exitTime = ticket.ExitTime ?? DateTime.UtcNow;
        var hours = Math.Ceiling((exitTime - ticket.EntryTime).TotalHours);

        decimal rate = ticket.Vehicle.VehicleType switch
        {
            VehicleType.Bike => 20,
            VehicleType.Car => 50,
            VehicleType.Truck => 100,
            _ => 50
        };

        return (decimal)hours * rate;
    }
}

public class ParkingLot
{
    private readonly List<ParkingFloor> _floors;
    private readonly Dictionary<string, ParkingTicket> _activeTickets;
    private readonly IFeeCalculator _feeCalculator;

    private readonly object _lock = new object();

    public ParkingLot(List<ParkingFloor> floors, IFeeCalculator feeCalculator)
    {
        _floors = floors;
        _feeCalculator = feeCalculator;
        _activeTickets = new Dictionary<string, ParkingTicket>();
    }

    public ParkingTicket ParkVehicle(Vehicle vehicle)
    {
        lock (_lock)
        {
            foreach (var floor in _floors)
            {
                var spot = floor.FindAvailableSpot(vehicle);

                if (spot != null)
                {
                    spot.Park(vehicle);

                    var ticket = new ParkingTicket(vehicle, spot);
                    _activeTickets[ticket.TicketId] = ticket;

                    return ticket;
                }
            }

            throw new InvalidOperationException("No spot available.");
        }
    }

    public decimal ExitVehicle(string ticketId)
    {
        lock (_lock)
        {
            if (!_activeTickets.ContainsKey(ticketId))
                throw new InvalidOperationException("Invalid ticket.");

            var ticket = _activeTickets[ticketId];

            ticket.MarkExit();

            decimal fee = _feeCalculator.CalculateFee(ticket);

            ticket.Spot.Vacate();

            _activeTickets.Remove(ticketId);

            return fee;
        }
    }
}

public class Program
{
    public static void Main(string[] args)
    {
        var floor1 = new ParkingFloor(1, new List<ParkingSpot>
        {
            new ParkingSpot(1, 1, SpotType.BikeSpot),
            new ParkingSpot(2, 1, SpotType.CarSpot),
            new ParkingSpot(3, 1, SpotType.LargeSpot)
        });

        var floor2 = new ParkingFloor(2, new List<ParkingSpot>
        {
            new ParkingSpot(4, 2, SpotType.BikeSpot),
            new ParkingSpot(5, 2, SpotType.CarSpot),
            new ParkingSpot(6, 2, SpotType.LargeSpot)
        });

        var parkingLot = new ParkingLot(
            new List<ParkingFloor> { floor1, floor2 },
            new HourlyFeeCalculator()
        );

        Vehicle car = new Car("TS09AB1234");

        var ticket = parkingLot.ParkVehicle(car);

        Console.WriteLine($"Ticket created: {ticket.TicketId}");
        Console.WriteLine($"Vehicle parked at Floor {ticket.Spot.FloorNumber}, Spot {ticket.Spot.Id}");

        decimal fee = parkingLot.ExitVehicle(ticket.TicketId);

        Console.WriteLine($"Fee: {fee}");
    }
}
