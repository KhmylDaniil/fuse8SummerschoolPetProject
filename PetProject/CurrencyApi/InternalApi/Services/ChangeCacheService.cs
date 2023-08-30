using Fuse8_ByteMinds.SummerSchool.InternalApi;
using InternalApi.Interfaces;
using InternalApi.Models.Entities;
using Microsoft.EntityFrameworkCore;

namespace InternalApi.Services
{
    /// <summary>
    /// Сервис работы с задачами по пересчету кэша
    /// </summary>
    public class ChangeCacheService: IChangeCacheService
    {
        private readonly IAppDbContext _appDbContext;
        private readonly ISettingsService _settingsService;

        public ChangeCacheService(IAppDbContext appDbContext, ISettingsService settingsService)
        {
            _appDbContext = appDbContext;
            _settingsService = settingsService;
        }

        /// <summary>
        /// Создание задачи по пересчету кэша
        /// </summary>
        /// <param name="currencyCode">Код новой базовой валюты</param>
        /// <param name="cancellationToken">Токен отмены</param>
        /// <returns>Идентификатор задачи</returns>
        public async Task<Guid> CreateChangeCacheTask(CurrencyCode currencyCode, CancellationToken cancellationToken)
        {
            var newTask = new ChangeCacheTask(currencyCode);

            _appDbContext.ChangeCacheTasks.Add(newTask);
            await _appDbContext.SaveChangesAsync(cancellationToken);

            return newTask.Id;
        }

        /// <summary>
        /// Пересчет кеша на новую валюту
        /// </summary>
        /// <param name="id">Идентификатор задачи</param>
        /// <param name="cancellationToken">Токен отмены</param>
        /// <returns></returns>
        public async Task ProcessChangeCacheTask(Guid id, CancellationToken cancellationToken)
        {
            var task = await _appDbContext.ChangeCacheTasks.FirstOrDefaultAsync(t => t.Id == id, cancellationToken);

            if (task is null) return;

            task.CacheTaskStatus = CacheTaskStatus.Processing;
            await _appDbContext.SaveChangesAsync(cancellationToken);

            var settings = await _settingsService.GetSettingsAsync(cancellationToken);

            if (settings.BaseCurrency == task.NewBaseCurrency)
            {
                task.CacheTaskStatus = CacheTaskStatus.Success;
                await _appDbContext.SaveChangesAsync(cancellationToken);

                return;
            }

            var cache = await _appDbContext.CurrenciesOnDates.OrderByDescending(x => x.Date).ToListAsync(cancellationToken);

            foreach (var item in cache)
            {
                var crossCourse = item.Currencies.FirstOrDefault(c => c.Code.Equals(Enum.GetName(task.NewBaseCurrency), StringComparison.OrdinalIgnoreCase)).Value;

                for(int i = 0; i < item.Currencies.Length; i++)
                    item.Currencies[i].Value = item.Currencies[i].Value / crossCourse;

                //вызов сеттера для обновления json поля для хранения данных
                item.Currencies = item.Currencies;
            }

            settings.BaseCurrency = task.NewBaseCurrency;
            task.CacheTaskStatus = CacheTaskStatus.Success;

            await _appDbContext.SaveChangesAsync(cancellationToken);
        }
    }
}
