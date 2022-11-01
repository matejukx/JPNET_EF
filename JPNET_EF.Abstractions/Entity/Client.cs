namespace JPNET_EF.Abstractions.Entity;

using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;
using System.Text.Json.Serialization;

public class Client
{
    [Key]
    public int Id { get; set; }

    [Required]
    public string Name { get; set; }

    [Required]
    public string Address { get; set; }

    [InverseProperty("Client")]
    [JsonIgnore]
    public List<Order> Orders { get; set; }
}