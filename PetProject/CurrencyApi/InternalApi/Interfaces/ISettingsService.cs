using InternalApi.Models.Entities;

namespace InternalApi.Interfaces
{
    /// <summary>
    /// Сервис изменения настроек приложения
    /// </summary>
    public interface ISettingsService
    {
        /// <summary>
        /// Получить настройки из базы данных
        /// </summary>
        /// <param name="cancellationToken">токен отмены</param>
        /// <returns>Настройки приложения</returns>
        public Task<Settings> GetSettingsAsync(CancellationToken cancellationToken);
    }
}
