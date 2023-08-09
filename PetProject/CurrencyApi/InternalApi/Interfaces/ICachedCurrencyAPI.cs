using Fuse8_ByteMinds.SummerSchool.InternalApi;
using Fuse8_ByteMinds.SummerSchool.InternalApi.Models;
using InternalApi.Models;

namespace InternalApi.Interfaces
{
    public interface ICachedCurrencyAPI
    {
        /// <summary>
        /// Получает текущий курс
        /// </summary>
        /// <param name="currencyType">Валюта, для которой необходимо получить курс</param>
        /// <param name="cancellationToken">Токен отмены</param>
        /// <returns>Текущий курс</returns>
        Task<CurrencyDTO> GetCurrentCurrencyAsync(CurrencyCode currencyCode, CancellationToken cancellationToken);

        /// <summary>
        /// Получает курс валюты, актуальный на <paramref name="date"/>
        /// </summary>
        /// <param name="currencyType">Валюта, для которой необходимо получить курс</param>
        /// <param name="date">Дата, на которую нужно получить курс валют</param>
        /// <param name="cancellationToken">Токен отмены</param>
        /// <returns>Курс на дату</returns>
        Task<CurrencyDTO> GetCurrencyOnDateAsync(CurrencyCode currencyCode, DateOnly date, CancellationToken cancellationToken);

        /// <summary>
        /// Показывает настройки приложения
        /// </summary>
        /// <param name="cancellationToken">Токен отмены</param>
        /// <returns>Настройки приложения</returns>
        Task<GetSettingsResponse> GetSettingsAsync(CancellationToken cancellationToken);
    }
}
