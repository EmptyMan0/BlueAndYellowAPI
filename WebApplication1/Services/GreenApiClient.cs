using System.Net.Http.Headers;
using System.Text;
using System.Text.Json;

namespace WebApplication1.Services
{
    public class GreenApiClient : IGreenApiClient
    {
        private readonly HttpClient _httpClient;
        private readonly ILogger<GreenApiClient> _logger;
        private const string BaseUrl = "https://4100.api.green-api.com";

        public GreenApiClient(HttpClient httpClient, ILogger<GreenApiClient> logger)
        {
            _httpClient = httpClient;
            _logger = logger;
        }

        public async Task<TResponse> GetAsync<TResponse>(
            string idInstance,
            string apiTokenInstance,
            string endpoint)
        {
            var url = $"{BaseUrl}/waInstance{idInstance}/{endpoint}/{apiTokenInstance}";

            _logger.LogInformation("GET Request: {Url}", url);

            var response = await _httpClient.GetAsync(url);
            var responseContent = await response.Content.ReadAsStringAsync();

            if (!response.IsSuccessStatusCode)
            {
                _logger.LogError("Request failed: {StatusCode} - {Content}", response.StatusCode, responseContent);
                throw new HttpRequestException($"Request failed: {response.StatusCode} - {responseContent}");
            }

            return JsonSerializer.Deserialize<TResponse>(responseContent)
                ?? throw new JsonException("Failed to deserialize response");
        }

        public async Task<TResponse> PostAsync<TRequest, TResponse>(
            string idInstance,
            string apiTokenInstance,
            string endpoint,
            TRequest data)
        {
            var url = $"{BaseUrl}/waInstance{idInstance}/{endpoint}/{apiTokenInstance}";

            var json = JsonSerializer.Serialize(data);
            var content = new StringContent(json, Encoding.UTF8, "application/json");

            _logger.LogInformation("POST Request: {Url} with body: {Body}", url, json);

            var response = await _httpClient.PostAsync(url, content);
            var responseContent = await response.Content.ReadAsStringAsync();

            if (!response.IsSuccessStatusCode)
            {
                _logger.LogError("Request failed: {StatusCode} - {Content}", response.StatusCode, responseContent);
                throw new HttpRequestException($"Request failed: {response.StatusCode} - {responseContent}");
            }

            return JsonSerializer.Deserialize<TResponse>(responseContent)
                ?? throw new JsonException("Failed to deserialize response");
        }

        public async Task<TResponse> UploadFileAsync<TResponse>(
            string idInstance,
            string apiTokenInstance,
            string endpoint,
            Stream fileStream,
            string fileName,
            string chatId,
            string? caption = null)
        {
            var url = $"{BaseUrl}/waInstance{idInstance}/{endpoint}/{apiTokenInstance}";

            using var multipartContent = new MultipartFormDataContent();

            var fileContent = new StreamContent(fileStream);
            fileContent.Headers.ContentType = new MediaTypeHeaderValue("application/octet-stream");
            multipartContent.Add(fileContent, "file", fileName);

            multipartContent.Add(new StringContent(chatId), "chatId");

            if (!string.IsNullOrEmpty(caption))
            {
                multipartContent.Add(new StringContent(caption), "caption");
            }

            _logger.LogInformation("Upload File Request: {Url} fileName: {FileName}", url, fileName);

            var response = await _httpClient.PostAsync(url, multipartContent);
            var responseContent = await response.Content.ReadAsStringAsync();

            if (!response.IsSuccessStatusCode)
            {
                _logger.LogError("Request failed: {StatusCode} - {Content}", response.StatusCode, responseContent);
                throw new HttpRequestException($"Request failed: {response.StatusCode} - {responseContent}");
            }

            return JsonSerializer.Deserialize<TResponse>(responseContent)
                ?? throw new JsonException("Failed to deserialize response");
        }
    }
}