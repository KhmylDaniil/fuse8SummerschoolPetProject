namespace Fuse8_ByteMinds.SummerSchool.InternalApi.Models.ExternalApiResponseModels
{
    /// <summary>
    /// Модель для десериализации ответа на запрос курса валют
    /// </summary>
    public class ExternalApiResponseLatest
    {
        public Dictionary<string, string> meta { get; set; }

        public Dictionary<string, GetCurrencyResponse> data { get; set; }
    }
}
