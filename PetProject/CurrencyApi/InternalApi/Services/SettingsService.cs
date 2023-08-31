using InternalApi.Interfaces;
using InternalApi.Models.Entities;
using Microsoft.EntityFrameworkCore;

namespace InternalApi.Services
{
    /// <summary>
    /// Сервис получения настроек приложения
    /// </summary>
    public class SettingsService: ISettingsService
    {
        private readonly IAppDbContext _appDbContext;

        /// <summary>
        /// Конструктор для <see cref="SettingsService"/>
        /// </summary>
        /// <param name="appDbContext">Контекст базы данных</param>
        public SettingsService(IAppDbContext appDbContext) => _appDbContext = appDbContext;

        /// <summary>
        /// Получение настроек из базы данных
        /// </summary>
        /// <param name="cancellationToken">токен отмены</param>
        /// <returns>Настройки приложения</returns>
        public async Task<Settings> GetSettingsAsync(CancellationToken cancellationToken)
        {
            var settings = await _appDbContext.Settings.ToListAsync(cancellationToken);

            if (settings.Count != 1)
                throw new ArgumentException("Не найдены необходимые настройки приложения.");

            return settings[0];
        }
    }
}
