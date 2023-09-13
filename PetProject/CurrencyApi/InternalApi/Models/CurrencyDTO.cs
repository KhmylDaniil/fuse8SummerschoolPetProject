
using Fuse8_ByteMinds.SummerSchool.InternalApi;

namespace InternalApi.Models
{
    /// <summary>
    /// Курс валюты
    /// </summary>
    /// <param name="CurrencyCode">Валюта</param>
    /// <param name="Value">Значение курса</param>
    public record CurrencyDto(CurrencyCode CurrencyCode, float Value);
}
