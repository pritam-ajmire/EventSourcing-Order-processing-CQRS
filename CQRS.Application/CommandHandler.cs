using CQRS.Domain;
using CQRS.ReadModel;

namespace CQRS.Application
{
    public class CommandHandler
    {
        //private readonly IEventStore _eventStore;
        private Dictionary<Guid, List<IEvent>> _eventStore = new Dictionary<Guid, List<IEvent>>();
        public IOrderReadRepository OrderReadRepository { get; }
        private OrderProjection orderProjection { get; }
        public CommandHandler(IOrderReadRepository orderReadRepository)
        {
            //_eventStore = eventStore;
            OrderReadRepository = orderReadRepository;
            orderProjection = new OrderProjection(orderReadRepository);
        }



        public void Handle(CreateOrderCommand command)
        {
            // domain object
            var order = new Order(command.OrderId, command.CustomerId);

            //validate & apply busines logic

            // domain event
            var orderCreatedEvent = new OrderCreatedEvent(order.OrderId, order.CustomerId);
            var events = new List<IEvent> { orderCreatedEvent };

            _eventStore.Add(command.OrderId, events);

            // reaise event to update read model
            orderProjection.ApplyEvent(orderCreatedEvent);
        }

        public void Handle(AddItemCommand command)
        {
            // save in event store db
            var itemAddedEvent = new ItemAddedEvent(command.OrderId, command.Item);
            _eventStore[command.OrderId].Add(itemAddedEvent);

            // raise event & update read model
            orderProjection.ApplyEvent(itemAddedEvent);
        }

        public void Handle(ShipOrderCommand command)
        {
            // save in event store db
            var orderShippedEvent = new OrderShippedEvent(command.OrderId);
            _eventStore[command.OrderId].Add(orderShippedEvent);


            // raise event & update read model
            orderProjection.ApplyEvent(orderShippedEvent);
        }

        public List<IEvent> GetHistory(Guid orderId)
        {
            return _eventStore[orderId];
        }
    }
}
