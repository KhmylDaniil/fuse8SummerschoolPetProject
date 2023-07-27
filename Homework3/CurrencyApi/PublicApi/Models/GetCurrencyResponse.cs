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
        public string? Code { get; init; }

        /// <summary>
        /// Текущий курс относительно базовой валюты
        /// </summary>
        public decimal Value { get; init; }
    }
}
