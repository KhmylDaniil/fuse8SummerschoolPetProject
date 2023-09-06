using InternalApi.Services;

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
        /// <param name="command">Модель с идентификатором задачи</param>
        /// <returns></returns>
        public ValueTask QueueAsync(WorkItem command);

        /// <summary>
        /// Вывести из очереди
        /// </summary>
        /// <param name="cancellationToken">Токен отмены</param>
        /// <returns>Модель с идентификатором задачи по пересчету кеша</returns>
        public ValueTask<WorkItem> DequeueAsync(CancellationToken cancellationToken);
    }
}
