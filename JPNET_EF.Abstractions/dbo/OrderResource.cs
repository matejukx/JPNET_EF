namespace JPNET_EF.Abstractions.dbo;

using Entity;

public class OrderResource
{
    public int ClientId { get; set; }
    public IEnumerable<Item> Items { get; set; }
}