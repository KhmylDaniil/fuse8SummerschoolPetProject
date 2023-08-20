namespace Fuse8_ByteMinds.SummerSchool.PublicApi
{
    /// <summary>
    /// Настройки приложения по валюте
    /// </summary>
    public class CurrencySettings
    {
        /// <summary>
        /// Базовая валюта, относительно которой считается курс
        /// </summary>
        public string? BaseCurrency { get; init; }

        /// <summary>
        /// Апи ключ
        /// </summary>
        public string? ApiKey { get; init; }

        /// <summary>
        /// Максимальное количество запросов в месяц
        /// </summary>
        public int MaxRequestsPerMonth { get; init; }
    }
}
