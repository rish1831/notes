# Parking Lot LLD

## Quick Lookup

### Domain entities

Where in code: `Vehicle`, `ParkingSpot`, `ParkingTicket`.

Short version: Hold core business state.

### Specialization

Where in code: `Bike`, `Car`, `Truck`.

Short version: Vehicle type decides spot compatibility.

### Search behavior

Where in code: `ParkingFloor.FindAvailableSpot`.

Short version: Finds the first matching available spot.

### Strategy pattern

Where in code: `IFeeCalculator`.

Short version: Fee calculation can change independently.

### Coordination service

Where in code: `ParkingLot`.

Short version: Orchestrates park and exit flow.

### Concurrency guard

Where in code: `_lock`.

Short version: Protects active tickets and spot updates.

## Most Important FAQ

### What are the main responsibilities?

- `Vehicle`: stores vehicle identity and type.
- `ParkingSpot`: knows whether a vehicle can fit and handles park/vacate state.
- `ParkingFloor`: searches available spots on one floor.
- `ParkingTicket`: stores parking session data.
- `IFeeCalculator`: calculates fee using a strategy.
- `ParkingLot`: coordinates parking, ticket creation, fee calculation, and exit.

### Why is `IFeeCalculator` useful?

It separates fee logic from parking orchestration. Tomorrow you can add weekend pricing, mall pricing, or slab pricing without changing `ParkingLot`.

## Normal FAQ

### Why does `ParkingLot` use a lock?

Parking and exit modify shared state: spot availability and `_activeTickets`. The lock prevents two requests from assigning or vacating the same spot at the same time.

### What is the current spot allocation strategy?

It is first-fit: scan floors in order, then choose the first available compatible spot.

```csharp
public ParkingSpot? FindAvailableSpot(Vehicle vehicle)
{
    return _spots.FirstOrDefault(spot =>
        spot.IsAvailable && spot.CanFitVehicle(vehicle));
}
```

## Code

See [ParkingLot.cs](ParkingLot.cs).

## Interview Follow-Ups

### How would you support multiple allocation strategies?

Extract spot selection behind an interface such as `ISpotAllocationStrategy`, then implement first-fit, nearest-floor, or best-fit strategies.

### How would you support different fee rules?

Add more `IFeeCalculator` implementations and inject the correct one based on lot type, city, vehicle type, or pricing policy.
