using Fuse8_ByteMinds.SummerSchool.PublicApi.Exceptions;
using Fuse8_ByteMinds.SummerSchool.PublicApi.Models;
using Microsoft.AspNetCore.Mvc;
using Newtonsoft.Json.Linq;

namespace Fuse8_ByteMinds.SummerSchool.PublicApi.Controllers
{
    /// <summary>
    /// Методы для обращения к внешнему API https://api.currencyapi.com
    /// </summary>
    [Route("api/[controller]/")]
    [ApiController]
    public class CurrencyController : ControllerBase
    {
        private readonly CurrencyHttpClient _httpClient;
        private readonly CurrencySettings _settings;

        public CurrencyController(CurrencyHttpClient httpClient, IConfiguration configuration)
        {
            _httpClient = httpClient;

            _settings = configuration.GetRequiredSection("CurrencySettings").Get<CurrencySettings>();
        }

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
            => await GetLatestAsync(_settings.DefaultCurrency, cancellationToken);

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
        {
            await CheckRequestLimit(cancellationToken);

            var uriBuilder = new CurrencyApiUriBuilder(_settings);
            uriBuilder.AddPath("latest");

            currencyCode = currencyCode.ToUpper();
            uriBuilder.AddQuery("currencies", currencyCode);

            var response = await _httpClient.GetStringAsync(uriBuilder.ToString(), cancellationToken);

            dynamic deserialisedObject = JObject.Parse(response);

            return new GetCurrencyResponse()
            {
                Code = currencyCode,
                Value = Math.Round((decimal)deserialisedObject.data[currencyCode].value, _settings.CurrencyRoundCount)
            };
        }

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
        {
            await CheckRequestLimit(cancellationToken);

            var uriBuilder = new CurrencyApiUriBuilder(_settings);
            uriBuilder.AddPath("historical");

            currencyCode = currencyCode.ToUpper();
            uriBuilder.AddQuery("currencies", currencyCode);
            uriBuilder.AddQuery("date", date.ToString());

            var response = await _httpClient.GetStringAsync(uriBuilder.ToString(), cancellationToken);

            dynamic deserialisedObject = JObject.Parse(response);

            return new GetCurrencyHistoricalResponse()
            {
                Code = currencyCode,
                Value = Math.Round((decimal)deserialisedObject.data[currencyCode].value, _settings.CurrencyRoundCount),
                Date = ((DateTime)deserialisedObject.meta.last_updated_at).Date.ToString("yyyy-mm-dd")
            };
        }

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
        {
            var uriBuilder = new CurrencyApiUriBuilder(_settings);
            uriBuilder.AddPath("status");

            var response = await _httpClient.GetStringAsync(uriBuilder.ToString(), cancellationToken);

            dynamic deserialisedObject = JObject.Parse(response);

            return new GetSettingsResponse()
            {
                DefaultCurrency = _settings.DefaultCurrency,
                BaseCurrency = _settings.BaseCurrency,
                RequestLimit = deserialisedObject.quotas.month.total,
                RequestCount = deserialisedObject.quotas.month.used,
                CurrencyRoundCount = _settings.CurrencyRoundCount,
            };
        }

        /// <summary>
        /// Метод проверки достижения лимита запросов в месяц
        /// </summary>
        /// <param name="cancellationToken">токен отмены</param>
        /// <returns></returns>
        /// <exception cref="ApiRequestLimitException">Ошибка исчерпания количества обращений к внешнему API</exception>
        private async Task CheckRequestLimit(CancellationToken cancellationToken)
        {
            var requestLimitCheck = await GetSettingsAsync(cancellationToken);

            if (requestLimitCheck.RequestCount >= _settings.MaxRequestsPerMonth)
                throw new ApiRequestLimitException();
        }
    }
}
