using Fuse8_ByteMinds.SummerSchool.PublicApi.Interfaces;
using Fuse8_ByteMinds.SummerSchool.PublicApi.Models.Responses;
using Google.Protobuf.WellKnownTypes;

namespace Fuse8_ByteMinds.SummerSchool.PublicApi.gRPC
{
    /// <summary>
    /// gRPC клиент для получени данных о курсах валют
    /// </summary>
    public class GrpcClient : IGrpcClient
    {
        private readonly CurrencyApi.CurrencyApiClient _grpcClient;
        private readonly ISettingsService _settingsService;
        private readonly IFavoriteCurrenciesService _favoriteCurrenciesService;

        /// <summary>
        /// Конструктор для <see cref="GrpcClient"/>
        /// </summary>
        /// <param name="grpcClient">Клиент gRPC</param>
        /// <param name="favoriteCurrenciesService">Сервис работы с избранными курсами валют</param>
        /// <param name="settingsService">Сервис работы с настройками</param>
        public GrpcClient(CurrencyApi.CurrencyApiClient grpcClient,
            IFavoriteCurrenciesService favoriteCurrenciesService,
            ISettingsService settingsService)
        {
            _grpcClient = grpcClient;
            _settingsService = settingsService;
            _favoriteCurrenciesService = favoriteCurrenciesService;
        }

        /// <summary>
        /// Метод получения последнего курса валюты
        /// </summary>
        /// <param name="currencyCode">код валюты или дефолтный из настроек</param>
        /// <param name="cancellationToken">токен отмены</param>
        /// <returns>Ответ на запрос курса валюты</returns>
        public async Task<GetCurrencyResponse> GetCurrencyResponseAsync(CurrencyCode? currencyCode, CancellationToken cancellationToken)
        {
            var settings = await _settingsService.GetSettingsAsNoTrackingAsync(cancellationToken);

            currencyCode ??= settings.DefaultCurrency;

            var currencyRequest = new CurrencyRequest { CurrencyCode = currencyCode.Value };
            var response = await _grpcClient.GetLatestAsyncAsync(currencyRequest, cancellationToken: cancellationToken);

            return new GetCurrencyResponse { Code = response.CurrencyCode, Value = (float)Math.Round(response.Value, settings.CurrencyRoundCount) };
        }

        /// <summary>
        /// Метод получения избранного курса валюты
        /// </summary>
        /// <param name="name">Название</param>
        /// <param name="cancellationToken">Токен отмены</param>
        /// <returns>Значение последнего избранного курса валюты</returns>
        public async Task<GetFavoredCurrencyValueResponse> GetFavoredCurrencyAsync(string name, CancellationToken cancellationToken)
        {
            var favCurrency = await _favoriteCurrenciesService.GetFavoriteCurrencyAsync(name, cancellationToken);

            var favCurrencyRequest = new FavoriteCurrencyRequest { Currency = favCurrency.Currency, BaseCurrency = favCurrency.BaseCurrency };

            var response = await _grpcClient.GetLatestFavoriteCurrencyAsyncAsync(favCurrencyRequest, cancellationToken: cancellationToken);

            var settings = await _settingsService.GetSettingsAsNoTrackingAsync(cancellationToken);

            return new GetFavoredCurrencyValueResponse(
                name: name,
                currency: response.Currency,
                baseCurrency: response.BaseCurrency,
                value: (float)Math.Round(response.Value, settings.CurrencyRoundCount));
        }

        /// <summary>
        /// Метод получения избранного курса валюты на дату актуальности
        /// </summary>
        /// <param name="name">Название</param>
        /// <param name="date">Дата актуальности курса</param>
        /// <param name="cancellationToken">Токен отмены</param>
        /// <returns>Значение последнего избранного курса валюты</returns>
        public async Task<GetFavoredCurrencyValueResponse> GetFavoredCurrencyHistoricalAsync(string name, DateOnly date, CancellationToken cancellationToken)
        {
            var favCurrency = await _favoriteCurrenciesService.GetFavoriteCurrencyAsync(name, cancellationToken);

            var favCurrencyRequest = new HistoricalFavoriteCurrencyRequest
            {
                Currency = favCurrency.Currency,
                BaseCurrency = favCurrency.BaseCurrency,
                Date = Timestamp.FromDateTime(date.ToDateTime(new TimeOnly(00, 00, 00), DateTimeKind.Utc))
            };

            var response = await _grpcClient.GetHistoricalFavoriteCurrencyAsyncAsync(favCurrencyRequest, cancellationToken: cancellationToken);

            var settings = await _settingsService.GetSettingsAsNoTrackingAsync(cancellationToken);

            return new GetFavoredCurrencyValueResponse(
                name: favCurrency.Name,
                currency: response.Currency,
                baseCurrency: response.BaseCurrency,
                value: (float)Math.Round(response.Value, settings.CurrencyRoundCount));
        }

        /// <summary>
        /// Метод получения курса валюты на дату
        /// </summary>
        /// <param name="currencyCode">Код валюты</param>
        /// <param name="date">Дата актуальности курса</param>
        /// <param name="cancellationToken">токен отмены</param>
        /// <returns>Ответ на запрос курса валюты с датой актуальности</returns>
        public async Task<GetCurrencyHistoricalResponse> GetHistoricalAsync(CurrencyCode currencyCode, DateOnly date, CancellationToken cancellationToken)
        {
            var currencyRequest = new CurrencyRequestWithDate
            {
                CurrencyCode = currencyCode,
                Date = Timestamp.FromDateTime(date.ToDateTime(new TimeOnly(00, 00, 00), DateTimeKind.Utc))
            };
            var response = await _grpcClient.GetHistoricalAsyncAsync(currencyRequest, cancellationToken: cancellationToken);

            var settings = await _settingsService.GetSettingsAsNoTrackingAsync(cancellationToken);

            return new GetCurrencyHistoricalResponse
            {
                Code = response.CurrencyCode,
                Value = (float)Math.Round(response.Value, settings.CurrencyRoundCount),
                Date = date.ToString("yyyy-MM-dd")
            };
        }

        /// <summary>
        /// Запрос настроек приложения
        /// </summary>
        /// <param name="cancellationToken">токен отмены</param>
        /// <returns>Ответ на запрос настроек приложения</returns>
        public async Task<GetSettingsResponse> GetSettingsAsync(CancellationToken cancellationToken)
        {
            var response = await _grpcClient.GetSettingsAsyncAsync(new SettingsRequest(), cancellationToken: cancellationToken);

            var settings = await _settingsService.GetSettingsAsNoTrackingAsync(cancellationToken);

            return new GetSettingsResponse
            {
                DefaultCurrency = settings.DefaultCurrency,
                BaseCurrency = response.BaseCurrency,
                NewRequestsAvailable = response.RemainingRequests,
                CurrencyRoundCount = settings.CurrencyRoundCount
            };            
        }
    }
}
