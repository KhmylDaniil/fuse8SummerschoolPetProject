using System.Net;

namespace Fuse8_ByteMinds.SummerSchool.Domain;

public static class ExceptionHandler
{
	/// <summary>
	/// Обрабатывает исключение, которое может возникнуть при выполнении <paramref name="action"/>
	/// </summary>
	/// <param name="action">Действие, которое может породить исключение</param>
	/// <returns>Сообщение об ошибке</returns>
	public static string? Handle(Action action)
	{
		try
		{
            action();
            return null;
        }
        catch (HttpRequestException ex) when (ex.StatusCode == HttpStatusCode.NotFound)
        {
            return "Ресурс не райден";
        }
        catch (HttpRequestException ex)
		{
			return ex.StatusCode.ToString();
		}
		catch (MoneyException ex)
		{
			return ex.Message;
		}
		catch (Exception)
		{
			return "Произошла непредвиденная ошибка";
        }
    }
}

public class MoneyException : Exception
{
    public new string Message { get; protected set; }

    public MoneyException()
	{
	}

	public MoneyException(string? message)
		: base(message)
	{
	}
}

public class NotValidKopekCountException : MoneyException
{
	public NotValidKopekCountException() : base()
	{
        Message = "Количество копеек должно быть больше 0 и меньше 99";
    }
}

public class NegativeRubleCountException : MoneyException
{
	public NegativeRubleCountException() : base()
	{
		Message = "Число рублей не может быть отрицательным";
    }
}
