using Fuse8_ByteMinds.SummerSchool.PublicApi.Models.Entities;

namespace Fuse8_ByteMinds.SummerSchool.PublicApi.Interfaces
{
    /// <summary>
    /// Сервис изменения настроек приложения
    /// </summary>
    public interface ISettingsService
    {
        /// <summary>
        /// Изменить валюту по умолчанию
        /// </summary>
        /// <param name="currencyCode">Код валюты</param>
        /// <param name="cancellationToken">Токен отмены</param>
        Task ChangeDefaultCurrencyAsync(CurrencyCode currencyCode, CancellationToken cancellationToken);

        /// <summary>
        /// Изменить точность округления
        /// </summary>
        /// <param name="currencyRoundCount">Количество знаков после запятой</param>
        /// <param name="cancellationToken">Токен отмены</param>
        Task ChangeCurrencyRoundCountAsync(int currencyRoundCount, CancellationToken cancellationToken);

        /// <summary>
        /// Получить настройки из базы данных с отслеживанием изменений
        /// </summary>
        /// <param name="cancellationToken">токен отмены</param>
        /// <returns>Настройки приложения</returns>
        Task<Settings> GetSettingsAsync(CancellationToken cancellationToken);

        /// <summary>
        /// Получить настройки из базы данных без отслеживания изменений
        /// </summary>
        /// <param name="cancellationToken">токен отмены</param>
        /// <returns>Настройки приложения</returns>
        Task<Settings> GetSettingsAsNoTrackingAsync(CancellationToken cancellationToken);
    }
}
