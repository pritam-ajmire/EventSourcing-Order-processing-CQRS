namespace CQRS.ReadModel
{
    public class OrderReadRepository : IOrderReadRepository
    {
        private readonly Dictionary<Guid, OrderReadModel> Orders = new Dictionary<Guid, OrderReadModel>();


        public OrderReadModel? Get(Guid orderId)
        {
            return Orders!.GetValueOrDefault(orderId, null);
        }

        public List<OrderReadModel> GetAllAsync()
        {
            return Orders!.Select(x => x.Value).ToList();
        }

        public void Save(OrderReadModel orderReadModel)
        {
            Orders.Add(orderReadModel.OrderId, orderReadModel);
        }

        public void Update(OrderReadModel order)
        {
            Orders[order.OrderId] = order;
        }
    }
}
