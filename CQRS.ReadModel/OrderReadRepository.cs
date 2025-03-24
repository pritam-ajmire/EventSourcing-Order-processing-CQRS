namespace CQRS.ReadModel
{
    public class OrderReadRepository : IOrderReadRepository
    {
        //private readonly OrderDbContext _context;
        private readonly Dictionary<Guid, OrderReadModel> Orders = new Dictionary<Guid, OrderReadModel>();
        //private readonly List<Order> Orders = new List<Order>();

        //public OrderReadRepository(OrderDbContext context)
        //{
        //    _context = context;
        //}

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
            //var currentOrder = Get(order.OrderId);
            Orders[order.OrderId] = order;
            //Orders[order.ord]
        }
    }
}
