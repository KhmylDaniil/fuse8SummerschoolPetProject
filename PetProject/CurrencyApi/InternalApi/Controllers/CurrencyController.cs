using Fuse8_ByteMinds.SummerSchool.InternalApi;
using Fuse8_ByteMinds.SummerSchool.InternalApi.Models;
using InternalApi.Interfaces;
using InternalApi.Models;
using Microsoft.AspNetCore.Mvc;

namespace InternalApi.Controllers
{
    /// <summary>
    /// Контроллер-копия gRPC сервиса
    /// </summary>
    [Route("api/[controller]")]
    [ApiController]
    public class CurrencyController : ControllerBase
    {
        private readonly ICachedCurrencyService _cachedCurrencyAPI;
        private readonly IChangeCacheService _changeCacheService;
        private readonly IBackgroundTaskQueue _backgroundTaskQueue;

        public CurrencyController(
            ICachedCurrencyService cachedCurrencyAPI,
            IChangeCacheService changeCacheService,
            IBackgroundTaskQueue backgroundTaskQueue)
        {
            _cachedCurrencyAPI = cachedCurrencyAPI;
            _changeCacheService = changeCacheService;
            _backgroundTaskQueue = backgroundTaskQueue;
        }

        /// <summary>
        /// Получить курс валюты по коду
        /// </summary>
        /// <param name="cancellationToken">Токен отмены</param>
        /// <param name="currencyCode">Код валюты</param>
        /// <response code="200">
        /// Возвращает если удалось получить курс валюты
        /// </response>
        /// <response code="404">
        /// Возвращает при неизвестном коде валюты
        /// </response>
        /// /// <response code="429">
        /// Возвращает если удалось не удалось получить доступ к API из-за исчерпания лимита
        /// </response>
        /// <response code="500">
        /// Возвращает при иной ошибке
        /// </response>
        /// <returns>Ответ на запрос курса валюты на последнюю дату</returns>
        [HttpGet("{currencyCode}")]
        public async Task<CurrencyDTO> GetLatestAsync(CurrencyCode currencyCode, CancellationToken cancellationToken)
            => await _cachedCurrencyAPI.GetCurrentCurrencyAsync(currencyCode, cancellationToken);

        /// <summary>
        /// Получить курс валюты по коду с указанием даты актуальности
        /// </summary>
        /// <param name="cancellationToken">Токен отмены</param>
        /// <param name="currencyCode">Код валюты</param>
        /// <param name="date">Дата, на которую актуален курс</param>
        /// <response code="200">
        /// Возвращает если удалось получить курс валюты
        /// </response>
        /// <response code="404">
        /// Возвращает при неизвестном коде валюты
        /// </response>
        /// <response code="429">
        /// Возвращает если удалось не удалось получить доступ к API из-за исчерпания лимита
        /// </response>
        /// <response code="500">
        /// Возвращает при иной ошибке
        /// </response>
        /// <returns>Ответ на запрос курса валюты с указанием даты актуальности курса</returns>
        [HttpGet("{currencyCode}/{date}")]
        public async Task<CurrencyDTO> GetHistoricalAsync(CurrencyCode currencyCode, DateOnly date, CancellationToken cancellationToken)
            => await _cachedCurrencyAPI.GetCurrencyOnDateAsync(currencyCode, date, cancellationToken);

        /// <summary>
        /// Создать задачу пересчета кеша
        /// </summary>
        /// <param name="currencyCode">Новый код базовой валюты</param>
        /// <param name="cancellationToken">Токен отмены</param>
        /// <response code="202">
        /// Возвращает при принятии задачи
        /// </response>
        /// <response code="500">
        /// Возвращает при ошибке
        /// </response>
        /// <returns>Идентификатор задачи</returns>
        [HttpPost("changeCache/{currencyCode}")]
        public async Task<IActionResult> CreateChangeCacheTask(CurrencyCode currencyCode, CancellationToken cancellationToken)
        {
            var task = await _changeCacheService.CreateChangeCacheTask(currencyCode, cancellationToken);
            
            await _backgroundTaskQueue.QueueAsync(task);

            return Accepted(task.Id);
        }

        /// <summary>
        /// Запрос текущих настроек приложения
        /// </summary>
        /// <param name="cancellationToken">Токен отмены</param>
        /// <response code="200">
        /// Возвращает если удалось получить настройки из внешнего апи
        /// </response>
        /// <response code="500">
        /// Возвращает при иной ошибке
        /// </response>
        /// <returns>Ответ на запрос текущих настроек приложения</returns>
        [HttpGet("Settings")]
        public async Task<GetSettingsResponse> GetSettingsAsync(CancellationToken cancellationToken)
            => await _cachedCurrencyAPI.GetSettingsAsync(cancellationToken);

        /// <summary>
        /// Проверка связи с внешним апи
        /// </summary>
        /// <param name="cancellationToken">Токен отмены</param>
        /// <returns>Ответ на хелчек</returns>
        [HttpGet("Healthcheck")]
        public async Task<HealthCheckResponse> HealthCheck(CancellationToken cancellationToken)
            => await _cachedCurrencyAPI.HealthCheck(cancellationToken);
    }
}
