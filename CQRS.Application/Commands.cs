namespace CQRS.Application;

public class CreateOrderCommand
{
    public Guid OrderId { get; }
    public Guid CustomerId { get; }

    public CreateOrderCommand(Guid orderId, Guid customerId)
    {
        OrderId = orderId;
        CustomerId = customerId;
    }
}

public class AddItemCommand
{
    public Guid OrderId { get; set; }
    public string Item { get; }

    public AddItemCommand(Guid orderId, string item)
    {
        OrderId = orderId;
        Item = item;
    }
}

public class SubmitOrderCommand
{
    public Guid OrderId { get; set; }
}

public class ShipOrderCommand
{
    public Guid OrderId { get; set; }
}