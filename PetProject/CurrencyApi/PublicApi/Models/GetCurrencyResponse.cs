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
        public CurrencyCode Code { get; init; }

        /// <summary>
        /// Текущий курс относительно базовой валюты
        /// </summary>
        public float Value { get; init; }
    }
}
