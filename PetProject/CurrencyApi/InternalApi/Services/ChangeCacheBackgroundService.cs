using InternalApi.Interfaces;
using Microsoft.EntityFrameworkCore;

namespace InternalApi.Services
{
    /// <summary>
    /// Сервис обработки фоновых задач по пересчету кеша
    /// </summary>
    public class ChangeCacheBackgroundService : BackgroundService
    {
        private readonly IServiceProvider _serviceProvider;
        private readonly ILogger<ChangeCacheBackgroundService> _logger;
        private readonly IBackgroundTaskQueue _queue;

        /// <summary>
        /// Конструктор для <see cref="ChangeCacheBackgroundService"/>
        /// </summary>
        /// <param name="serviceProvider">Провайдер сервисов</param>
        /// <param name="logger">Логгер</param>
        /// <param name="queue">Сервис очереди фоновых задач</param>
        public ChangeCacheBackgroundService(
            IServiceProvider serviceProvider,
            ILogger<ChangeCacheBackgroundService> logger,
            IBackgroundTaskQueue queue)
        {
            _serviceProvider = serviceProvider;
            _logger = logger;
            _queue = queue;
        }

        /// <summary>
        /// Внесение последней неисполненной задачи по пересчету кеша в очередь при старте приложения
        /// </summary>
        /// <param name="cancellationToken">Токен отмены</param>
        /// <returns></returns>
        public override async Task StartAsync(CancellationToken cancellationToken)
        {
            using var scope = _serviceProvider.CreateScope();

            var dbContext = scope.ServiceProvider.GetRequiredService<AppDbContext>();

            var tasks = await dbContext.ChangeCacheTasks
                .Where(t => t.CacheTaskStatus < Models.Entities.CacheTaskStatus.Success)
                .OrderByDescending(t => t.CreationTime)
                .ToListAsync(cancellationToken);

            if (tasks.Any())
            {
                for (int i = 1; i < tasks.Count; i++)
                    tasks[i].CacheTaskStatus = Models.Entities.CacheTaskStatus.Canceled;

                await dbContext.SaveChangesAsync(cancellationToken);

                await _queue.QueueAsync(new WorkItem(tasks[0].Id));
            }

            await base.StartAsync(cancellationToken);
        }

        /// <summary>
        /// Обработка фоновых задач по пересчету кеша
        /// </summary>
        /// <param name="stoppingToken">Токен отмены</param>
        /// <returns></returns>
        protected override async Task ExecuteAsync(CancellationToken stoppingToken)
        {
            while(!stoppingToken.IsCancellationRequested)
            {
                var command = await _queue.DequeueAsync(stoppingToken);

                try
                {
                    using var scope = _serviceProvider.CreateScope();
                    var workerService = scope.ServiceProvider.GetRequiredService<IChangeCacheService>();
                    await workerService.ProcessChangeCacheTaskAsync(command.TaskId, stoppingToken);
                }
                catch (Exception ex)
                {
                    using var scope = _serviceProvider.CreateScope();
                    
                    var dbContext = scope.ServiceProvider.GetRequiredService<AppDbContext>();

                    var task = await dbContext.ChangeCacheTasks.FirstOrDefaultAsync(x => x.Id == command.TaskId, stoppingToken);

                    task.CacheTaskStatus = Models.Entities.CacheTaskStatus.Error;
                    await dbContext.SaveChangesAsync(stoppingToken);

                    _logger.LogError(ex, "Ошибка при пересчете кеша");
                }
            }
        }
    }
}
