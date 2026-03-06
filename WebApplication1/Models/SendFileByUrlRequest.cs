using System.Text.Json.Serialization;

namespace WebApplication1.Models.Requests;

/// <summary>
/// Модель запроса для отправки файла по URL
/// </summary>
public class SendFileByUrlRequest
{
    /// <summary>
    /// Идентификатор чата (номер телефона или ID группы)
    /// </summary>
    [JsonPropertyName("chatId")]
    public string? ChatId { get; set; }

    /// <summary>
    /// Прямая ссылка на файл для отправки
    /// </summary>
    [JsonPropertyName("urlFile")]
    public string? UrlFile { get; set; }

    /// <summary>
    /// Имя файла с расширением
    /// </summary>
    [JsonPropertyName("fileName")]
    public string? FileName { get; set; }

    /// <summary>
    /// Подпись к файлу
    /// </summary>
    [JsonPropertyName("caption")]
    public string? Caption { get; set; }

    /// <summary>
    /// ID сообщения для цитирования
    /// </summary>
    [JsonPropertyName("quotedMessageId")]
    public string? QuotedMessageId { get; set; }
}