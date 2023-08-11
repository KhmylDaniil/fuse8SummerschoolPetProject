using Fuse8_ByteMinds.SummerSchool.InternalApi;
using Fuse8_ByteMinds.SummerSchool.InternalApi.Models;
using InternalApi.Interfaces;
using InternalApi.Models;
using System.Text.Json;

namespace InternalApi.Services
{
    /// <summary>
    /// Сервис получения данных о курсе валюты из внешнего апи с поддержкой кэширования
    /// </summary>
    public class CashedCurrencyService : ICachedCurrencyAPI
    {
        private readonly ICurrencyAPI _currencyAPI;
        private readonly CurrencySettings _settings;
        private static readonly string _basePath = $"D:\\source\\fuse8homework\\PetProject\\CurrencyApi\\Cache\\";

        public CashedCurrencyService(CurrencyHttpClient currencyAPI, IConfiguration configuration)
        {
            _currencyAPI = currencyAPI;
            _settings = configuration.GetRequiredSection("CurrencySettings").Get<CurrencySettings>();
        }

        /// <summary>
        /// Метод получения курса валюты на дату
        /// </summary>
        /// <param name="currencyCode">Код валюты</param>
        /// <param name="date">Дата актуальности курса</param>
        /// <param name="cancellationToken">Токен отмены</param>
        /// <returns>Модель курса валюты</returns>
        public async Task<CurrencyDTO> GetCurrencyOnDateAsync(CurrencyCode currencyCode, DateOnly date, CancellationToken cancellationToken)
        {
            var resultFromCache = await CheckCache(currencyCode, date);

            if (resultFromCache is not null)
                return resultFromCache;

            var dataToSave = await _currencyAPI.GetAllCurrenciesOnDateAsync(_settings.BaseCurrency, date, cancellationToken);

            await SaveCacheAsync(dataToSave);

            var currencyFromExternalApi = dataToSave.Currencies.FirstOrDefault(c => c.Code.Equals(Enum.GetName(currencyCode), StringComparison.OrdinalIgnoreCase));

            return new CurrencyDTO(currencyCode, GetRounded(currencyFromExternalApi.Value));
        }

        /// <summary>
        /// Метод получения последнего курса валюты
        /// </summary>
        /// <param name="currencyCode">Код валюты</param>
        /// <param name="cancellationToken">Токен отмены</param>
        /// <returns>Модель курса валюты</returns>
        public async Task<CurrencyDTO> GetCurrentCurrencyAsync(CurrencyCode currencyCode, CancellationToken cancellationToken)
        {
            var resultFromCache = await CheckCache(currencyCode);

            if (resultFromCache is not null)
                return resultFromCache;

            var result = await _currencyAPI.GetAllCurrentCurrenciesAsync(_settings.BaseCurrency, cancellationToken);

            CurrenciesOnDate dataToSave = new()
            {
                Date = DateTime.Now,
                Currencies = result
            };

            await SaveCacheAsync(dataToSave);

            var currencyFromExternalApi = result.FirstOrDefault(c => c.Code.Equals(Enum.GetName(currencyCode), StringComparison.OrdinalIgnoreCase));

            return new CurrencyDTO(currencyCode, GetRounded(currencyFromExternalApi.Value));
        }

        /// <summary>
        /// Метод получения настроек приложения
        /// </summary>
        /// <param name="cancellationToken">Токен отмены</param>
        /// <returns>Настройки приложения</returns>
        public async Task<GetSettingsResponse> GetSettingsAsync(CancellationToken cancellationToken)
            => await _currencyAPI.GetSettingsAsync(cancellationToken);

        /// <summary>
        /// Сохранение кэша в файл
        /// </summary>
        /// <param name="currenciesOnDate">Данные на дату</param>
        /// <returns></returns>
        private static async Task SaveCacheAsync(CurrenciesOnDate currenciesOnDate)
        {
            string jsonedCurrencies = JsonSerializer.Serialize(currenciesOnDate.Currencies);

            string path = $@"{_basePath}{currenciesOnDate.Date:yyyy-MM-dd-HH-mm-ss}.txt";

            using StreamWriter writer = new(path, false);

            await writer.WriteLineAsync(jsonedCurrencies);
        }

        /// <summary>
        /// Метод проверки последнего кэша
        /// </summary>
        /// <param name="currencyCode">Код валюты</param>
        /// <returns>Модель курса валюты</returns>
        private async Task<CurrencyDTO?> CheckCache(CurrencyCode currencyCode)
        {
            var files = new DirectoryInfo(_basePath).GetFiles();

            var lastFile = files.MaxBy(f => f.Name);

            if (TryParseDateTimeFromFileName(lastFile, out DateTime output) && DateTime.Now - output <= new TimeSpan(2, 0, 0))
            {
                return await LoadCache(currencyCode, lastFile.FullName);
            }
            return null;
        }

        /// <summary>
        /// Метод проверки кэша с датой
        /// </summary>
        /// <param name="currencyCode">Код валюты</param>
        /// <param name="date">Дата актуальности кэша</param>
        /// <returns>Модель курса валюты</returns>
        private async Task<CurrencyDTO?> CheckCache(CurrencyCode currencyCode, DateOnly date)
        {
            var files = new DirectoryInfo(_basePath).GetFiles();

            var filesOnData = files.Where(x => x.Name.StartsWith(date.ToString("yyyy-MM-dd")));

            var lastFile = filesOnData.MaxBy(f => f.Name);

            if (TryParseDateTimeFromFileName(lastFile, out DateTime output))
            {
                return await LoadCache(currencyCode, lastFile.FullName);
            }
            return null;
        }

        /// <summary>
        /// Загрузка кэшированных данных из файла
        /// </summary>
        /// <param name="currencyCode">Код валюты</param>
        /// <param name="path">Путь к файлу</param>
        /// <returns>Модель курса валюты</returns>
        private async Task<CurrencyDTO> LoadCache(CurrencyCode currencyCode, string path)
        {
            using StreamReader reader = new(path);

            string json = await reader.ReadToEndAsync();

            var cachedInfo = JsonSerializer.Deserialize<Currency[]>(json);

            var result = cachedInfo.FirstOrDefault(c => c.Code.Equals(Enum.GetName(currencyCode), StringComparison.OrdinalIgnoreCase));

            return new CurrencyDTO(currencyCode, GetRounded(result.Value));
        }

        /// <summary>
        /// Метод для преобразования названия файла в формат времени
        /// </summary>
        /// <param name="file">файл</param>
        /// <param name="output">дата и время</param>
        /// <returns>Удалось преобразовать</returns>
        private static bool TryParseDateTimeFromFileName(FileInfo? file, out DateTime output)
        {
            bool result = DateTime.TryParseExact(
                s: file?.Name[..^4],
                format: "yyyy-MM-dd-HH-mm-ss",
                provider: System.Globalization.CultureInfo.InvariantCulture,
                style: System.Globalization.DateTimeStyles.None,
                result: out DateTime innerOutput);

            output = innerOutput;
            return result;
        }

        /// <summary>
        /// Округление до количества знаков согласно конфигурации
        /// </summary>
        /// <param name="source">входящее значение</param>
        /// <returns>округленное значение</returns>
        private float GetRounded(float source) => (float)Math.Round(source, _settings.CurrencyRoundCount);
    }
}
