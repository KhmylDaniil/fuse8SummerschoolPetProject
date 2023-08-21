using Fuse8_ByteMinds.SummerSchool.PublicApi.Models.Responses;

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
        Task<GetCurrencyResponse> GetCurrencyResponseAsync(CurrencyCode? currencyCode, CancellationToken cancellationToken);

        /// <summary>
        /// Метод получения избранного курса валюты
        /// </summary>
        /// <param name="name">Название</param>
        /// <param name="cancellationToken">Токен отмены</param>
        /// <returns>Значение последнего избранного курса валюты</returns>
        Task<GetFavoredCurrencyValueResponse> GetFavoredCurrencyAsync(string name, CancellationToken cancellationToken);

        /// <summary>
        /// Метод получения курса валюты на дату
        /// </summary>
        /// <param name="currencyCode">Код валюты</param>
        /// <param name="date">Дата актуальности курса</param>
        /// <param name="cancellationToken">токен отмены</param>
        /// <returns>Ответ на запрос курса валюты с датой актуальности</returns>
        Task<GetCurrencyHistoricalResponse> GetHistoricalAsync(CurrencyCode currencyCode, DateOnly date, CancellationToken cancellationToken);

        /// <summary>
        /// Метод получения избранного курса валюты на дату актуальности
        /// </summary>
        /// <param name="name">Название</param>
        /// <param name="date">Дата актуальности курса</param>
        /// <param name="cancellationToken">Токен отмены</param>
        /// <returns>Значение последнего избранного курса валюты</returns>
        Task<GetFavoredCurrencyValueResponse> GetFavoredCurrencyHistoricalAsync(string name, DateOnly date, CancellationToken cancellationToken);

        /// <summary>
        /// Запрос настроек приложения
        /// </summary>
        /// <param name="cancellationToken">токен отмены</param>
        /// <returns>Ответ на запрос настроек приложения</returns>
        Task<GetSettingsResponse> GetSettingsAsync(CancellationToken cancellationToken);
    }
}
