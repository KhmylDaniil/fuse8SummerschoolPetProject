using Fuse8_ByteMinds.SummerSchool.InternalApi.Models;
using InternalApi.Interfaces;
using InternalApi.Models;
using InternalApi.Services;
using Microsoft.AspNetCore.Mvc;

namespace InternalApi.Controllers
{
    /// <summary>
    /// Контроллер настроек и хелсчека
    /// </summary>
    [Route("api/[controller]")]
    [ApiController]
    public class SettingsController: ControllerBase
    {
        private readonly ICurrencyApi _currencyApi;

        /// <summary>
        /// Конструктор для <see cref="SettingsController"/>
        /// </summary>
        /// <param name="currencyApi">Сервис работы с внешним апи</param>
        public SettingsController(CurrencyHttpClient currencyApi) => _currencyApi = currencyApi;

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
            => _currencyApi.GetSettingsAsync(cancellationToken);

        /// <summary>
        /// Проверка связи с внешним апи
        /// </summary>
        /// <param name="cancellationToken">Токен отмены</param>
        /// <returns>Ответ на хелчек</returns>
        [HttpGet("ExternalApiHealth")]
        public Task<HealthCheckResponse> HealthCheckAsync(CancellationToken cancellationToken) => _currencyApi.HealthCheckAsync(cancellationToken);
    }
}
