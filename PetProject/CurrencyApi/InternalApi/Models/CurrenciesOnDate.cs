using Fuse8_ByteMinds.SummerSchool.InternalApi.Models;
using Fuse8_ByteMinds.SummerSchool.InternalApi.Models.ExternalApiResponseModels;
using System.Text.Json;

namespace InternalApi.Models
{
    /// <summary>
    /// Данные о курсах валют на определенную дату
    /// </summary>
    public class CurrenciesOnDate
    {
        private Currency[] _currencies;
        
        public string CurrenciesAsJson { get; set; }

        /// <summary>
        /// Дата актуальности курсов валют
        /// </summary>
        public DateTime Date { get; init; }

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
