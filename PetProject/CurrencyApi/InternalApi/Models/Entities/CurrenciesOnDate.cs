using Fuse8_ByteMinds.SummerSchool.InternalApi.Models.ExternalApiResponseModels;
using System.Text.Json;

namespace InternalApi.Models.Entities
{
    /// <summary>
    /// Данные о курсах валют на определенную дату
    /// </summary>
    public class CurrenciesOnDate
    {
        private Currency[] _currencies;

        /// <summary>
        /// Значения курсов валют в json формате
        /// </summary>
        public string CurrenciesAsJson { get; set; }

        /// <summary>
        /// Дата актуальности курсов валют
        /// </summary>
        public DateTime Date { get; set; }

        /// <summary>
        /// Курсы валют
        /// </summary>
        public Currency[] Currencies
        {
            get
            {
                _currencies ??= JsonSerializer.Deserialize<Currency[]>(CurrenciesAsJson);
                return _currencies;
            }
            set
            {
                _currencies = value;
                CurrenciesAsJson = JsonSerializer.Serialize(value);
            }
        }
    }
}
