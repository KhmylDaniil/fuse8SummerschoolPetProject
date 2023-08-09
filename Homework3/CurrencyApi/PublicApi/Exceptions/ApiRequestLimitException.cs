namespace Fuse8_ByteMinds.SummerSchool.PublicApi.Exceptions
{
    /// <summary>
    /// Ошибка исчерпания количества обращений к внешнему API
    /// </summary>
    public class ApiRequestLimitException : Exception
    {
        public ApiRequestLimitException() : base("Доступные запросы исчерпаны") { }
    }
}
