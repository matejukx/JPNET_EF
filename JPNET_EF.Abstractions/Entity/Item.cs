namespace JPNET_EF.Abstractions.Entity;

public class Item
{
    public int Id { get; set; }
    public double Price { get; set; }
    public string Name { get; set; }
    public string Description { get; set; }
    public long Quantity { get; set; }
}