namespace Fuse8_ByteMinds.SummerSchool.PublicApi.Exceptions
{
    /// <summary>
    /// Исключение неподдерживаемого кода валюты
    /// </summary>
    public class CurrencyNotFoundException : Exception
    {
        public CurrencyNotFoundException() : base("Код валюты не найден") { }
       }
}
