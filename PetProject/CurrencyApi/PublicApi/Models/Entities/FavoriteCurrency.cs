using Fuse8_ByteMinds.SummerSchool.PublicApi.Models.Responses;

namespace Fuse8_ByteMinds.SummerSchool.PublicApi.Models.Entities
{
    /// <summary>
    /// Избранная валюта
    /// </summary>
    public class FavoriteCurrency
    {
        /// <summary>
        /// Конструктор для <see cref="FavoriteCurrency"/>
        /// </summary>
        /// <param name="name">Название</param>
        /// <param name="currency">Код валюты</param>
        /// <param name="baseCurrency">Код базовой валюты</param>
        public FavoriteCurrency(string name, CurrencyCode currency, CurrencyCode baseCurrency)
        {
            Name = name;
            Currency = currency;
            BaseCurrency = baseCurrency;
        }

        /// <summary>
        /// Конструктор для <see cref="FavoriteCurrency"/> для EF
        /// </summary>
        protected FavoriteCurrency() { }

        /// <summary>
        /// Идентификатор
        /// </summary>
        public int Id { get; set; }
        
        /// <summary>
        /// Название
        /// </summary>
        public string Name { get; private set; }

        /// <summary>
        /// Код валюты
        /// </summary>
        public CurrencyCode Currency { get; private set; }

        /// <summary>
        /// Код базовой валюты, к которой рассчитывается курс
        /// </summary>
        public CurrencyCode BaseCurrency { get; private set; }

        /// <summary>
        /// Изменить избранную валюту
        /// </summary>
        /// <param name="name">Название</param>
        /// <param name="currency">Код валюты</param>
        /// <param name="baseCurrency">Код базовой валюты</param>
        public void ChangeFavCur(string name, CurrencyCode currency, CurrencyCode baseCurrency)
        {
            Name = name;
            Currency = currency;
            BaseCurrency = baseCurrency;
        }

        /// <summary>
        /// Вывод ответа в контроллер
        /// </summary>
        /// <returns></returns>
        public GetFavoredCurrencyResponse ToResponse() => new(Name, Currency, BaseCurrency);
    }
}
