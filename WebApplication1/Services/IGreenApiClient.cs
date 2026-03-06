namespace WebApplication1.Services;

/// <summary>
/// Интерфейс клиента для работы с Green API
/// </summary>
public interface IGreenApiClient
{
    Task<TResponse> GetAsync<TResponse>(string idInstance, string apiTokenInstance, string endpoint);

    Task<TResponse> PostAsync<TRequest, TResponse>(
        string idInstance,
        string apiTokenInstance,
        string endpoint,
        TRequest data);

    Task<TResponse> UploadFileAsync<TResponse>(
        string idInstance,
        string apiTokenInstance,
        string endpoint,
        Stream fileStream,
        string fileName,
        string chatId,
        string? caption = null);
}