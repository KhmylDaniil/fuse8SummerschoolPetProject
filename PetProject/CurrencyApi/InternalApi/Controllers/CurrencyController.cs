using Fuse8_ByteMinds.SummerSchool.InternalApi.Models;
using Microsoft.AspNetCore.Mvc;

namespace Fuse8_ByteMinds.SummerSchool.InternalApi.Controllers
{
    /// <summary>
    /// Методы для обращения к внешнему API https://api.currencyapi.com
    /// </summary>
    [Route("api/[controller]/")]
    [ApiController]
    public class CurrencyController : ControllerBase
    {
        private readonly CurrencyHttpClient _httpClient;

        public CurrencyController(CurrencyHttpClient httpClient) => _httpClient = httpClient;

        /// <summary>
        /// Получить курс валюты по умолчанию
        /// </summary>
        /// <param name="cancellationToken">Токен отмены</param>
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
        [HttpGet]
        public async Task<GetCurrencyResponse> GetLatestDefaultCurrencyAsync(CancellationToken cancellationToken)
            => await _httpClient.GetLatestAsync(null, cancellationToken);

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
        public async Task<GetCurrencyResponse> GetLatestAsync(string currencyCode, CancellationToken cancellationToken)
            => await _httpClient.GetLatestAsync(currencyCode, cancellationToken);

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
        /// /// <response code="429">
        /// Возвращает если удалось не удалось получить доступ к API из-за исчерпания лимита
        /// </response>
        /// <response code="500">
        /// Возвращает при иной ошибке
        /// </response>
        /// <returns>Ответ на запрос курса валюты с указанием даты актуальности курса</returns>
        [HttpGet("{currencyCode}/{date}")]
        public async Task<GetCurrencyHistoricalResponse> GetHistoricalAsync(string currencyCode, DateTime date, CancellationToken cancellationToken)
            => await _httpClient.GetHistoricalAsync(currencyCode, date, cancellationToken);

        /// <summary>
        /// Запрос текущих настроек приложения
        /// </summary>
        /// <param name="cancellationToken">Токен отмены</param>
        /// <response code="200">
        /// Возвращает если удалось получить настройки
        /// </response>
        /// <response code="500">
        /// Возвращает при иной ошибке
        /// <returns>Ответ на запрос текущих настроек приложения</returns>
        [HttpGet("Settings")]
        public async Task<GetSettingsResponse> GetSettingsAsync(CancellationToken cancellationToken)
            => await _httpClient.GetSettingsAsync(cancellationToken);
    }
}
