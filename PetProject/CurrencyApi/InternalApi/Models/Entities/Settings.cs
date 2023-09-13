using Fuse8_ByteMinds.SummerSchool.InternalApi;

namespace InternalApi.Models.Entities
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
        /// Код базовой валюты
        /// </summary>
        public CurrencyCode BaseCurrency { get; set; }
    }
}
