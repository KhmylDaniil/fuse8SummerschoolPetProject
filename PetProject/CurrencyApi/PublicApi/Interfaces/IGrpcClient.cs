using Fuse8_ByteMinds.SummerSchool.PublicApi.Models;

namespace Fuse8_ByteMinds.SummerSchool.PublicApi.Interfaces
{
    /// <summary>
    /// gRPC клиент для получени данных о курсах валют
    /// </summary>
    public interface IGrpcClient
    {
        /// <summary>
        /// Метод получения последнего курса валюты
        /// </summary>
        /// <param name="currencyCode">код валюты или дефолтный из настроек</param>
        /// <param name="cancellationToken">токен отмены</param>
        /// <returns>Ответ на запрос курса валюты</returns>
        public Task<GetCurrencyResponse> GetCurrencyResponseAsync(CurrencyCode? currencyCode, CancellationToken cancellationToken);

        /// <summary>
        /// Метод получения курса валюты на дату
        /// </summary>
        /// <param name="currencyCode">Код валюты</param>
        /// <param name="date">Дата актуальности курса</param>
        /// <param name="cancellationToken">токен отмены</param>
        /// <returns>Ответ на запрос курса валюты с датой актуальности</returns>
        public Task<GetCurrencyHistoricalResponse> GetHistoricalAsync(CurrencyCode currencyCode, DateOnly date, CancellationToken cancellationToken);

        /// <summary>
        /// Запрос настроек приложения
        /// </summary>
        /// <param name="cancellationToken">токен отмены</param>
        /// <returns>Ответ на запрос настроек приложения</returns>
        public Task<GetSettingsResponse> GetSettingsAsync(CancellationToken cancellationToken);
    }
}
