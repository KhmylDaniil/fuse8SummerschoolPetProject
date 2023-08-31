using Fuse8_ByteMinds.SummerSchool.PublicApi.Interfaces;
using Microsoft.AspNetCore.Mvc;

namespace Fuse8_ByteMinds.SummerSchool.PublicApi.Controllers
{
    /// <summary>
    /// Методы для изменения настроек приложения в базе данных
    /// </summary>
    [Route("api/[controller]/")]
    [ApiController]
    public class SettingsController : ControllerBase
    {
        private readonly ISettingsService _settingsService;

        /// <summary>
        /// Конструктор для <see cref="SettingsController"/>
        /// </summary>
        /// <param name="settingsService">Сервис работы с настройками</param>
        public SettingsController(ISettingsService settingsService)
        {
            _settingsService = settingsService;
        }

        /// <summary>
        /// Изменить код валюты по умолчанию
        /// </summary>
        /// <param name="currencyCode">Код валюты</param>
        /// <param name="cancellationToken">Токен отмены</param>
        /// <response code="200">
        /// Возвращает если удалось изменить код валюты
        /// </response>
        /// <response code="500">
        /// Возвращает при ошибке
        /// </response>
        /// <returns></returns>
        [HttpPut("changeCurrency")]
        public async Task ChangeDefaultCurrencyAsync(CurrencyCode currencyCode, CancellationToken cancellationToken)
            => await _settingsService.ChangeDefaultCurrencyAsync(currencyCode, cancellationToken);

        /// <summary>
        /// Изменить количество знаков для округления курса валюты
        /// </summary>
        /// <param name="round">Количество знаков округления</param>
        /// <param name="cancellationToken">Токен отмены</param>
        /// <response code="200">
        /// Возвращает если удалось изменить количество знаков для округления
        /// </response>
        /// <response code="500">
        /// Возвращает при ошибке
        /// </response>
        /// <returns></returns>
        [HttpPut("changeRoundCount")]
        public async Task ChangeCurrencyRoundCountAsync(int round, CancellationToken cancellationToken)
            => await _settingsService.ChangeCurrencyRoundCountAsync(round, cancellationToken);
    }
}
