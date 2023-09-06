using InternalApi.Interfaces;
using System.Threading.Channels;

namespace InternalApi.Services
{
    /// <summary>
    /// Сервис очереди фоновых задач пересчета кеша
    /// </summary>
    public class BackgroundTaskQueue: IBackgroundTaskQueue
    {
        private readonly Channel<WorkItem> _queue;

        /// <summary>
        /// Конструктор для <see cref="BackgroundTaskQueue"/>
        /// </summary>
        public BackgroundTaskQueue()
        {
            var options = new BoundedChannelOptions(50)
            {
                FullMode = BoundedChannelFullMode.Wait
            };
            _queue = Channel.CreateBounded<WorkItem>(options);
        }

        /// <summary>
        /// Записать в очередь
        /// </summary>
        /// <param name="command">Модель для хранения идентификатора задачи</param>
        /// <returns></returns>
        public ValueTask QueueAsync(WorkItem command)
            => _queue.Writer.WriteAsync(command);

        /// <summary>
        /// Вывести из очереди
        /// </summary>
        /// <param name="cancellationToken">Токен отмены</param>
        /// <returns>Задача по пересчету кеша</returns>
        public ValueTask<WorkItem> DequeueAsync(CancellationToken cancellationToken)
            => _queue.Reader.ReadAsync(cancellationToken);
    }

    /// <summary>
    /// Модель для хранения идентификатора задачи
    /// </summary>
    /// <param name="TaskId">Идентификатор задачи</param>
    public record WorkItem(Guid TaskId);
}
