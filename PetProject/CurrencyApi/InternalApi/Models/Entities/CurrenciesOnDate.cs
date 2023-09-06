using Fuse8_ByteMinds.SummerSchool.InternalApi.Models.ExternalApiResponseModels;

namespace InternalApi.Models.Entities
{
    /// <summary>
    /// Данные о курсах валют на определенную дату
    /// </summary>
    public class CurrenciesOnDate
    {
        /// <summary>
        /// Дата актуальности курсов валют
        /// </summary>
        public DateTime Date { get; set; }

        /// <summary>
        /// Курсы валют
        /// </summary>
        public Currency[] Currencies { get; set; }
    }

    /// <summary>
    /// Модель для использования данных о курсах валют в кеше памяти
    /// </summary>
    public class CachedCurrenciesOnDate : CurrenciesOnDate
    {
        public CachedCurrenciesOnDate(CurrenciesOnDate currenciesOnDate)
        {
            Date = currenciesOnDate.Date;
            Currencies = currenciesOnDate.Currencies;
        }
    }
}
