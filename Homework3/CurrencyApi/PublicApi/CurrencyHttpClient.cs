﻿using Fuse8_ByteMinds.SummerSchool.PublicApi.Exceptions;
using Fuse8_ByteMinds.SummerSchool.PublicApi.Models;
using Fuse8_ByteMinds.SummerSchool.PublicApi.Models.ExternalApiResponseModels;
using Microsoft.Extensions.Options;
using System.Net;
using System.Text.Json;

namespace Fuse8_ByteMinds.SummerSchool.PublicApi
{
    /// <summary>
    /// Клиент для обращения к внешнему API курсов валюты https://api.currencyapi.com
    /// </summary>
    public class CurrencyHttpClient
    {
        private readonly HttpClient _httpClient;

        private readonly CurrencySettings _settings;

        public CurrencyHttpClient(HttpClient httpClient, IOptionsSnapshot<CurrencySettings> settings)
        {
            _httpClient = httpClient;

            _settings = settings.Value;
        }

        /// <summary>
        /// Обращение к внешнему API
        /// </summary>
        /// <param name="uri">URI</param>
        /// <param name="cancellationToken">Токен отмены</param>
        /// <returns></returns>
        /// <exception cref="CurrencyNotFoundException">Исключение неподдерживаемого кода валюты</exception>
        public async Task<string> GetStringAsyncWithCheck(string uri, CancellationToken cancellationToken)
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
        /// Метод обращения к текущему курсу валюты
        /// </summary>
        /// <param name="currencyCode">Код валюты</param>
        /// <param name="cancellationToken">Токен отмены</param>
        /// <returns></returns>
        public async Task<GetCurrencyResponse> GetLatestAsync(string? currencyCode, CancellationToken cancellationToken)
        {
            await CheckRequestLimit(cancellationToken);

            currencyCode = currencyCode is null? _settings.DefaultCurrency : currencyCode.ToUpper();

            string url = $"latest?currencies={currencyCode.ToUpper()}" +
                $"&base_currency={_settings.BaseCurrency}&apikey={_settings.ApiKey}";

            var responseJson = await GetStringAsyncWithCheck(url, cancellationToken);

            var response = JsonSerializer.Deserialize<ExternalApiResponseLatest>(responseJson);

            return new GetCurrencyResponse()
            {
                Code = currencyCode,
                Value = (float)Math.Round(response.Data[currencyCode].Value, _settings.CurrencyRoundCount)
            };
        }

        /// <summary>
        /// Метод обращения к курсу валюты на дату
        /// </summary>
        /// <param name="currencyCode">Код валюты</param>
        /// <param name="date">Дата</param>
        /// <param name="cancellationToken">Токен отмены</param>
        /// <returns></returns>
        public async Task<GetCurrencyHistoricalResponse> GetHistoricalAsync(string currencyCode, DateTime date, CancellationToken cancellationToken)
        {
            await CheckRequestLimit(cancellationToken);

            currencyCode = currencyCode.ToUpper();

            string url = $"historical?currencies={currencyCode.ToUpper()}&date={date}" +
                $"&base_currency={_settings.BaseCurrency}&apikey={_settings.ApiKey}";

            var responseJson = await GetStringAsyncWithCheck(url, cancellationToken);

            var response = JsonSerializer.Deserialize<ExternalApiResponseLatest>(responseJson);

            return new GetCurrencyHistoricalResponse()
            {
                Code = currencyCode,
                Value = (float)Math.Round(response.Data[currencyCode].Value, _settings.CurrencyRoundCount),
                Date = DateTime.Parse(response.Meta["last_updated_at"]).Date.ToString("yyyy-mm-dd")
            };
        }

        /// <summary>
        /// Метод вызова настроек приложения
        /// </summary>
        /// <param name="cancellationToken">Токен отмены</param>
        /// <returns></returns>
        public async Task<GetSettingsResponse> GetSettingsAsync(CancellationToken cancellationToken)
        {
            string url = $"status?apikey={_settings.ApiKey}";

            var responseJson = await GetStringAsyncWithCheck(url, cancellationToken);

            var response = JsonSerializer.Deserialize<ExternalApiResponseStatus>(responseJson);

            return new GetSettingsResponse()
            {
                DefaultCurrency = _settings.DefaultCurrency,
                BaseCurrency = _settings.BaseCurrency,
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
        private async Task CheckRequestLimit(CancellationToken cancellationToken)
        {
            var requestLimitCheck = await GetSettingsAsync(cancellationToken);

            if (requestLimitCheck.RequestCount >= _settings.MaxRequestsPerMonth)
                throw new ApiRequestLimitException();
        }
    }
}
