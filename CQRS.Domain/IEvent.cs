using Newtonsoft.Json;

namespace CQRS.Domain;

[JsonObject(ItemTypeNameHandling = TypeNameHandling.Auto)]
public interface IEvent
{
    Guid OrderId { get; }
    DateTime Timestamp { get; }
}

// domain events

public record OrderCreatedEvent(Guid OrderId, string CustomerId) : IEvent
{
    public DateTime Timestamp { get; } = DateTime.UtcNow;
}

public record ItemAddedEvent(Guid OrderId, string Item) : IEvent
{
    public DateTime Timestamp { get; } = DateTime.UtcNow;
}

public record OrderShippedEvent(Guid OrderId) : IEvent
{
    public DateTime Timestamp { get; } = DateTime.UtcNow;
}
