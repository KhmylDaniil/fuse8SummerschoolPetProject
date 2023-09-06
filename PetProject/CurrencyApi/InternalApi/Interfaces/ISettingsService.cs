using InternalApi.Models.Entities;

namespace InternalApi.Interfaces
{
    /// <summary>
    /// Сервис изменения настроек приложения
    /// </summary>
    public interface ISettingsService
    {
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
