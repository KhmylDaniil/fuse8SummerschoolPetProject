using Fuse8_ByteMinds.SummerSchool.InternalApi.Models;

namespace InternalApi.Models
{
    /// <summary>
    /// Данные о курсах валют на определенную дату
    /// </summary>
    public class CurrenciesOnDate
    {
        /// <summary>
        /// Дата актуальности курсов валют
        /// </summary>
        public DateTime Date { get; init; }

        /// <summary>
        /// Курсы валют
        /// </summary>
        public Currency[] Currencies { get; init; }
    }
}
