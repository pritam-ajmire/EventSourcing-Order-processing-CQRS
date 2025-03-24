namespace CQRS.Domain
{
    public class OrderAggregate
    {
        public Guid OrderId { get; private set; }
        public string CustomerId { get; private set; }
        public List<string> Items { get; private set; } = new();
        public bool IsShipped { get; private set; }

        private readonly List<IEvent> _events = new();

        public OrderAggregate(Guid orderId, string customerId)
        {
            Apply(new OrderCreatedEvent(orderId, customerId));
        }

        public void AddItem(string item)
        {
            if (IsShipped) throw new InvalidOperationException("Order already shipped.");
            Apply(new ItemAddedEvent(OrderId, item));
        }

        public void ShipOrder()
        {
            if (IsShipped) throw new InvalidOperationException("Order already shipped.");
            Apply(new OrderShippedEvent(OrderId));
        }

        private void Apply(IEvent @event)
        {
            switch (@event)
            {
                case OrderCreatedEvent e:
                    OrderId = e.AggregateId;
                    CustomerId = e.CustomerId;
                    break;
                case ItemAddedEvent e:
                    Items.Add(e.Item);
                    break;
                case OrderShippedEvent e:
                    IsShipped = true;
                    break;
            }
            _events.Add(@event);
        }

        public List<IEvent> GetUncommittedEvents() => _events;
    }
}
