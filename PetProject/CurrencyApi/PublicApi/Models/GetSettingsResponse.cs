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
        /// Общее количество доступных запросов, полученное от внешнего API (quotas->month->total)
        /// </summary>
        public int RequestLimit { get; init; }

        /// <summary>
        /// Количество использованных запросов, полученное от внешнего API (quotas->month->used)
        /// </summary>
        public int RequestCount { get; init; }

        /// <summary>
        /// Количество знаков после запятой, до которого следует округлять значение курса валют
        /// </summary>
        public int CurrencyRoundCount { get; init; }
    }
}
