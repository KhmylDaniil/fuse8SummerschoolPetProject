namespace Fuse8_ByteMinds.SummerSchool.Domain;

public static class DomainExtensions
{
    public static bool IsNullOrEmpty<T>(this IEnumerable<T> values) => values == null || !values.Any();

    public static string JoinToString<T>(this IEnumerable<T> values, string separator)
    {
        if (values == null || IsNullOrEmpty(separator))
            throw new ArgumentNullException();

        return string.Join(separator, values);
    }

    public static int DaysCountBetween(this DateTimeOffset first, DateTimeOffset second)
        => Math.Abs((first - second).Days);
}
