namespace Fuse8_ByteMinds.SummerSchool.PublicApi.Models
{
    /// <summary>
    /// Ответ на запрос курса валюты с указанием даты актуальности курса
    /// </summary>
    public record GetCurrencyHistoricalResponse : GetCurrencyResponse
    {
        /// <summary>
        /// Дата актуальности курса в формате yyyy-mm-dd
        /// </summary>
        public string? Date { get; init; }
    }
}
