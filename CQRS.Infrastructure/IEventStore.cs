using CQRS.Domain;

namespace CQRS.Infrastructure;

public interface IEventStore
{
    void SaveEvents(Guid aggregateId, List<IEvent> events);
    List<IEvent> GetEvents(Guid aggregateId);
}