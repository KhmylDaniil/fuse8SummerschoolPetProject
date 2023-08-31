using Fuse8_ByteMinds.SummerSchool.InternalApi;
using Fuse8_ByteMinds.SummerSchool.InternalApi.Exceptions;
using Fuse8_ByteMinds.SummerSchool.InternalApi.Models;
using Fuse8_ByteMinds.SummerSchool.InternalApi.Models.ExternalApiResponseModels;
using InternalApi.Interfaces;
using InternalApi.Models;
using InternalApi.Models.Entities;
using Microsoft.Extensions.Options;
using System.Net;
using System.Text.Json;

namespace InternalApi.Services
{
    /// <summary>
    /// Клиент для обращения к внешнему API курсов валюты https://api.currencyapi.com
    /// </summary>
    public class CurrencyHttpClient : ICurrencyApi
    {
        private readonly HttpClient _httpClient;
        private readonly CurrencySettings _settings;
        private readonly ISettingsService _settingsService;

        /// <summary>
        /// Конструктор для <see cref="CurrencyHttpClient"/>
        /// </summary>
        /// <param name="httpClient"><inheritdoc/></param>
        /// <param name="settingsService"></param>
        /// <param name="settings"></param>
        public CurrencyHttpClient(HttpClient httpClient, ISettingsService settingsService, IOptionsSnapshot<CurrencySettings> settings)
        {
            _httpClient = httpClient;
            _settingsService = settingsService;
            _settings = settings.Value;
        }

        /// <summary>
        /// Обращение к внешнему API
        /// </summary>
        /// <param name="uri">URI</param>
        /// <param name="cancellationToken">Токен отмены</param>
        /// <returns></returns>
        /// <exception cref="CurrencyNotFoundException">Исключение неподдерживаемого кода валюты</exception>
        public async Task<string> GetStringWithCheckAsync(string uri, CancellationToken cancellationToken)
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

        /// <summary>
        /// Метод обращения ко всем текущим курсам валют
        /// </summary>
        /// <param name="cancellationToken">Токен отмены</param>
        /// <returns>Текущие курсы всех валют</returns>
        public async Task<Currency[]> GetAllCurrentCurrenciesAsync(CancellationToken cancellationToken)
        {
            await CheckRequestLimitAsync(cancellationToken);

            var settingsFromDb = await _settingsService.GetSettingsAsync(cancellationToken);

            string url = $"latest?base_currency={Enum.GetName(settingsFromDb.BaseCurrency).ToUpper()}&apikey={_settings.ApiKey}";

            var responseJson = await GetStringWithCheckAsync(url, cancellationToken);

            var response = JsonSerializer.Deserialize<ExternalApiResponseCurrencies>(responseJson);

            return response.Data.Values.ToArray();
        }

        /// <summary>
        /// Метод получения всех курсов валют на указанную дату
        /// </summary>
        /// <param name="date">Дата актуальности курсов валют</param>
        /// <param name="cancellationToken">Токен отмены</param>
        /// <returns>Курсы валют на актуальную дату</returns>
        public async Task<CurrenciesOnDate> GetAllCurrenciesOnDateAsync(DateOnly date, CancellationToken cancellationToken)
        {
            await CheckRequestLimitAsync(cancellationToken);

            var settingsFromDb = await _settingsService.GetSettingsAsync(cancellationToken);

            string url = $"historical?&date={date}" +
                $"&base_currency={Enum.GetName(settingsFromDb.BaseCurrency).ToUpper()}&apikey={_settings.ApiKey}";

            var responseJson = await GetStringWithCheckAsync(url, cancellationToken);

            var response = JsonSerializer.Deserialize<ExternalApiResponseCurrencies>(responseJson);

            CurrenciesOnDate result = new()
            {
                Date = DateTime.Parse(response.Meta["last_updated_at"], styles: System.Globalization.DateTimeStyles.AdjustToUniversal),
                Currencies = response.Data.Values.ToArray()
            };

            return result;
        }

        /// <summary>
        /// Проверка связи с внешним апи
        /// </summary>
        /// <param name="cancellationToken">Токен отмены</param>
        /// <returns>Ответ на хелчек</returns>
        public async Task<HealthCheckResponse> HealthCheckAsync(CancellationToken cancellationToken)
        {
            string url = $"status?apikey={_settings.ApiKey}";

            var response = await _httpClient.GetAsync(url, cancellationToken);

            var result = new HealthCheckResponse
            {
                CheckedOn = DateTimeOffset.Now,
                Status = response.IsSuccessStatusCode ? HealthCheckResponse.CheckStatus.Ok : HealthCheckResponse.CheckStatus.Failed
            };

            return result;
        }

        /// <summary>
        /// Метод вызова настроек приложения
        /// </summary>
        /// <param name="cancellationToken">Токен отмены</param>
        /// <returns></returns>
        public async Task<GetSettingsResponse> GetSettingsAsync(CancellationToken cancellationToken)
        {
            string url = $"status?apikey={_settings.ApiKey}";

            var responseJson = await GetStringWithCheckAsync(url, cancellationToken);

            var response = JsonSerializer.Deserialize<ExternalApiResponseStatus>(responseJson);

            var settingsFromDb = await _settingsService.GetSettingsAsync(cancellationToken);

            return new GetSettingsResponse()
            {
                DefaultCurrency = _settings.DefaultCurrency,
                BaseCurrency = Enum.GetName(settingsFromDb.BaseCurrency),
                RequestLimit = response.Quotas["month"].Total,
                RequestCount = response.Quotas["month"].Used,
                CurrencyRoundCount = _settings.CurrencyRoundCount,
            };
        }

        /// <summary>
        /// Метод проверки достижения лимита запросов в месяц
        /// </summary>
        /// <param name="cancellationToken">токен отмены</param>
        /// <returns></returns>
        /// <exception cref="ApiRequestLimitException">Ошибка исчерпания количества обращений к внешнему API</exception>
        private async Task CheckRequestLimitAsync(CancellationToken cancellationToken)
        {
            var requestLimitCheck = await GetSettingsAsync(cancellationToken);

            if (requestLimitCheck.RequestCount >= _settings.MaxRequestsPerMonth)
                throw new ApiRequestLimitException();
        }
    }
}
