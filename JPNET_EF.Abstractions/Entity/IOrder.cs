namespace JPNET_EF.Abstractions.Entity;

public interface IOrder
{
    public int Id { get; set; }
    public Client? Client { get; set; }
    
    public List<OrderedItem> Items { get; set; }

    public bool IsFinished { get; set; }

    public double? TotalPrice { get; }
    
    public int? TotalQuantity { get; }
}