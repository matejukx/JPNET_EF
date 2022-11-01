namespace JPNET_EF.Abstractions.dbo;

using System.Net;
using Entity;

public class OrderResource
{
    public int ClientId { get; set; }
    public IEnumerable<Item> Items { get; set; }
    
    public bool IsInternet { get; set; }
}