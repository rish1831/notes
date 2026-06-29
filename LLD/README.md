# Low-Level Design Lookup

## Topics

| Topic | File | What to revise |
| --- | --- | --- |
| Bulb and Room | [Bulb/README.md](Bulb/README.md) | Encapsulation, aggregation vs composition, object state, and domain methods |
| Parking Lot | [ParkingLot/README.md](ParkingLot/README.md) | Entities, services, strategy pattern, locking, tickets, and fee calculation |

## Most Important FAQ

### How should I explain an LLD answer?

Start with responsibilities, then entities, then relationships, then behavior. After that, mention extensibility points and edge cases.

### What should I avoid?

Avoid jumping directly into code. First explain the model: what objects exist, who owns what data, and which class performs which behavior.

## Normal FAQ

### Should every class have an interface?

No. Use interfaces when behavior needs to vary, needs mocking, or has multiple implementations. Simple domain entities usually do not need interfaces.

### What makes an LLD answer senior?

Clear ownership, low coupling, explicit invariants, well-placed validation, and an explanation of trade-offs.
