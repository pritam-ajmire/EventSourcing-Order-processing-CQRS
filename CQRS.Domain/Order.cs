namespace CQRS.Domain
{
    // Domain Models
    public class Order
    {

        public Order(Guid orderId, Guid customerId)
        {
            OrderId = orderId;
            this.CustomerId = customerId.ToString();
        }

        public Guid OrderId { get; set; }
        public string CustomerId { get; set; }
        public List<string> Items { get; set; }
        public bool IsSubmitted { get; set; }
        public bool IsShipped { get; set; }
    }
}
