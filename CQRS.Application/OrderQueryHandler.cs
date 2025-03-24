using CQRS.ReadModel;

namespace CQRS.Application
{
    public class OrderQueryHandler
    {
        private readonly IOrderReadRepository _orderReadRepository;

        public OrderQueryHandler(IOrderReadRepository orderReadRepository)
        {
            _orderReadRepository = orderReadRepository;
        }

        public OrderReadModel? HandleAsync(GetOrderQuery query)
        {
            return _orderReadRepository.Get(query.OrderId);
        }

        //public async Task<List<OrderReadModel>> HandleAsync(GetAllOrdersQuery query)
        //{
        //    return await _orderReadRepository.GetAllAsync();
        //}
    }
}
