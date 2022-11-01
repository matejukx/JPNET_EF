namespace JPNET_EF.Abstractions.Entity;

using System.ComponentModel.DataAnnotations.Schema;
using System.Text.Json.Serialization;

public class OrderedItem
{ 
   public int Id { get; set; }
   
   [ForeignKey("Item_Id")]
   public int ItemId { get; set; }
   public Item Item { get; set; }
   
   public int Quantity { get; set; }
   
   [ForeignKey("Order_Id")]
   public int OrderId { get; set; }
   
   [JsonIgnore]
   public Order Order { get; set; }
}