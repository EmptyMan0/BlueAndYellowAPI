using System.Text.Json.Serialization;

namespace WebApplication1.Models.Requests;

public class SendFileByUrlRequest
{
    [JsonPropertyName("chatId")]
    public string? ChatId { get; set; }

    [JsonPropertyName("urlFile")]
    public string? UrlFile { get; set; }

    [JsonPropertyName("fileName")]
    public string? FileName { get; set; }

    [JsonPropertyName("caption")]
    public string? Caption { get; set; }

    [JsonPropertyName("quotedMessageId")]
    public string? QuotedMessageId { get; set; }
}