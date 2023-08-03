namespace Fuse8_ByteMinds.SummerSchool.InternalApi.Models.ExternalApiResponseModels
{
    /// <summary>
    /// Модель для десериализации ответа на запрос статуса
    /// </summary>
    public class ExternalApiResponseStatus
    {
        public Dictionary<string, Quota> quotas { get; set; }

        public class Quota
        {
            public int total { get; set; }
            public int used { get; set; }
        }
    }
}
