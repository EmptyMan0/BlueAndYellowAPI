using System.Text.Json.Serialization;

namespace WebApplication1.Models.Requests;

/// <summary>
/// Модель запроса содержащая данные инстанса
/// </summary>
public class InstanceRequest
{
    /// <summary>
    /// Уникальный идентификатор инстанса
    /// </summary>
    [JsonPropertyName("idInstance")]
    public string? IdInstance { get; set; }

    /// <summary>
    /// Токен авторизации инстанса
    /// </summary>
    [JsonPropertyName("apiTokenInstance")]
    public string? ApiTokenInstance { get; set; }
}