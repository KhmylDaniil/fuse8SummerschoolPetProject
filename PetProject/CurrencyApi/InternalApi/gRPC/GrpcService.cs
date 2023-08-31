using Fuse8_ByteMinds.SummerSchool.InternalApi;
using Grpc.Core;
using InternalApi.Interfaces;
using InternalApi.Services;

namespace InternalApi.gRPC
{
    /// <summary>
    /// gRPC сервис для передачи данных о курсах валют
    /// </summary>
    public class GrpcService : GrpcDocument.GrpcDocumentBase
    {
        private readonly ICachedCurrencyService _cachedCurrencyApi;
        private readonly ICurrencyApi _currencyApi;

        /// <summary>
        /// Конструктор для <see cref="GrpcService"/>
        /// </summary>
        /// <param name="cachedCurrencyApi">Сервис для получения курсов валют</param>
        /// <param name="currencyHttpClient">Сервис для работы с внешним апи</param>
        public GrpcService(ICachedCurrencyService cachedCurrencyApi, CurrencyHttpClient currencyHttpClient)
        {
            _cachedCurrencyApi = cachedCurrencyApi;
            _currencyApi = currencyHttpClient;
        }

        /// <summary>
        /// Метод передачи последнего курса валюты
        /// </summary>
        /// <param name="request">Запрос кода валюты</param>
        /// <param name="context">контекст</param>
        /// <returns>Ответ на запрос валюты по коду</returns>
        public override async Task<CurrencyResponse> GetLatestAsync(CurrencyRequest request, ServerCallContext context)
        {
            var response = await _cachedCurrencyApi.GetCurrentCurrencyAsync(
                currencyCode: request.CurrencyCode,
                cancellationToken: default,
                dontRound: true);

            return new CurrencyResponse
            {
                CurrencyCode = response.CurrencyCode,
                Value = response.Value,
            };
        }

        /// <summary>
        /// Метод передачи последнего избранного курса валюты
        /// </summary>
        /// <param name="request">Запрос избранного курса валюты</param>
        /// <param name="context">контекст</param>
        /// <returns>Значение избранного курса валюты по названию</returns>
        public override async Task<FavoriteCurrencyResponse> GetLatestFavoriteCurrencyAsync(FavoriteCurrencyRequest request, ServerCallContext context)
        {
            var response = await _cachedCurrencyApi.GetFavoredCurrencyAsync(request.Currency, request.BaseCurrency, default);

            return new FavoriteCurrencyResponse
            {
               Currency = request.Currency,
               BaseCurrency = request.BaseCurrency,
               Value = response
            };
        }

        /// <summary>
        /// Метод передачи курса валюты на дату
        /// </summary>
        /// <param name="request">Запрос кода валюты с датой</param>
        /// <param name="context">контекст</param>
        /// <returns>Ответ на запрос валюты по коду</returns>
        public override async Task<CurrencyResponse> GetHistoricalAsync(CurrencyRequestWithDate request, ServerCallContext context)
        {
            var response = await _cachedCurrencyApi.GetCurrencyOnDateAsync(
                currencyCode: request.CurrencyCode,
                date: DateOnly.FromDateTime(request.Date.ToDateTime()),
                cancellationToken: default,
                dontRound: true);

            return new CurrencyResponse
            {
                CurrencyCode = response.CurrencyCode,
                Value = response.Value,
            };
        }

        /// <summary>
        /// Метод передачи последнего избранного курса валюты на дату актуальности
        /// </summary>
        /// <param name="request">Запрос избранного курса валюты на дату актуальности</param>
        /// <param name="context">контекст</param>
        /// <returns>Значение избранного курса валюты по названию</returns>
        public override async Task<FavoriteCurrencyResponse> GetHistoricalFavoriteCurrencyAsync(HistoricalFavoriteCurrencyRequest request, ServerCallContext context)
        {
            var response = await _cachedCurrencyApi.GetFavoredCurrencyHistoricalAsync(
                currency: request.Currency,
                baseCurrency: request.BaseCurrency,
                date: DateOnly.FromDateTime(request.Date.ToDateTime()),
                cancellationToken: default);

            return new FavoriteCurrencyResponse
            {
                Currency = request.Currency,
                BaseCurrency = request.BaseCurrency,
                Value = response
            };
        }

        /// <summary>
        /// Метод передачи настроек приложения
        /// </summary>
        /// <param name="request">Запрос настроек</param>
        /// <param name="context">контекст</param>
        /// <returns>Настройки приложения</returns>
        public override async Task<SettingsResponse> GetSettingsAsync(SettingsRequest request, ServerCallContext context)
        {
            var response = await _currencyApi.GetSettingsAsync(default);

            return new SettingsResponse
            {
                BaseCurrency = response.BaseCurrency,
                RemainingRequests = response.RequestLimit > response.RequestCount
            };
        }
    }
}
