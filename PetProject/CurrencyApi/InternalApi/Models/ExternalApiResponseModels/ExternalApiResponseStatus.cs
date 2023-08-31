using System.Text.Json.Serialization;

namespace Fuse8_ByteMinds.SummerSchool.InternalApi.Models.ExternalApiResponseModels
{
    /// <summary>
    /// Модель для десериализации ответа на запрос статуса
    /// </summary>
    public class ExternalApiResponseStatus
    {
        /// <summary>
        /// Квоты
        /// </summary>
        [JsonPropertyName("quotas")]
        public Dictionary<string, Quota> Quotas { get; set; }

        /// <summary>
        /// Квота
        /// </summary>
        public class Quota
        {
            /// <summary>
            /// Всего
            /// </summary>
            [JsonPropertyName("total")]
            public int Total { get; set; }

            /// <summary>
            /// Использовано
            /// </summary>
            [JsonPropertyName("used")]
            public int Used { get; set; }
        }
    }
}
