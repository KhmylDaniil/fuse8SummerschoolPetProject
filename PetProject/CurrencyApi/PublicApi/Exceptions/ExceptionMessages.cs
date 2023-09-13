namespace Fuse8_ByteMinds.SummerSchool.PublicApi.Exceptions
{
    /// <summary>
    /// Сообщения об ошибках
    /// </summary>
    public static class ExceptionMessages
    {
        public const string NotSingleSettings = "Не найдены необходимые настройки приложения.";

        public const string CurrencyRoundCantBeNegative = "Точность округления не может быть меньше нуля.";

        public const string FavCurNotFound = "Запрашиваемый избранный курс валюты не найден.";

        public const string NotUniqueFavCur = "Избранный курс валюты с такой сигнатурой уже есть в базе.";

        public const string NameCantBeNull = "Название не может быть пустым.";
    }
}
