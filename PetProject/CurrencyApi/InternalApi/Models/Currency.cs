using System.Text.Json.Serialization;

namespace Fuse8_ByteMinds.SummerSchool.InternalApi.Models
{
    /// <summary>
    /// Данные о курсе валюты
    /// </summary>
    public record Currency
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
