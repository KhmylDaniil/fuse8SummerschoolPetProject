using System.Text.Json.Serialization;

namespace Fuse8_ByteMinds.SummerSchool.InternalApi.Models.ExternalApiResponseModels
{
    /// <summary>
    /// Модель для десериализации ответа на запрос курса валют
    /// </summary>
    public class ExternalApiResponseCurrencies
    {
        [JsonPropertyName("meta")]
        public Dictionary<string, string> Meta { get; set; }

        [JsonPropertyName("data")]
        public Dictionary<string, Currency> Data { get; set; }
    }
}
