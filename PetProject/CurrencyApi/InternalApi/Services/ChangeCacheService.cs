using Fuse8_ByteMinds.SummerSchool.InternalApi;
using InternalApi.Interfaces;
using InternalApi.Models.Entities;
using Microsoft.EntityFrameworkCore;
using Microsoft.Extensions.Caching.Memory;

namespace InternalApi.Services
{
    /// <summary>
    /// Сервис работы с задачами по пересчету кэша
    /// </summary>
    public class ChangeCacheService: IChangeCacheService
    {
        private readonly IAppDbContext _appDbContext;
        private readonly ISettingsService _settingsService;
        private readonly IMemoryCache _memoryCache;

        /// <summary>
        /// Конструктор для <see cref="ChangeCacheService"/>
        /// </summary>
        /// <param name="appDbContext">Контекст базы данных</param>
        /// <param name="settingsService">Сервис настроек приложения</param>
        /// <param name="memoryCache">Кеш памяти</param>
        public ChangeCacheService(IAppDbContext appDbContext, ISettingsService settingsService, IMemoryCache memoryCache)
        {
            _appDbContext = appDbContext;
            _settingsService = settingsService;
            _memoryCache = memoryCache;
        }

        /// <summary>
        /// Создание задачи по пересчету кэша
        /// </summary>
        /// <param name="currencyCode">Код новой базовой валюты</param>
        /// <param name="cancellationToken">Токен отмены</param>
        /// <returns>Идентификатор задачи</returns>
        public async Task<Guid> CreateChangeCacheTaskAsync(CurrencyCode currencyCode, CancellationToken cancellationToken)
        {
            var newTask = new ChangeCacheTask(currencyCode);

            _appDbContext.ChangeCacheTasks.Add(newTask);
            await _appDbContext.SaveChangesAsync(cancellationToken);

            return newTask.Id;
        }

        /// <summary>
        /// Пересчет кеша на новую валюту
        /// </summary>
        /// <param name="taskId">Идентификатор задачи по пересчету кеша</param>
        /// <param name="cancellationToken">Токен отмены</param>
        /// <returns></returns>
        public async Task ProcessChangeCacheTaskAsync(Guid taskId, CancellationToken cancellationToken)
        {
            var task = await _appDbContext.ChangeCacheTasks.FirstOrDefaultAsync(x => x.Id == taskId, cancellationToken);

            if (task == null)
                return;

            task.CacheTaskStatus = CacheTaskStatus.Processing;
            await _appDbContext.SaveChangesAsync(cancellationToken);

            var settings = await _settingsService.GetSettingsAsync(cancellationToken);

            if (settings.BaseCurrency != task.NewBaseCurrency)
            {
                var cache = await _appDbContext.CurrenciesOnDates.OrderByDescending(x => x.Date).ToListAsync(cancellationToken);

                foreach (var item in cache)
                {
                    var crossCourse = item.Currencies.First(c => c.Code.Equals(Enum.GetName(task.NewBaseCurrency), StringComparison.OrdinalIgnoreCase)).Value;

                    for (int i = 0; i < item.Currencies.Length; i++)
                        item.Currencies[i].Value = item.Currencies[i].Value / crossCourse;

                    //вызов сеттера для обновления json поля хранения данных
                    item.Currencies = item.Currencies;
                }

                settings.BaseCurrency = task.NewBaseCurrency;
            }
            
            task.CacheTaskStatus = CacheTaskStatus.Success;

            _memoryCache.Remove(Constants.CashedCurrencyData);

            await _appDbContext.SaveChangesAsync(cancellationToken);
        }
    }
}
