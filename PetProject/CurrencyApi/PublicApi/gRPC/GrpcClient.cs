using Fuse8_ByteMinds.SummerSchool.PublicApi.Exceptions;
using Fuse8_ByteMinds.SummerSchool.PublicApi.Interfaces;
using Fuse8_ByteMinds.SummerSchool.PublicApi.Models;
using Google.Protobuf.WellKnownTypes;
using Enum = System.Enum;

namespace Fuse8_ByteMinds.SummerSchool.PublicApi.gRPC
{
    /// <summary>
    /// gRPC клиент для получени данных о курсах валют
    /// </summary>
    public class GrpcClient : IGrpcClient
    {
        private readonly GrpcDocument.GrpcDocumentClient _grpcClient;
        private readonly CurrencySettings _settings;

        public GrpcClient(GrpcDocument.GrpcDocumentClient grpcClient, IConfiguration configuration)
        {
            _grpcClient = grpcClient;
            _settings = configuration.GetRequiredSection("CurrencySettings").Get<CurrencySettings>();
        }

        /// <summary>
        /// Метод получения последнего курса валюты
        /// </summary>
        /// <param name="currencyCode">код валюты или дефолтный из конфигурации</param>
        /// <param name="cancellationToken">токен отмены</param>
        /// <returns>Ответ на запрос курса валюты</returns>
        public async Task<GetCurrencyResponse> GetCurrencyResponseAsync(string? currencyCode, CancellationToken cancellationToken)
        {
            currencyCode = currencyCode is null
                ? _settings.DefaultCurrency
                : currencyCode;

            if (!Enum.TryParse(currencyCode, true, out CurrencyCode output))
                throw new CurrencyNotFoundException();

            var currencyRequest = new CurrencyRequest { CurrencyCode = output };
            var response = await _grpcClient.GetLatestAsyncAsync(currencyRequest, cancellationToken: cancellationToken);

            return new GetCurrencyResponse { code = Enum.GetName(response.CurrencyCode), value = response.Value};
        }

        /// <summary>
        /// Метод получения курса валюты на дату
        /// </summary>
        /// <param name="currencyCode">Код валюты</param>
        /// <param name="date">Дата актуальности курса</param>
        /// <param name="cancellationToken">токен отмены</param>
        /// <returns>Ответ на запрос курса валюты с датой актуальности</returns>
        public async Task<GetCurrencyHistoricalResponse> GetHistoricalAsync(string currencyCode, DateOnly date, CancellationToken cancellationToken)
        {
            if (!Enum.TryParse(currencyCode, true, out CurrencyCode output))
                throw new CurrencyNotFoundException();

            var currencyRequest = new CurrencyRequestWithDate
            {
                CurrencyCode = output,
                Date = Timestamp.FromDateTime(date.ToDateTime(new TimeOnly(23, 59, 59), DateTimeKind.Utc))
            };
            var response = await _grpcClient.GetHistoricalAsyncAsync(currencyRequest, cancellationToken: cancellationToken);

            return new GetCurrencyHistoricalResponse
            {
                code = Enum.GetName(response.CurrencyCode),
                value = response.Value,
                date = date.ToString("yyyy-MM-dd")
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

            return new GetSettingsResponse
            {
                DefaultCurrency = _settings.DefaultCurrency,
                BaseCurrency = response.BaseCurrency,
                NewRequestsAvailable = response.RemainingRequests,
                CurrencyRoundCount = _settings.CurrencyRoundCount
            };            
        }
    }
}
