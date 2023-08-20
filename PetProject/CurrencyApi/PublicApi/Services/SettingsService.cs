﻿using Fuse8_ByteMinds.SummerSchool.PublicApi.Interfaces;
using Fuse8_ByteMinds.SummerSchool.PublicApi.Models;
using Microsoft.EntityFrameworkCore;

namespace Fuse8_ByteMinds.SummerSchool.PublicApi.Services
{
    /// <summary>
    /// Сервис изменения настроек приложения
    /// </summary>
    public class SettingsService : ISettingsService
    {
        private readonly IAppDbContext _appDbContext;

        public SettingsService(IAppDbContext appDbContext)
        {
            _appDbContext = appDbContext;
        }

        /// <summary>
        /// Изменить точность округления
        /// </summary>
        /// <param name="currencyRoundCount">Количество знаков после запятой</param>
        /// <param name="cancellationToken">Токен отмены</param>
        public async Task ChangeCurrencyRoundCountAsync(int currencyRoundCount, CancellationToken cancellationToken)
        {
            if (currencyRoundCount < 0)
                throw new ArgumentException(Exceptions.ExceptionMessages.CurrencyRoundCantBeNegative);

            var settings = await GetSettingsAsync(cancellationToken);

            settings.CurrencyRoundCount = currencyRoundCount;

            await _appDbContext.SaveChangesAsync(cancellationToken);
        }

        /// <summary>
        /// Изменить валюту по умолчанию
        /// </summary>
        /// <param name="currencyCode">Код валюты</param>
        /// <param name="cancellationToken">Токен отмены</param>
        public async Task ChangeDefaultCurrencyAsync(CurrencyCode currencyCode, CancellationToken cancellationToken)
        {
            var settings = await GetSettingsAsync(cancellationToken);

            settings.DefaultCurrency = currencyCode;

            await _appDbContext.SaveChangesAsync(cancellationToken);
        }

        /// <summary>
        /// Получение настроек из базы данных
        /// </summary>
        /// <param name="cancellationToken">токен отмены</param>
        /// <returns>Настройки приложения</returns>
        public async Task<Settings> GetSettingsAsync(CancellationToken cancellationToken)
        {
            var settings = await _appDbContext.Settings.ToListAsync(cancellationToken);

            if (settings.Count != 1)
                throw new ArgumentException(Exceptions.ExceptionMessages.NotSingleSettings);

            return settings[0];
        }
    }
}
