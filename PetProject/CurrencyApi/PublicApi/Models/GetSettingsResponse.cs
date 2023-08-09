using Microsoft.AspNetCore.Mvc;

namespace Fuse8_ByteMinds.SummerSchool.PublicApi.Models
{
    /// <summary>
    /// Ответ на запрос текущих настроек приложения
    /// </summary>
    public record GetSettingsResponse
    {
        /// <summary>
        /// Текущий курс валют по умолчанию из конфигурации
        /// </summary>
        public string? DefaultCurrency { get; init; }

        /// <summary>
        /// Базовая валюта, относительно которой считается курс
        /// </summary>
        public string? BaseCurrency { get; init; }

        /// <summary>
        /// Доступны ли новые запросы
        /// </summary>
        public bool NewRequestsAvailable { get; init; }

        /// <summary>
        /// Количество знаков после запятой, до которого следует округлять значение курса валют
        /// </summary>
        public int CurrencyRoundCount { get; init; }
    }
}
