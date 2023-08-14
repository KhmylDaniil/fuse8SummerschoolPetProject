using Fuse8_ByteMinds.SummerSchool.PublicApi.Interfaces;
using Fuse8_ByteMinds.SummerSchool.PublicApi.Models;
using Microsoft.AspNetCore.Mvc;

namespace Fuse8_ByteMinds.SummerSchool.PublicApi.Controllers
{
    /// <summary>
    /// Методы для обращения к gRPC сервису
    /// </summary>
    [Route("api/[controller]/")]
    [ApiController]
    public class CurrencyController : ControllerBase
    {
        private readonly IGrpcClient _gprcClient;

        public CurrencyController(IGrpcClient grpcClient)
        {
            _gprcClient = grpcClient;
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
        public async Task<GetCurrencyResponse> GetLatestAsync(CancellationToken cancellationToken)
            => await _gprcClient.GetCurrencyResponseAsync(null, cancellationToken);

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
            => await _gprcClient.GetCurrencyResponseAsync(currencyCode, cancellationToken);

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
        public async Task<GetCurrencyHistoricalResponse> GetHistoricalAsync(string currencyCode, DateOnly date, CancellationToken cancellationToken)
            => await _gprcClient.GetHistoricalAsync(currencyCode, date, cancellationToken);

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
            => await _gprcClient.GetSettingsAsync(cancellationToken);
    }
}
