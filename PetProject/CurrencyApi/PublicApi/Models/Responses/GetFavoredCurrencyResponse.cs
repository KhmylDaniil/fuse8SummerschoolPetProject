namespace Fuse8_ByteMinds.SummerSchool.PublicApi.Models.Responses
{
    /// <summary>
    /// Ответ на запрос избранного курса валюты
    /// </summary>
    public record GetFavoredCurrencyResponse
    {
        /// <summary>
        /// Конструктор для <see cref="GetFavoredCurrencyResponse"/>
        /// </summary>
        /// <param name="name">Название</param>
        /// <param name="currency">Код валюты</param>
        /// <param name="baseCurrency">Код базовой валюты</param>
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
