using Fuse8_ByteMinds.SummerSchool.PublicApi.Exceptions;
using Fuse8_ByteMinds.SummerSchool.PublicApi.Interfaces;
using Fuse8_ByteMinds.SummerSchool.PublicApi.Models;
using Google.Protobuf.WellKnownTypes;
using Microsoft.Extensions.Options;

namespace Fuse8_ByteMinds.SummerSchool.PublicApi.gRPC
{
    /// <summary>
    /// gRPC клиент для получени данных о курсах валют
    /// </summary>
    public class GrpcClient : IGrpcClient
    {
        private readonly GrpcDocument.GrpcDocumentClient _grpcClient;
        private readonly CurrencySettings _settings;
        private readonly ISettingsService _settingsService;

        public GrpcClient(GrpcDocument.GrpcDocumentClient grpcClient,
            IOptionsSnapshot<CurrencySettings> settings,
            ISettingsService settingsService)
        {
            _grpcClient = grpcClient;
            _settings = settings.Value;
            _settingsService = settingsService;
        }

        /// <summary>
        /// Метод получения последнего курса валюты
        /// </summary>
        /// <param name="currencyCode">код валюты или дефолтный из настроек</param>
        /// <param name="cancellationToken">токен отмены</param>
        /// <returns>Ответ на запрос курса валюты</returns>
        public async Task<GetCurrencyResponse> GetCurrencyResponseAsync(CurrencyCode? currencyCode, CancellationToken cancellationToken)
        {
            if (currencyCode == null)
            {
                var settings = await _settingsService.GetSettingsAsync(cancellationToken);
                currencyCode = settings.DefaultCurrency;
            }

            var currencyRequest = new CurrencyRequest { CurrencyCode = currencyCode.Value };
            var response = await _grpcClient.GetLatestAsyncAsync(currencyRequest, cancellationToken: cancellationToken);

            return new GetCurrencyResponse { Code = response.CurrencyCode, Value = response.Value};
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

            return new GetCurrencyHistoricalResponse
            {
                Code = response.CurrencyCode,
                Value = response.Value,
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

            var settings = await _settingsService.GetSettingsAsync(cancellationToken);

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
