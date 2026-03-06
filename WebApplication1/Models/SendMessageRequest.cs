using System.Text.Json.Serialization;

namespace WebApplication1.Models.Requests;

public class SendMessageRequest
{
    [JsonPropertyName("chatId")]
    public string? ChatId { get; set; }

    [JsonPropertyName("message")]
    public string? Message { get; set; }

    [JsonPropertyName("quotedMessageId")]
    public string? QuotedMessageId { get; set; }
}