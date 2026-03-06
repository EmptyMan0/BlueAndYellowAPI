using Microsoft.AspNetCore.Mvc;
using WebApplication1.Models;
using WebApplication1.Models.Requests;
using WebApplication1.Models.Responses;
using WebApplication1.Services;

namespace WebApplication1.Controllers
{
    /// <summary>
    /// Контроллер для работы с Green API
    /// </summary>
    [ApiController]
    [Route("[controller]")]
    public class ApiController : ControllerBase
    {
        private readonly IGreenApiClient _greenApiClient;
        private readonly ILogger<ApiController> _logger;

        public ApiController(
            IGreenApiClient greenApiClient,
            ILogger<ApiController> logger)
        {
            _greenApiClient = greenApiClient;
            _logger = logger;
        }

        /// <summary>
        /// Получение текущих настроек инстанса.
        /// </summary>
        /// <param name="idInstance">Уникальный идентификатор инстанса</param>
        /// <param name="apiTokenInstance">Токен авторизации инстанса</param>
        /// <returns>Настройки экземпляра или ошибка</returns>
        [HttpGet("/getSettings")]
        public async Task<IActionResult> GetSettingsAsync(
            [FromQuery] string idInstance,
            [FromQuery] string apiTokenInstance)
        {
            try
            {
                if (string.IsNullOrEmpty(idInstance) || string.IsNullOrEmpty(apiTokenInstance))
                {
                    return BadRequest(new { error = "idInstance и apiTokenInstance обязательны" });
                }

                var settings = await _greenApiClient.GetAsync<object>(
                    idInstance,
                    apiTokenInstance,
                    "getSettings");

                return Ok(settings);
            }
            catch (Exception ex)
            {
                _logger.LogError(ex, "Error getting settings");
                return StatusCode(500, new { error = "Internal server error", message = ex.Message });
            }
        }

        /// <summary>
        /// Получение состояния инстанса.
        /// </summary>
        /// <param name="idInstance">Уникальный идентификатор инстанса</param>
        /// <param name="apiTokenInstance">Токен авторизации инстанса</param>
        /// <returns>Состояние инстанса или ошибка</returns>
        [HttpGet("/getStateInstance")]
        public async Task<IActionResult> GetStateInstanceAsync(
            [FromQuery] string idInstance,
            [FromQuery] string apiTokenInstance)
        {
            try
            {
                if (string.IsNullOrEmpty(idInstance) || string.IsNullOrEmpty(apiTokenInstance))
                {
                    return BadRequest(new { error = "idInstance и apiTokenInstance обязательны" });
                }

                var state = await _greenApiClient.GetAsync<object>(
                    idInstance,
                    apiTokenInstance,
                    "getStateInstance");

                return Ok(state);
            }
            catch (Exception ex)
            {
                _logger.LogError(ex, "Error getting state instance");
                return StatusCode(500, new { error = "Internal server error", message = ex.Message });
            }
        }

        /// <summary>
        /// Отправка текстового сообщения в личный или групповой чат.
        /// </summary>
        /// <param name="request">Данные для отправки сообщения (чат, текст, цитата)</param>
        /// <returns>Идентификатор отправленного сообщения или ошибка</returns>
        [HttpPost("/sendMessage")]
        public async Task<IActionResult> SendMessageAsync([FromBody] SendMessageRequest request)
        {
            try
            {
                var idInstance = Request.Headers["idInstance"].FirstOrDefault();
                var apiTokenInstance = Request.Headers["apiTokenInstance"].FirstOrDefault();

                if (string.IsNullOrEmpty(idInstance) || string.IsNullOrEmpty(apiTokenInstance))
                {
                    return BadRequest(new { error = "idInstance и apiTokenInstance обязательны в заголовках" });
                }

                if (string.IsNullOrEmpty(request.ChatId) || string.IsNullOrEmpty(request.Message))
                {
                    return BadRequest(new { error = "chatId и message обязательны" });
                }

                var chatId = FormatChatId(request.ChatId);

                var requestData = new
                {
                    chatId,
                    message = request.Message,
                    quotedMessageId = string.IsNullOrEmpty(request.QuotedMessageId) ? null : request.QuotedMessageId
                };

                var result = await _greenApiClient.PostAsync<object, IdMessageResponse>(
                    idInstance,
                    apiTokenInstance,
                    "sendMessage",
                    requestData);

                return Ok(result);
            }
            catch (Exception ex)
            {
                _logger.LogError(ex, "Error sending message");
                return StatusCode(500, new { error = "Internal server error", message = ex.Message });
            }
        }

        /// <summary>
        /// Отправка файла, загружаемого по ссылке.
        /// </summary>
        /// <param name="request">Данные для отправки файла (чат, URL, имя файла, описание)</param>
        /// <returns>Идентификатор отправленного сообщения или ошибка</returns>
        [HttpPost("/sendFileByUrl")]
        public async Task<IActionResult> SendFileByUrlAsync([FromBody] SendFileByUrlRequest request)
        {
            try
            {
                var idInstance = Request.Headers["idInstance"].FirstOrDefault();
                var apiTokenInstance = Request.Headers["apiTokenInstance"].FirstOrDefault();

                if (string.IsNullOrEmpty(idInstance) || string.IsNullOrEmpty(apiTokenInstance))
                {
                    return BadRequest(new { error = "idInstance и apiTokenInstance обязательны в заголовках" });
                }

                if (string.IsNullOrEmpty(request.ChatId) || string.IsNullOrEmpty(request.UrlFile) || string.IsNullOrEmpty(request.FileName))
                {
                    return BadRequest(new { error = "chatId, urlFile и fileName обязательны" });
                }

                var chatId = FormatChatId(request.ChatId);

                var requestData = new
                {
                    chatId,
                    urlFile = request.UrlFile,
                    fileName = request.FileName,
                    caption = string.IsNullOrEmpty(request.Caption) ? null : request.Caption,
                    quotedMessageId = string.IsNullOrEmpty(request.QuotedMessageId) ? null : request.QuotedMessageId
                };

                var result = await _greenApiClient.PostAsync<object, IdMessageResponse>(
                    idInstance,
                    apiTokenInstance,
                    "sendFileByUrl",
                    requestData);

                return Ok(result);
            }
            catch (Exception ex)
            {
                _logger.LogError(ex, "Error sending file by URL");
                return StatusCode(500, new { error = "Internal server error", message = ex.Message });
            }
        }

        private static string FormatChatId(string chatId)
        {
            if (chatId.Contains("@"))
                return chatId;
            return $"{chatId}@c.us";
        }
    }
}