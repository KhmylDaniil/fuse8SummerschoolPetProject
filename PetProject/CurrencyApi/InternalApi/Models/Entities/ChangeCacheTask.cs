using Fuse8_ByteMinds.SummerSchool.InternalApi;

namespace InternalApi.Models.Entities
{
    /// <summary>
    /// Задача по пересчету кэша
    /// </summary>
    public class ChangeCacheTask
    {
        /// <summary>
        /// Конструктор
        /// </summary>
        /// <param name="newBaseCurrency">Код новой базовой валюты</param>
        public ChangeCacheTask(CurrencyCode newBaseCurrency)
        {
            NewBaseCurrency = newBaseCurrency;
            CreationTime = DateTime.UtcNow;
            CacheTaskStatus = CacheTaskStatus.Created;
        }

        /// <summary>
        /// Конструктор для EF
        /// </summary>
        protected ChangeCacheTask() { }

        /// <summary>
        /// Идентификатор
        /// </summary>
        public Guid Id { get; set; }

        /// <summary>
        /// Время создания
        /// </summary>
        public DateTime CreationTime { get; set; }

        /// <summary>
        /// Код новой базовой валюты
        /// </summary>
        public CurrencyCode NewBaseCurrency { get; set; }

        /// <summary>
        /// Статус задачи
        /// </summary>
        public CacheTaskStatus CacheTaskStatus { get; set; }
    }

    /// <summary>
    /// Статус задачи
    /// </summary>
    public enum CacheTaskStatus
    {
        Created = 0,
        Processing,
        Success,
        Error,
        Canceled
    }
}
