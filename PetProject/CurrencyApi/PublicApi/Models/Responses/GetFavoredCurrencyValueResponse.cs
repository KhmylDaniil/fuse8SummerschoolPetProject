namespace Fuse8_ByteMinds.SummerSchool.PublicApi.Models.Responses
{
    /// <summary>
    /// Ответ на запрос значения избранного курса валюты
    /// </summary>
    public record GetFavoredCurrencyValueResponse : GetFavoredCurrencyResponse
    {
        public GetFavoredCurrencyValueResponse(string name, CurrencyCode currency, CurrencyCode baseCurrency, float value)
            : base(name, currency, baseCurrency)
        {
            Value = value;
        }

        /// <summary>
        /// Значение курса валюты к базовой
        /// </summary>
        public float Value { get; init; }
    }
}
