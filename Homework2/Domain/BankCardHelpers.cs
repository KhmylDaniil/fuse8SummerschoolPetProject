using System.ComponentModel.DataAnnotations;
using System.Reflection;

namespace Fuse8_ByteMinds.SummerSchool.Domain;

public static class BankCardHelpers
{
    /// <summary>
    /// Получает номер карты без маски
    /// </summary>
    /// <param name="card">Банковская карта</param>
    /// <returns>Номер карты без маски</returns>
    public static string GetUnmaskedCardNumber(BankCard card)
    {
        var fieldInfo = typeof(BankCard).GetField("_number", BindingFlags.Instance | BindingFlags.NonPublic);

        return fieldInfo.GetValue(card).ToString();
    }

    /*
    /// <summary>
    /// Получает номер карты без маски и без знания о названиях полей класса BankCard
    /// В тестах не работает, потому что в них заданы некорректные номера карт
    /// </summary>
    /// <param name="card">Банковская карта</param>
    /// <returns>Номер карты без маски</returns>
    public static string GetUnmaskedCardNumber(BankCard card)
    {
        var fieldInfos = typeof(BankCard).GetFields(BindingFlags.Instance | BindingFlags.NonPublic);

        CreditCardAttribute creditCardAttribute = new();

        foreach (var fieldInfo in fieldInfos)
        {
            var cardFieldData = typeof(BankCard).GetField(fieldInfo.Name, BindingFlags.Instance | BindingFlags.NonPublic).GetValue(card).ToString();

            if (creditCardAttribute.IsValid(cardFieldData))
                return cardFieldData;
        }
        return string.Empty;
    }
    */
}
