using System.Text.Json.Serialization;

namespace WebApplication1.Models.Responses;

/// <summary>
/// Ответ от Green API содержащий уникальный идентификатор отправленного сообщения
/// </summary>
public class IdMessageResponse
{
    /// <summary>
    /// Уникальный идентификатор сообщения
    /// </summary>
    [JsonPropertyName("idMessage")]
    public string? IdMessage { get; set; }
}