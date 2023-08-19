﻿using Fuse8_ByteMinds.SummerSchool.InternalApi;
using Fuse8_ByteMinds.SummerSchool.InternalApi.Models;
using InternalApi.Interfaces;
using InternalApi.Models;
using Microsoft.EntityFrameworkCore;
using Microsoft.Extensions.Caching.Memory;
using Microsoft.Extensions.Options;
using Fuse8_ByteMinds.SummerSchool.InternalApi.Exceptions;

namespace InternalApi.Services
{
    /// <summary>
    /// Сервис для получения данных о курсе валюты с возможностью получения данных из кеша, базы данных и внешнего апи
    /// </summary>
    public class CachedCurrencyService : ICachedCurrencyApi
    {
        private readonly IAppDbContext _appDbContext;
        private readonly ICurrencyApi _currencyAPI;
        private readonly IMemoryCache _memoryCache;
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
            IOptionsSnapshot<CurrencySettings> settings)
        {
            _appDbContext = appDbContext;
            _currencyAPI = currencyAPI;
            _settings = settings.Value;
            _memoryCache = memoryCache;
        }

        /// <summary>
        /// Метод получения курса валюты на дату
        /// </summary>
        /// <param name="currencyCode">Код валюты</param>
        /// <param name="date">Дата актуальности курса</param>
        /// <param name="cancellationToken">Токен отмены</param>
        /// <returns>Модель курса валюты</returns>
        public async Task<CurrencyDTO> GetCurrencyOnDateAsync(CurrencyCode currencyCode, DateOnly date, CancellationToken cancellationToken)
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
                data = await _currencyAPI.GetAllCurrenciesOnDateAsync(_settings.BaseCurrency, date, cancellationToken);

                _appDbContext.CurrenciesOnDates.Add(data);
                await _appDbContext.SaveChangesAsync(cancellationToken);

            }

            return FindCurrencyByCode(currencyCode, data);
        }

        /// <summary>
        /// Метод получения последнего курса валюты
        /// </summary>
        /// <param name="currencyCode">Код валюты</param>
        /// <param name="cancellationToken">Токен отмены</param>
        /// <returns>Модель курса валюты</returns>
        public async Task<CurrencyDTO> GetCurrentCurrencyAsync(CurrencyCode currencyCode, CancellationToken cancellationToken)
        {
            CurrenciesOnDate? currenciesOnDate = null;

            if (_memoryCache.TryGetValue("cur", out CurrenciesOnDate? cachedOutput) && DateTime.UtcNow - cachedOutput?.Date <= new TimeSpan(2, 0, 0))
                currenciesOnDate = cachedOutput;

            currenciesOnDate ??= await _appDbContext.CurrenciesOnDates.OrderBy(c => c.Date).LastOrDefaultAsync(cancellationToken);

            if (currenciesOnDate == null)
            {
                var currencies = await _currencyAPI.GetAllCurrentCurrenciesAsync(_settings.BaseCurrency, cancellationToken);

                var result = new CurrenciesOnDate { Date = DateTime.UtcNow, Currencies = currencies };

                _appDbContext.CurrenciesOnDates.Add(result);
                await _appDbContext.SaveChangesAsync(cancellationToken);
            }

            _memoryCache.Set("cur", currenciesOnDate);

            return FindCurrencyByCode(currencyCode, currenciesOnDate);
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
        /// <returns>Курс валюты</returns>
        /// <exception cref="CurrencyNotFoundException"></exception>
        private CurrencyDTO FindCurrencyByCode(CurrencyCode currencyCode, CurrenciesOnDate data)
        {
            var currency = data.Currencies.FirstOrDefault(c => c.Code.Equals(Enum.GetName(currencyCode), StringComparison.OrdinalIgnoreCase))
                ?? throw new CurrencyNotFoundException();

            return new CurrencyDTO(currencyCode, (float)Math.Round(currency.Value, _settings.CurrencyRoundCount));
        }
    }
}
