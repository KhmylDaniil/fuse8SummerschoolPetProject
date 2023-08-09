using System.Text.Json.Serialization;

namespace Fuse8_ByteMinds.SummerSchool.PublicApi.Models
{
    /// <summary>
    /// Ответ на запрос курса валюты
    /// </summary>
    public record GetCurrencyResponse
    {
        /// <summary>
        /// Код валюты
        /// </summary>
        [JsonPropertyName("code")]
        public string Code { get; init; }

        /// <summary>
        /// Текущий курс относительно базовой валюты
        /// </summary>
        [JsonPropertyName("value")]
        public float Value { get; init; }
    }
}
