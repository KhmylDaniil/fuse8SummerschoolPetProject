namespace Fuse8_ByteMinds.SummerSchool.PublicApi.Models
{
    /// <summary>
    /// Настройки приложения в базе данных
    /// </summary>
    public class Settings
    {
        /// <summary>
        /// Айди (отдельный ключ требуется для заполнения таблицы данными через конфигурацию)
        /// </summary>
        public int Id { get; set; }
        
        /// <summary>
        /// Код валюты по умолчанию
        /// </summary>
        public CurrencyCode DefaultCurrency { get; set; }

        /// <summary>
        /// Количество знаков после запятой, до которого следует округлять значение курса валют
        /// </summary>
        public int CurrencyRoundCount { get; set; }
    }
}
