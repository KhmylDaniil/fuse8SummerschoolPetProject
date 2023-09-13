namespace Fuse8_ByteMinds.SummerSchool.InternalApi.Exceptions
{
    /// <summary>
    /// Ошибка исчерпания количества обращений к внешнему API
    /// </summary>
    public class ApiRequestLimitException : Exception
    {
        /// <summary>
        /// Конструктор для <see cref="ApiRequestLimitException"/>
        /// </summary>
        public ApiRequestLimitException() : base("Доступные запросы исчерпаны") { }
    }
}
