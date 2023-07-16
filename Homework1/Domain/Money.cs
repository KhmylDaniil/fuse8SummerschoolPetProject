namespace Fuse8_ByteMinds.SummerSchool.Domain;

/// <summary>
/// Модель для хранения денег
/// </summary>
public class Money : IComparable<Money>
{
	const int KopeksInRuble = 100;

	public Money(int rubles, int kopeks)
		: this(false, rubles, kopeks)
	{
	}

	public Money(bool isNegative, int rubles, int kopeks)
	{
		IsNegative = rubles == 0 && kopeks == 0 && isNegative
			? throw new ArgumentException("Zero money mustn`t be negative.")
			: isNegative;

        Rubles = rubles >= 0 ? rubles
			: throw new ArgumentException("Value can`t be negative.");
		
		Kopeks = kopeks >= 0 && kopeks < KopeksInRuble ? kopeks
			: throw new ArgumentException($"Value must be in range greater or equal to 0 and less than {KopeksInRuble}.");
    }

	/// <summary>
	/// Отрицательное значение
	/// </summary>
	public bool IsNegative { get; }

	/// <summary>
	/// Число рублей
	/// </summary>
	public int Rubles { get; }

	/// <summary>
	/// Количество копеек
	/// </summary>
	public int Kopeks { get; }

	/// <summary>
	/// Реализация интерфейса сравнения
	/// </summary>
	/// <returns></returns>
    public int CompareTo(Money? other)
    {
		if (other is null) return 1;

		if (IsNegative && !other.IsNegative) return -1;
		else if (!IsNegative && other.IsNegative) return 1;

		return IsNegative && other.IsNegative
			? ModuleMoneyCompare(other) * -1
			: ModuleMoneyCompare(other);
    }


    public static bool operator >(Money left, Money right) => left.CompareTo(right) > 0;
    public static bool operator <(Money left, Money right) => left.CompareTo(right) < 0;
	public static bool operator >=(Money left, Money right) => left.CompareTo(right) >= 0;
    public static bool operator <=(Money left, Money right) => left.CompareTo(right) <= 0;

	/// <summary>
	/// Сложение денег с одинаковым знаком
	/// </summary>
	/// <param name="firstTerm">Первое слагаемое</param>
	/// <param name="secondTerm">Второе слагаемое</param>
	/// <returns>Сумма</returns>
	static Money MoneyWithSameSignAddition(Money firstTerm, Money secondTerm)
    {
		return new(
            isNegative: firstTerm.IsNegative,
            kopeks: (firstTerm.Kopeks + secondTerm.Kopeks) % KopeksInRuble,
            rubles: firstTerm.Rubles + secondTerm.Rubles + ((firstTerm.Kopeks + secondTerm.Kopeks) / KopeksInRuble));
    }

    /// <summary>
    /// Вычитание денег с одинаковым знаком
    /// </summary>
    /// <param name="minuend">Уменьшаемое</param>
    /// <param name="subtrahend">Вычитаемое</param>
    /// <param name="notSameSignAsMinuend">Будет ли разность иного знака, чем уменьшаемое</param>
    /// <returns>Разность</returns>
    static Money MoneyWithSameSignSubstraction(Money minuend, Money subtrahend, bool notSameSignAsMinuend)
	{
        int substractAdditionalRubel = minuend.Kopeks < subtrahend.Kopeks ? 1 : 0;

        return new(
			isNegative: notSameSignAsMinuend,
            kopeks: minuend.Kopeks - subtrahend.Kopeks + substractAdditionalRubel * KopeksInRuble,
            rubles: minuend.Rubles - subtrahend.Rubles - substractAdditionalRubel);
    }

	/// <summary>
	/// Оператор сложения денег
	/// </summary>
	/// <param name="left">Слагаемое1</param>
	/// <param name="right">Слагаемое2</param>
	/// <returns>Сумма</returns>
    public static Money operator +(Money left, Money right)
	{
		if (left.IsNegative == right.IsNegative)
			return MoneyWithSameSignAddition(left, right);

		return left.ModuleMoneyCompare(right) switch
        {
            1 => MoneyWithSameSignSubstraction(left, right, left.IsNegative),
            -1 => MoneyWithSameSignSubstraction(right, left, right.IsNegative),
            0 => new(0, 0),
            _ => throw new ArgumentException($"{nameof(ModuleMoneyCompare)} results with unexpected error.")
        };
    }

	/// <summary>
	/// Оператор вычитания денег
	/// </summary>
	/// <param name="left">Умеьшаемое</param>
	/// <param name="right">Вычитаемое</param>
	/// <returns>Разность</returns>
    public static Money operator -(Money left, Money right)
	{
		if (left.IsNegative != right.IsNegative)
			return MoneyWithSameSignAddition(left, right);

        return left.ModuleMoneyCompare(right) switch
        {
            1 => MoneyWithSameSignSubstraction(left, right, left.IsNegative),
            -1 => MoneyWithSameSignSubstraction(right, left, !left.IsNegative),
            0 => new(0, 0),
            _ => throw new ArgumentException($"{nameof(ModuleMoneyCompare)} results with unexpected error.")
        };
    }

    public override bool Equals(object? obj) => Equals(obj as Money);
	public bool Equals(Money? money) => money is not null && CompareTo(money) == 0;

	public override int GetHashCode() => HashCode.Combine(IsNegative, Rubles, Kopeks);

    public override string ToString()
    {
		string negativeSign = IsNegative ? "-" : string.Empty;
        return string.Format("{0}{1},{2}", negativeSign, Rubles, Kopeks);
    }

    /// <summary>
    /// Сравнение денег по модулю
    /// </summary>
    /// <param name="other">Сравнивамое число</param>
    /// <returns>Результат сравнения по модулю</returns>
    private int ModuleMoneyCompare(Money other)
    {
        if (Rubles > other.Rubles) return 1;
        else if (Rubles < other.Rubles) return -1;

        if (Kopeks > other.Kopeks) return 1;
        else if (Kopeks < other.Kopeks) return -1;

        return 0;
    }
}