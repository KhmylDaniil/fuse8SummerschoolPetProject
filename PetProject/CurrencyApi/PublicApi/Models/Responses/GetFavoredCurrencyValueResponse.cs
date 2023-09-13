namespace Fuse8_ByteMinds.SummerSchool.PublicApi.Models.Responses
{
    /// <summary>
    /// Ответ на запрос значения избранного курса валюты
    /// </summary>
    public record GetFavoredCurrencyValueResponse : GetFavoredCurrencyResponse
    {
        /// <summary>
        /// Конструктор для <see cref="GetFavoredCurrencyValueResponse"/>
        /// </summary>
        /// <param name="name">Название</param>
        /// <param name="currency">Код валюты</param>
        /// <param name="baseCurrency">Код базовой валюты</param>
        /// <param name="value">Значение курса</param>
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
