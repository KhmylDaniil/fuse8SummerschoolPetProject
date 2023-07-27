namespace Fuse8_ByteMinds.SummerSchool.PublicApi
{
    /// <summary>
    /// Настройки приложения по валюте
    /// </summary>
    public class CurrencySettings
    {
        /// <summary>
        /// Валюта по умолчанию
        /// </summary>
        public string? DefaultCurrency { get; init; }

        /// <summary>
        /// Базовая валюта, относительно которой считается курс
        /// </summary>
        public string? BaseCurrency { get; init; }

        /// <summary>
        /// Количество знаков после запятой, до которого следует округлять значение курса валют
        /// </summary>
        public int CurrencyRoundCount { get; init; }

        /// <summary>
        /// Базовый адрес
        /// </summary>
        public string? BaseAddress { get; init; }

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
