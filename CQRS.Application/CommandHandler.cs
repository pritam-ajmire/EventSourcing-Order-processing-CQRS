using CQRS.Domain;
using CQRS.ReadModel;

namespace CQRS.Application
{
    public class CommandHandler
    {
        //private readonly IEventStore _eventStore;
        private Dictionary<Guid, List<IEvent>> _eventStore = new Dictionary<Guid, List<IEvent>>();
        public IOrderReadRepository OrderReadRepository { get; }
        public CommandHandler(IOrderReadRepository orderReadRepository)
        {
            //_eventStore = eventStore;
            OrderReadRepository = orderReadRepository;
        }



        public void HandleAsync(CreateOrderCommand command)
        {
            var order = new Order(command.OrderId, command.CustomerId);

            var events = new List<IEvent> { new OrderCreatedEvent(order.OrderId, order.CustomerId) };


            _eventStore.Add(command.OrderId, events);

            // reaise event to update read model
            OrderReadRepository.Save(new OrderReadModel { OrderId = order.OrderId, CustomerId = order.CustomerId });
        }

        public void HandleAsync(AddItemCommand command)
        {
            _eventStore[command.OrderId].Add(new ItemAddedEvent(command.OrderId, command.Item));

            var order = OrderReadRepository.Get(command.OrderId);
            order.Items.Add(command.Item);
        }

        public void HandleAsync(ShipOrderCommand command)
        {

            _eventStore[command.OrderId].Add(new OrderShippedEvent(command.OrderId));

            var order = OrderReadRepository.Get(command.OrderId);
            order.IsShipped = true;

            //var order = await _eventStore.LoadAggregateAsync<Order>(command.OrderId);
            //order.Ship();
            //await _eventStore.AppendEventAsync(order.Id, new OrderShippedEvent(order.Id));
        }

        public List<IEvent> GetHistory(Guid orderId)
        {
            return _eventStore[orderId];
        }
    }
}
