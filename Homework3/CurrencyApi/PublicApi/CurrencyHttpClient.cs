using Fuse8_ByteMinds.SummerSchool.PublicApi.Exceptions;
using System.Net;

namespace Fuse8_ByteMinds.SummerSchool.PublicApi
{
    /// <summary>
    /// Клиент для обращения к внешнему API курсов валюты https://api.currencyapi.com
    /// </summary>
    public class CurrencyHttpClient
    {
        private readonly HttpClient _httpClient;

        public CurrencyHttpClient(HttpClient httpClient) => _httpClient = httpClient;

        /// <summary>
        /// Обращение к внешнему API
        /// </summary>
        /// <param name="uri">URI</param>
        /// <param name="cancellationToken">Токен отмены</param>
        /// <returns></returns>
        /// <exception cref="CurrencyNotFoundException">Исключение неподдерживаемого кода валюты</exception>
        public async Task<string> GetStringAsync(string uri, CancellationToken cancellationToken)
        {
            try
            {
                return await _httpClient.GetStringAsync(uri, cancellationToken);
            }
            catch (HttpRequestException ex) when (ex.StatusCode == HttpStatusCode.UnprocessableEntity)
            {
                throw new CurrencyNotFoundException();
            }
        }
    }
}
