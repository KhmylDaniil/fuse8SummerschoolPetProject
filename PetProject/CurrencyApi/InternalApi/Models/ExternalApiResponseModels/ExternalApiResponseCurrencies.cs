using System.Text.Json.Serialization;

namespace Fuse8_ByteMinds.SummerSchool.InternalApi.Models.ExternalApiResponseModels
{
    /// <summary>
    /// Модель для десериализации ответа на запрос курса валют
    /// </summary>
    public class ExternalApiResponseCurrencies
    {
        /// <summary>
        /// Метаданные
        /// </summary>
        [JsonPropertyName("meta")]
        public Dictionary<string, string> Meta { get; set; }

        /// <summary>
        /// Данные
        /// </summary>
        [JsonPropertyName("data")]
        public Dictionary<string, Currency> Data { get; set; }
    }

    /// <summary>
    /// Данные о курсе валюты
    /// </summary>
    public class Currency
    {
        /// <summary>
        /// Код валюты
        /// </summary>
        [JsonPropertyName("code")]
        public string Code { get; set; }

        /// <summary>
        /// Текущий курс относительно базовой валюты
        /// </summary>
        [JsonPropertyName("value")]
        public float Value { get; set; }
    }
}
