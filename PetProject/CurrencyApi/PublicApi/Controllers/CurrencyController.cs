using Fuse8_ByteMinds.SummerSchool.PublicApi.Interfaces;
using Fuse8_ByteMinds.SummerSchool.PublicApi.Models.Responses;
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

        /// <summary>
        /// Конструктор для <see cref="CurrencyController"/>
        /// </summary>
        /// <param name="grpcClient">gRPC клиент</param>
        public CurrencyController(IGrpcClient grpcClient) => _gprcClient = grpcClient;

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
        public Task<GetCurrencyResponse> GetLatestAsync(CancellationToken cancellationToken)
            => _gprcClient.GetCurrencyResponseAsync(null, cancellationToken);

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
        public Task<GetCurrencyResponse> GetLatestAsync(CurrencyCode currencyCode, CancellationToken cancellationToken)
            => _gprcClient.GetCurrencyResponseAsync(currencyCode, cancellationToken);

        /// <summary>
        /// Получить избранный курс валюты по названию
        /// </summary>
        /// <param name="cancellationToken">Токен отмены</param>
        /// <param name="favCurName">Название</param>
        /// <response code="200">
        /// Возвращает если удалось получить избранный курс валюты
        /// </response>
        /// /// <response code="429">
        /// Возвращает если удалось не удалось получить доступ к API из-за исчерпания лимита
        /// </response>
        /// <response code="500">
        /// Возвращает при иной ошибке
        /// </response>
        /// <returns>Значение избранного курса валюты на последнюю дату</returns>
        [HttpGet("FavCur/{favCurName}")]
        public Task<GetFavoredCurrencyValueResponse> GetLatestFavoriteCurrencyAsync(string favCurName, CancellationToken cancellationToken)
            => _gprcClient.GetFavoredCurrencyAsync(favCurName, cancellationToken);

        /// <summary>
        /// Получить избранный курс валюты по названию на дату актуальности
        /// </summary>
        /// <param name="cancellationToken">Токен отмены</param>
        /// <param name="favCurName">Название</param>
        /// <param name="date">Дата актуальности курса</param>
        /// <response code="200">
        /// Возвращает если удалось получить избранный курс валюты
        /// </response>
        /// /// <response code="429">
        /// Возвращает если удалось не удалось получить доступ к API из-за исчерпания лимита
        /// </response>
        /// <response code="500">
        /// Возвращает при иной ошибке
        /// </response>
        /// <returns>Значение избранного курса валюты на последнюю дату</returns>
        [HttpGet("FavCur/{favCurName}/{date}")]
        public Task<GetFavoredCurrencyValueResponse> GetHistoricalFavoriteCurrencyAsync(string favCurName, DateOnly date, CancellationToken cancellationToken)
            => _gprcClient.GetFavoredCurrencyHistoricalAsync(favCurName, date, cancellationToken);

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
        public Task<GetCurrencyHistoricalResponse> GetHistoricalAsync(CurrencyCode currencyCode, DateOnly date, CancellationToken cancellationToken)
            => _gprcClient.GetHistoricalAsync(currencyCode, date, cancellationToken);

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
        public Task<GetSettingsResponse> GetSettingsAsync(CancellationToken cancellationToken)
            => _gprcClient.GetSettingsAsync(cancellationToken);
    }
}
