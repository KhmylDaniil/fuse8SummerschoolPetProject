using Fuse8_ByteMinds.SummerSchool.InternalApi;
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

        /// <summary>
        /// Конструктор для <see cref="CurrencyController"/>
        /// </summary>
        /// <param name="cachedCurrencyAPI">Сервис получения курса валюты из кеша</param>
        public CurrencyController(ICachedCurrencyService cachedCurrencyAPI) => _cachedCurrencyAPI = cachedCurrencyAPI;

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
        public Task<CurrencyDto> GetLatestAsync(CurrencyCode currencyCode, CancellationToken cancellationToken)
            => _cachedCurrencyAPI.GetCurrentCurrencyAsync(currencyCode, cancellationToken);

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
        public Task<CurrencyDto> GetHistoricalAsync(CurrencyCode currencyCode, DateOnly date, CancellationToken cancellationToken)
            => _cachedCurrencyAPI.GetCurrencyOnDateAsync(currencyCode, date, cancellationToken);
    }
}
