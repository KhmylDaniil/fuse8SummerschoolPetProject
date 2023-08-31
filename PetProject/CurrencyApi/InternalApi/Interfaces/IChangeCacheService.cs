using Fuse8_ByteMinds.SummerSchool.InternalApi;
using InternalApi.Models.Entities;

namespace InternalApi.Interfaces
{
    /// <summary>
    /// Интерфейс работы с задачами по пересчету кэша
    /// </summary>
    public interface IChangeCacheService
    {
        /// <summary>
        /// Создание задачи по пересчету кэша
        /// </summary>
        /// <param name="currencyCode">Код новой базовой валюты</param>
        /// <param name="cancellationToken">Токен отмены</param>
        /// <returns>Идентификатор задачи</returns>
        public Task<ChangeCacheTask> CreateChangeCacheTask(CurrencyCode currencyCode, CancellationToken cancellationToken);

        /// <summary>
        /// Пересчет кеша на новую валюту
        /// </summary>
        /// <param name="task">Задача</param>
        /// <param name="cancellationToken">Токен отмены</param>
        /// <returns></returns>
        public Task ProcessChangeCacheTask(ChangeCacheTask task, CancellationToken cancellationToken);
    }
}
