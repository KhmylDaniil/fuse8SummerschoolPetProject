namespace Fuse8_ByteMinds.SummerSchool.PublicApi.Models.Responses
{
    /// <summary>
    /// Ответ на запрос избранного курса валюты
    /// </summary>
    public record GetFavoredCurrencyResponse
    {
        public GetFavoredCurrencyResponse(string name, CurrencyCode currency, CurrencyCode baseCurrency)
        {
            Name = name;
            Currency = currency;
            BaseCurrency = baseCurrency;
        }

        /// <summary>
        /// Название
        /// </summary>
        public string Name { get; init; }

        /// <summary>
        /// Код валюты
        /// </summary>
        public CurrencyCode Currency { get; init; }

        /// <summary>
        /// Код базовой валюты, к которой рассчитывается курс
        /// </summary>
        public CurrencyCode BaseCurrency { get; init; }
    }
}
