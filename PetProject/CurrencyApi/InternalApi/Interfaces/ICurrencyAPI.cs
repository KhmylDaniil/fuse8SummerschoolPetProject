using Fuse8_ByteMinds.SummerSchool.InternalApi.Models;
using Fuse8_ByteMinds.SummerSchool.InternalApi.Models.ExternalApiResponseModels;
using InternalApi.Models;

namespace InternalApi.Interfaces
{
    /// <summary>
    /// Сервис получения курсов валют из внешнего апи
    /// </summary>
    public interface ICurrencyApi
    {
        /// <summary>
        /// Получает текущий курс для всех валют
        /// </summary>
        /// <param name="baseCurrency">Базовая валюта, относительно которой необходимо получить курс</param>
        /// <param name="cancellationToken">Токен отмены</param>
        /// <returns>Список курсов валют</returns>
        public Task<Currency[]> GetAllCurrentCurrenciesAsync(string baseCurrency, CancellationToken cancellationToken);

        /// <summary>
        /// Получает курс для всех валют, актуальный на <paramref name="date"/>
        /// </summary>
        /// <param name="baseCurrency">Базовая валюта, относительно которой необходимо получить курс</param>
        /// <param name="date">Дата, на которую нужно получить курс валют</param>
        /// <param name="cancellationToken">Токен отмены</param>
        /// <returns>Список курсов валют на дату</returns>
        public Task<CurrenciesOnDate> GetAllCurrenciesOnDateAsync(string baseCurrency, DateOnly date, CancellationToken cancellationToken);

        /// <summary>
        /// Показывает настройки приложения
        /// </summary>
        /// <param name="cancellationToken">Токен отмены</param>
        /// <returns>Настройки приложения</returns>
        public Task<GetSettingsResponse> GetSettingsAsync(CancellationToken cancellationToken);

        /// <summary>
        /// Проверка связи с внешним апи
        /// </summary>
        /// <param name="cancellationToken">Токен отмены</param>
        /// <returns>Ответ на хелчек</returns>
        public Task<HealthCheckResponse> HealthCheck(CancellationToken cancellationToken);
    }
}
