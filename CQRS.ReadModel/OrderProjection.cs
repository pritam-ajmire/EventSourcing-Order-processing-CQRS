﻿using CQRS.Domain;

namespace CQRS.ReadModel
{
    public class OrderProjection
    {
        private readonly IOrderReadRepository _repository;

        public OrderProjection(IOrderReadRepository repository)
        {
            _repository = repository;
        }

        public void ApplyEvent(IEvent @event)
        {
            switch (@event)
            {
                case OrderCreatedEvent e:
                    _repository.Save(new OrderReadModel { OrderId = e.OrderId, CustomerId = e.CustomerId, Items = new List<string>(), IsShipped = false });
                    break;
                case ItemAddedEvent e:
                    {
                        var order = _repository.Get(e.OrderId);
                        order.Items.Add(e.Item);
                        _repository.Update(order);
                        break;
                    }
                case OrderSubmittedEvent e:
                    {
                        var order = _repository.Get(e.OrderId);
                        order.IsSubmitted = true;
                        _repository.Update(order);
                        break;
                    }
                case OrderShippedEvent e:
                    {
                        var order = _repository.Get(e.OrderId);
                        order.IsShipped = true;
                        _repository.Update(order);
                        break;
                    }
            }
        }
    }
}
