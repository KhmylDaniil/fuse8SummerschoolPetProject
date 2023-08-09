using Fuse8_ByteMinds.SummerSchool.InternalApi;
using Grpc.Core;
using InternalApi.Interfaces;

namespace InternalApi.gRPC
{
    /// <summary>
    /// gRPC сервис для передачи данных о курсах валют
    /// </summary>
    public class GrpcService : GrpcDocument.GrpcDocumentBase
    {
        private readonly ICachedCurrencyAPI _cachedCurrencyAPI;

        public GrpcService(ICachedCurrencyAPI cachedCurrencyAPI)
        {
            _cachedCurrencyAPI = cachedCurrencyAPI;
        }

        /// <summary>
        /// Метод передачи последнего курса валюты
        /// </summary>
        /// <param name="request">Запрос кода валюты</param>
        /// <param name="context">контекст</param>
        /// <returns>Ответ на запрос валюты по коду</returns>
        public override async Task<CurrencyResponse> GetLatestAsync(CurrencyRequest request, ServerCallContext context)
        {
            var response = await _cachedCurrencyAPI.GetCurrentCurrencyAsync(request.CurrencyCode, default);

            return new CurrencyResponse
            {
                CurrencyCode = response.CurrencyCode,
                Value = response.Value,
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
            var response = await _cachedCurrencyAPI.GetCurrencyOnDateAsync(request.CurrencyCode, DateOnly.FromDateTime(request.Date.ToDateTime()), default);

            return new CurrencyResponse
            {
                CurrencyCode = response.CurrencyCode,
                Value = response.Value,
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
            var response = await _cachedCurrencyAPI.GetSettingsAsync(default);

            return new SettingsResponse
            {
                BaseCurrency = response.BaseCurrency,
                RemainingRequests = response.RequestLimit > response.RequestCount
            };
        }
    }
}
