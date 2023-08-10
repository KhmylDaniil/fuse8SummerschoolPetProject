using System.Text.Json.Serialization;

namespace Fuse8_ByteMinds.SummerSchool.PublicApi.Models.ExternalApiResponseModels
{
    /// <summary>
    /// Модель для десериализации ответа на запрос курса валют
    /// </summary>
    public class ExternalApiResponseLatest
    {
        [JsonPropertyName("meta")]
        public Dictionary<string, string> Meta { get; set; }


        [JsonPropertyName("data")]
        public Dictionary<string, GetCurrencyResponse> Data { get; set; }
    }
}
