namespace JPNET_EF.Abstractions.Entity;

using System.ComponentModel.DataAnnotations.Schema;
using System.Text.Json.Serialization;

public class Order : IOrder
{
    public int Id { get; set; }
    
    [ForeignKey("Client_Id")]
    public int ClientId { get; set; }
    
    public Client Client { get; set; }
    
    [InverseProperty("Order")]
    public List<OrderedItem> Items { get; set; }
    
    public bool IsFinished { get; set; }
    
    [JsonInclude]
    public double? TotalPrice
    {
        get
        {
            try
            {
                return Items.Sum(i => i.Item.Price * i.Quantity);
            }
            catch (Exception ex)
            {
                return null;
            }
        }
    }

    [JsonInclude]
    public int? TotalQuantity
    {
        get
        {
            try
            {
                return Items.Sum(i => i.Quantity);
            }
            catch (Exception ex)
            {
                return null;
            }
        }
    }
}