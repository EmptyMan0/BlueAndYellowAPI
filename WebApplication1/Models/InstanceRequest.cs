using System.Text.Json.Serialization;

namespace WebApplication1.Models.Requests;

public class InstanceRequest
{
    [JsonPropertyName("idInstance")]
    public string? IdInstance { get; set; }

    [JsonPropertyName("apiTokenInstance")]
    public string? ApiTokenInstance { get; set; }
}