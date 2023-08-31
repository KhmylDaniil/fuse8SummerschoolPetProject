using InternalApi.Interfaces;
using InternalApi.Models.Entities;
using System.Threading.Channels;

namespace InternalApi.Services
{
    /// <summary>
    /// Сервис очереди фоновых задач пересчета кеша
    /// </summary>
    public class BackgroundTaskQueue: IBackgroundTaskQueue
    {
        private readonly Channel<ChangeCacheTask> _queue;

        public BackgroundTaskQueue()
        {
            var options = new BoundedChannelOptions(50)
            {
                FullMode = BoundedChannelFullMode.Wait
            };
            _queue = Channel.CreateBounded<ChangeCacheTask>(options);
        }

        /// <summary>
        /// Записать в очередь
        /// </summary>
        /// <param name="task">Задача пересчета кеша</param>
        /// <returns></returns>
        public ValueTask QueueAsync(ChangeCacheTask task)
            => _queue.Writer.WriteAsync(task);

        /// <summary>
        /// Вывести из очереди
        /// </summary>
        /// <param name="cancellationToken">Токен отмены</param>
        /// <returns>Задача по пересчету кеша</returns>
        public ValueTask<ChangeCacheTask> DequeueAsync(CancellationToken cancellationToken)
            => _queue.Reader.ReadAsync(cancellationToken);
    }
}
