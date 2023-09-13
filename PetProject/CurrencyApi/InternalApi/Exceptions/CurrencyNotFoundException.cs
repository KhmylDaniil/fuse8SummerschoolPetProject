namespace Fuse8_ByteMinds.SummerSchool.InternalApi.Exceptions
{
    /// <summary>
    /// Исключение неподдерживаемого кода валюты
    /// </summary>
    public class CurrencyNotFoundException : Exception
    {
        /// <summary>
        /// Конструктор для <see cref="CurrencyNotFoundException"/>
        /// </summary>
        public CurrencyNotFoundException() : base("Код валюты не найден") { }
    }
}
