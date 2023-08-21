﻿using Fuse8_ByteMinds.SummerSchool.InternalApi;
using Fuse8_ByteMinds.SummerSchool.InternalApi.Models;
using InternalApi.Models;

namespace InternalApi.Interfaces
{
    /// <summary>
    /// Интерфейс получения курса валюты из кеша, базы данных или внешнего апи
    /// </summary>
    public interface ICachedCurrencyApi
    {
        /// <summary>
        /// Получает текущий курс
        /// </summary>
        /// <param name="currencyCode">Валюта, для которой необходимо получить курс</param>
        /// <param name="cancellationToken">Токен отмены</param>
        /// <param name="dontRound">Флаг отмены округления</param>
        /// <returns>Текущий курс</returns>
        Task<CurrencyDTO> GetCurrentCurrencyAsync(CurrencyCode currencyCode, CancellationToken cancellationToken, bool dontRound = default);

        /// <summary>
        /// Получает курс валюты, актуальный на <paramref name="date"/>
        /// </summary>
        /// <param name="currencyCode">Валюта, для которой необходимо получить курс</param>
        /// <param name="date">Дата, на которую нужно получить курс валют</param>
        /// <param name="cancellationToken">Токен отмены</param>
        /// <param name="dontRound">Флаг отмены округления</param>
        /// <returns>Курс на дату</returns>
        Task<CurrencyDTO> GetCurrencyOnDateAsync(CurrencyCode currencyCode, DateOnly date, CancellationToken cancellationToken, bool dontRound = default);

        /// <summary>
        /// Метод получения избранного курса валюты
        /// </summary>
        /// <param name="currency">Код валюты</param>
        /// <param name="baseCurrency">Код базовой валюты</param>
        /// <param name="cancellationToken">Токен отмены</param>
        /// <returns>Значение курса валюты к базовой валюте</returns>
        public Task<float> GetFavoredCurrencyAsync(CurrencyCode currency, CurrencyCode baseCurrency, CancellationToken cancellationToken);

        /// <summary>
        /// Метод получения избранного курса валюты на дату актуальности
        /// </summary>
        /// <param name="currency">Код валюты</param>
        /// <param name="baseCurrency">Код базовой валюты</param>
        /// <param name="date">Дата актуальности курса</param>
        /// <param name="cancellationToken">Токен отмены</param>
        /// <returns>Значение курса валюты к базовой валюте на дату актуальности</returns>
        Task<float> GetFavoredCurrencyHistoricalAsync(CurrencyCode currency, CurrencyCode baseCurrency, DateOnly date, CancellationToken cancellationToken);

        /// <summary>
        /// Показывает настройки приложения
        /// </summary>
        /// <param name="cancellationToken">Токен отмены</param>
        /// <returns>Настройки приложения</returns>
        Task<GetSettingsResponse> GetSettingsAsync(CancellationToken cancellationToken);

        /// <summary>
        /// Проверка связи с внешним апи
        /// </summary>
        /// <param name="cancellationToken">Токен отмены</param>
        /// <returns>Ответ на хелчек</returns>
        Task<HealthCheckResponse> HealthCheck(CancellationToken cancellationToken);
    }
}
