﻿using System.Text.Json.Serialization;
using Fuse8_ByteMinds.SummerSchool.PublicApi.Models.Responses;

namespace Fuse8_ByteMinds.SummerSchool.PublicApi.Models.ExternalApiResponseModels
{
    /// <summary>
    /// Модель для десериализации ответа на запрос курса валют
    /// </summary>
    public class ExternalApiResponseLatest
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
        public Dictionary<string, GetCurrencyResponse> Data { get; set; }
    }
}
