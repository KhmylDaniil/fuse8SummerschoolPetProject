using InternalApi.Models.Entities;
using Microsoft.EntityFrameworkCore;

namespace InternalApi.Interfaces
{
    /// <summary>
    /// контекст базы данных
    /// </summary>
    public interface IAppDbContext
    {
        /// <summary>
        /// Данные о курсах валюты на дату
        /// </summary>
        DbSet<CurrenciesOnDate> CurrenciesOnDates { get; }

        /// <summary>
        /// Задачи изменения базовой валюты кэша
        /// </summary>
        DbSet<ChangeCacheTask> ChangeCacheTasks { get; }

        /// <summary>
        /// Настройки приложения в базе данных
        /// </summary>
        DbSet<Settings> Settings { get; }

        Task<int> SaveChangesAsync(CancellationToken cancellationToken = default);
    }
}
