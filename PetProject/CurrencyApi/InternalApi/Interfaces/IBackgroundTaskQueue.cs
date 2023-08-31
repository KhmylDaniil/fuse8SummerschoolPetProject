using InternalApi.Models.Entities;

namespace InternalApi.Interfaces
{
    /// <summary>
    /// Интерфейс очереди фоновых задач пересчета кеша
    /// </summary>
    public interface IBackgroundTaskQueue
    {
        /// <summary>
        /// Записать в очередь
        /// </summary>
        /// <param name="task">Задача пересчета кеша</param>
        /// <returns></returns>
        public ValueTask QueueAsync(ChangeCacheTask task);

        /// <summary>
        /// Вывести из очереди
        /// </summary>
        /// <param name="cancellationToken">Токен отмены</param>
        /// <returns>Задача по пересчету кеша</returns>
        public ValueTask<ChangeCacheTask> DequeueAsync(CancellationToken cancellationToken);
    }
}
