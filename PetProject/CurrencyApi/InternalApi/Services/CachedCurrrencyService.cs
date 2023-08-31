﻿using Fuse8_ByteMinds.SummerSchool.InternalApi;
using Fuse8_ByteMinds.SummerSchool.InternalApi.Models;
using InternalApi.Interfaces;
using InternalApi.Models;
using Microsoft.EntityFrameworkCore;
using Microsoft.Extensions.Caching.Memory;
using Microsoft.Extensions.Options;
using Fuse8_ByteMinds.SummerSchool.InternalApi.Exceptions;
using InternalApi.Models.Entities;
using System.Runtime.CompilerServices;

namespace InternalApi.Services
{
    /// <summary>
    /// Сервис для получения данных о курсе валюты с возможностью получения данных из кеша, базы данных и внешнего апи
    /// </summary>
    public class CachedCurrencyService : ICachedCurrencyService
    {
        private readonly IAppDbContext _appDbContext;
        private readonly ICurrencyApi _currencyAPI;
        private readonly IMemoryCache _memoryCache;
        private readonly ISettingsService _settingsService;
        private readonly CurrencySettings _settings;

        /// <summary>
        /// Конструктор
        /// </summary>
        /// <param name="appDbContext">Контекст базы данных</param>
        /// <param name="currencyAPI">Сервис доступа к внешнему апи</param>
        /// <param name="memoryCache">Кеш памяти</param>
        /// <param name="settings">Настройки приложения</param>
        public CachedCurrencyService(IAppDbContext appDbContext,
            CurrencyHttpClient currencyAPI,
            IMemoryCache memoryCache,
            ISettingsService settingsService,
            IOptionsSnapshot<CurrencySettings> settings)
        {
            _appDbContext = appDbContext;
            _currencyAPI = currencyAPI;
            _settings = settings.Value;
            _memoryCache = memoryCache;
            _settingsService = settingsService;
        }

        /// <summary>
        /// Метод получения курса валюты на дату
        /// </summary>
        /// <param name="currencyCode">Код валюты</param>
        /// <param name="date">Дата актуальности курса</param>
        /// <param name="cancellationToken">Токен отмены</param>
        /// <param name="dontRound">Флаг отмены округления</param>
        /// <returns>Модель курса валюты</returns>
        public async Task<CurrencyDTO> GetCurrencyOnDateAsync(CurrencyCode currencyCode, DateOnly date, CancellationToken cancellationToken, bool dontRound = default)
        {
            CurrenciesOnDate? data = null;

            DateTime actualityDateTime = date.ToDateTime(new TimeOnly(0, 0), DateTimeKind.Utc);

            if (_memoryCache.TryGetValue("cur", out CurrenciesOnDate? cachedOutput)
                && actualityDateTime >= cachedOutput?.Date
                && actualityDateTime.AddDays(1) <= cachedOutput?.Date)
                    data = cachedOutput;

            data ??= await _appDbContext.CurrenciesOnDates
                .Where(c => c.Date >= actualityDateTime && c.Date <= actualityDateTime.AddDays(1))
                .OrderBy(c => c.Date).LastOrDefaultAsync(cancellationToken);

            if (data == null)
            {
                await WaitIfChangeCacheTaskIsProcessing(cancellationToken);

                data = await _currencyAPI.GetAllCurrenciesOnDateAsync(date, cancellationToken);

                _appDbContext.CurrenciesOnDates.Add(data);
                await _appDbContext.SaveChangesAsync(cancellationToken);
            }

            return FindCurrencyByCode(currencyCode, data, dontRound);
        }

        /// <summary>
        /// Метод получения последнего курса валюты
        /// </summary>
        /// <param name="currencyCode">Код валюты</param>
        /// <param name="cancellationToken">Токен отмены</param>
        /// <param name="dontRound">Флаг отмены округления</param>
        /// <returns>Модель курса валюты</returns>
        public async Task<CurrencyDTO> GetCurrentCurrencyAsync(CurrencyCode currencyCode, CancellationToken cancellationToken, bool dontRound = default)
        {
            CurrenciesOnDate? currenciesOnDate = null;

            if (_memoryCache.TryGetValue("cur", out CurrenciesOnDate? cachedOutput) && DateTime.UtcNow - cachedOutput?.Date <= new TimeSpan(2, 0, 0))
                currenciesOnDate = cachedOutput;

            currenciesOnDate ??= await _appDbContext.CurrenciesOnDates.OrderBy(c => c.Date).LastOrDefaultAsync(cancellationToken);

            if (currenciesOnDate == null)
            {
                await WaitIfChangeCacheTaskIsProcessing(cancellationToken);

                var currencies = await _currencyAPI.GetAllCurrentCurrenciesAsync(cancellationToken);

                currenciesOnDate = new CurrenciesOnDate { Date = DateTime.UtcNow, Currencies = currencies };

                _appDbContext.CurrenciesOnDates.Add(currenciesOnDate);
                await _appDbContext.SaveChangesAsync(cancellationToken);
            }

            _memoryCache.Set("cur", currenciesOnDate);

            return FindCurrencyByCode(currencyCode, currenciesOnDate, dontRound);
        }

        /// <summary>
        /// Метод получения избранного курса валюты
        /// </summary>
        /// <param name="currency">Код валюты</param>
        /// <param name="baseCurrency">Код базовой валюты</param>
        /// <param name="cancellationToken">Токен отмены</param>
        /// <returns></returns>
        public async Task<float> GetFavoredCurrencyAsync(CurrencyCode currency, CurrencyCode baseCurrency, CancellationToken cancellationToken)
        {
            var currencyToCacheBaseCurrencyDTO = await GetCurrentCurrencyAsync(currency, cancellationToken, dontRound: true);

            var settingsFromDb = await _settingsService.GetSettingsAsync(cancellationToken);

            if (baseCurrency == settingsFromDb.BaseCurrency)
                return currencyToCacheBaseCurrencyDTO.Value;

            var baseCurrencyToCacheBaseCurrencyDTO = await GetCurrentCurrencyAsync(baseCurrency, cancellationToken, dontRound: true);

            return currencyToCacheBaseCurrencyDTO.Value / baseCurrencyToCacheBaseCurrencyDTO.Value;
        }

        /// <summary>
        /// Метод получения избранного курса валюты на дату актуальности
        /// </summary>
        /// <param name="currency">Код валюты</param>
        /// <param name="baseCurrency">Код базовой валюты</param>
        /// <param name="date">Дата актуальности курса</param>
        /// <param name="cancellationToken">Токен отмены</param>
        /// <returns></returns>
        public async Task<float> GetFavoredCurrencyHistoricalAsync(CurrencyCode currency, CurrencyCode baseCurrency, DateOnly date, CancellationToken cancellationToken)
        {
            var currencyToCacheBaseCurrencyDTO = await GetCurrencyOnDateAsync(currency, date, cancellationToken, dontRound: true);

            var settingsFromDb = await _settingsService.GetSettingsAsync(cancellationToken);

            if (baseCurrency == settingsFromDb.BaseCurrency)
                return currencyToCacheBaseCurrencyDTO.Value;

            var baseCurrencyToCacheBaseCurrencyDTO = await GetCurrencyOnDateAsync(baseCurrency, date, cancellationToken, dontRound: true);

            return currencyToCacheBaseCurrencyDTO.Value / baseCurrencyToCacheBaseCurrencyDTO.Value;
        }

        /// <summary>
        /// Метод получения настроек приложения
        /// </summary>
        /// <param name="cancellationToken">Токен отмены</param>
        /// <returns>Настройки приложения</returns>
        public async Task<GetSettingsResponse> GetSettingsAsync(CancellationToken cancellationToken)
            => await _currencyAPI.GetSettingsAsync(cancellationToken);

        /// <summary>
        /// Проверка связи с внешним апи
        /// </summary>
        /// <param name="cancellationToken">Токен отмены</param>
        /// <returns>Ответ на хелчек</returns>
        public Task<HealthCheckResponse> HealthCheck(CancellationToken cancellationToken)
            => _currencyAPI.HealthCheck(cancellationToken);

        /// <summary>
        /// Нахождение конкретной валюты по коду
        /// </summary>
        /// <param name="currencyCode">Код валюты</param>
        /// <param name="data">Данные о курсе валюты</param>
        /// <param name="dontRound">Флаг отмены округления</param>
        /// <returns>Курс валюты</returns>
        /// <exception cref="CurrencyNotFoundException"></exception>
        private CurrencyDTO FindCurrencyByCode(CurrencyCode currencyCode, CurrenciesOnDate data, bool dontRound)
        {
            var currency = data.Currencies.FirstOrDefault(c => c.Code.Equals(Enum.GetName(currencyCode), StringComparison.OrdinalIgnoreCase))
                ?? throw new CurrencyNotFoundException();

            return dontRound
                ? new CurrencyDTO(currencyCode, currency.Value)
                : new CurrencyDTO(currencyCode, (float)Math.Round(currency.Value, _settings.CurrencyRoundCount));
        }

        /// <summary>
        /// Проверка наличия задач по пересчету кеша и смене базовой валюты
        /// </summary>
        /// <param name="cancellationToken">Токен отмены</param>
        /// <returns></returns>
        private async Task WaitIfChangeCacheTaskIsProcessing(CancellationToken cancellationToken)
        {
            if (!await _appDbContext.ChangeCacheTasks.AnyAsync(t => t.CacheTaskStatus < CacheTaskStatus.Success, cancellationToken))
                return;

            await Task.Delay(10_000, cancellationToken);

            if (await _appDbContext.ChangeCacheTasks.AnyAsync(t => t.CacheTaskStatus < CacheTaskStatus.Success, cancellationToken))
                throw new ArgumentException("Превышено время ожидания работы задачи по пересчету кеша и смене базовой валюты.");
        }
    }
}
