namespace JPNET_EF.Abstractions.dbo;

using System.ComponentModel.DataAnnotations;
using System.Text.Json.Serialization;

public class ClientResource
{
    [Required]
    [JsonPropertyName("name")]
    public string Name { get; set; }
    
    [Required]
    [JsonPropertyName("address")]
    public string Address { get; set; }
    
    [JsonPropertyName("ipAddress")]
    public string? IpAddress { get; set; }
}