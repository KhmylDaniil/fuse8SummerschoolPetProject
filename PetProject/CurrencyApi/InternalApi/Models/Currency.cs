namespace Fuse8_ByteMinds.SummerSchool.InternalApi.Models
{
    /// <summary>
    /// Данные о курсе валюты
    /// </summary>
    public record Currency
    {
        /// <summary>
        /// Код валюты
        /// </summary>
        public string Code { get; init; }

        /// <summary>
        /// Текущий курс относительно базовой валюты
        /// </summary>
        public float Value { get; init; }
    }
}
