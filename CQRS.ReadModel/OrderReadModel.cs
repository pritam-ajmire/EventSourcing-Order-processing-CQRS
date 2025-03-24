namespace CQRS.ReadModel;

public class OrderReadModel
{
    public Guid OrderId { get; set; }
    public string CustomerId { get; set; }
    public List<string> Items { get; set; } = new List<string>();
    public bool IsShipped { get; set; }
}
