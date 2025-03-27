namespace CQRS.ReadModel;

public class OrderReadModel
{
    public Guid OrderId { get; set; }
    public string CustomerId { get; set; }
    public List<string> Items { get; set; } = new List<string>();
    public bool IsSubmitted { get; set; }
    public bool IsShipped { get; set; }
}

// Can create other read models for Payment, for shipment etc