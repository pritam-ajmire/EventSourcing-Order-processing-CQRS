namespace CQRS.Application
{
    public class GetOrderQuery
    {
        public Guid OrderId { get; }

        public GetOrderQuery(Guid orderId)
        {
            OrderId = orderId;
        }
    }
}
