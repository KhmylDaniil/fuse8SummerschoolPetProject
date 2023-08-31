using InternalApi.Models.Entities;
using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.ChangeTracking;

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

        /// <summary>
        /// Сохранение изменений в контекст
        /// </summary>
        /// <param name="cancellationToken">Токен отмены</param>
        /// <returns></returns>
        Task<int> SaveChangesAsync(CancellationToken cancellationToken = default);

        /// <summary>
        /// Доступ к статусу в трекере изменений
        /// </summary>
        /// <typeparam name="TEntity">Тип сущности</typeparam>
        /// <param name="entity">Сущность в базе</param>
        /// <returns></returns>
        EntityEntry<TEntity> Entry<TEntity>(TEntity entity) where TEntity : class;
    }
}
