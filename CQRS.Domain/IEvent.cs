using Newtonsoft.Json;

namespace CQRS.Domain;

[JsonObject(ItemTypeNameHandling = TypeNameHandling.Auto)]
public interface IEvent
{
    Guid AggregateId { get; }
    DateTime Timestamp { get; }
}

public record OrderCreatedEvent(Guid AggregateId, string CustomerId) : IEvent
{
    public DateTime Timestamp { get; } = DateTime.UtcNow;
}

public record ItemAddedEvent(Guid AggregateId, string Item) : IEvent
{
    public DateTime Timestamp { get; } = DateTime.UtcNow;
}

public record OrderShippedEvent(Guid AggregateId) : IEvent
{
    public DateTime Timestamp { get; } = DateTime.UtcNow;
}
