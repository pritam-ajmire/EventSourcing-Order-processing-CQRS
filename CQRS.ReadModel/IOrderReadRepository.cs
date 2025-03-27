namespace CQRS.ReadModel
{
    public interface IOrderReadRepository
    {

        OrderReadModel? Get(Guid orderId);
        List<OrderReadModel> GetAllAsync();


        void Save(OrderReadModel orderReadModel);
        void Update(OrderReadModel order);
    }
}
