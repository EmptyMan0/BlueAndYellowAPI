using System.Text.Json.Serialization;

namespace WebApplication1.Models.Requests;

/// <summary>
/// Модель запроса для отправки текстового сообщения
/// </summary>
public class SendMessageRequest
{
    /// <summary>
    /// Идентификатор чата
    /// </summary>
    [JsonPropertyName("chatId")]
    public string? ChatId { get; set; }

    /// <summary>
    /// Текст сообщения для отправки
    /// </summary>
    [JsonPropertyName("message")]
    public string? Message { get; set; }

    /// <summary>
    /// ID сообщения для цитирования
    /// </summary>
    [JsonPropertyName("quotedMessageId")]
    public string? QuotedMessageId { get; set; }
}