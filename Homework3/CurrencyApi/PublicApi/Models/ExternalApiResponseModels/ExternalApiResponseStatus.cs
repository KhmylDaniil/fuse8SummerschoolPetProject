using System.Text.Json.Serialization;

namespace Fuse8_ByteMinds.SummerSchool.PublicApi.Models.ExternalApiResponseModels
{
    /// <summary>
    /// Модель для десериализации ответа на запрос статуса
    /// </summary>
    public class ExternalApiResponseStatus
    {
        [JsonPropertyName("quotas")]
        public Dictionary<string, Quota> Quotas { get; set; }

        public class Quota
        {
            [JsonPropertyName("total")]
            public int Total { get; set; }

            [JsonPropertyName("used")]
            public int Used { get; set; }
        }
    }
}
