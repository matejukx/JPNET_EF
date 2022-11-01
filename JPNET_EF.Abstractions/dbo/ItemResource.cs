namespace JPNET_EF.Abstractions.dbo;

public class ItemResource
{
    public int Id { get; set; }
    public double Price { get; set; }
    public string Name { get; set; }
    public string Description { get; set; }
    public long Quantity { get; set; }
}