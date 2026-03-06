using System.Text.Json.Serialization;

namespace WebApplication1.Models.Responses;

public class IdMessageResponse
{
    [JsonPropertyName("idMessage")]
    public string? IdMessage { get; set; }
}