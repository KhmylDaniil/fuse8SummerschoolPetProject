using Fuse8_ByteMinds.SummerSchool.InternalApi;
using InternalApi.Interfaces;
using Microsoft.AspNetCore.Mvc;

namespace InternalApi.Controllers
{
    /// <summary>
    /// Контроллер изменения базовой валюты кеша
    /// </summary>
    [Route("api/[controller]")]
    [ApiController]
    public class ChangeCacheController: ControllerBase
    {
        private readonly IChangeCacheService _changeCacheService;
        private readonly IBackgroundTaskQueue _backgroundTaskQueue;

        /// <summary>
        /// Конструктор для <see cref="ChangeCacheController"/>
        /// </summary>
        /// <param name="changeCacheService">Сервис работы с задачами по пересчету кеша</param>
        /// <param name="backgroundTaskQueue">Очередь фоновых задач по пересчету кеша</param>
        public ChangeCacheController(IChangeCacheService changeCacheService, IBackgroundTaskQueue backgroundTaskQueue)
        {
            _changeCacheService = changeCacheService;
            _backgroundTaskQueue = backgroundTaskQueue;
        }

        /// <summary>
        /// Создать задачу пересчета кеша
        /// </summary>
        /// <param name="currencyCode">Новый код базовой валюты</param>
        /// <param name="cancellationToken">Токен отмены</param>
        /// <response code="202">
        /// Возвращает при принятии задачи
        /// </response>
        /// <response code="500">
        /// Возвращает при ошибке
        /// </response>
        /// <returns>Идентификатор задачи</returns>
        [HttpPost("{currencyCode}")]
        public async Task<IActionResult> CreateChangeCacheTaskAsync(CurrencyCode currencyCode, CancellationToken cancellationToken)
        {
            var taskId = await _changeCacheService.CreateChangeCacheTaskAsync(currencyCode, cancellationToken);

            await _backgroundTaskQueue.QueueAsync(new Services.WorkItem(taskId));

            return Accepted(taskId);
        }
    }
}
