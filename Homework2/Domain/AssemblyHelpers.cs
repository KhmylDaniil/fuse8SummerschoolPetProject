using System.Reflection;

namespace Fuse8_ByteMinds.SummerSchool.Domain;

public static class AssemblyHelpers
{
    /// <summary>
    /// Получает информацию о базовых типах классов из namespace "Fuse8_ByteMinds.SummerSchool.Domain", у которых есть наследники.
    /// </summary>
    /// <remarks>
    ///	Информация возвращается только по самым базовым классам.
    /// Информация о промежуточных базовых классах не возвращается
    /// </remarks>
    /// <returns>Список типов с количеством наследников</returns>
    public static (string BaseTypeName, int InheritorCount)[] GetTypesWithInheritors()
	{
		var assemblyClassTypes = Assembly.GetAssembly(typeof(AssemblyHelpers))
			!.DefinedTypes
			.Where(p => p.IsClass);

		var namesArray = assemblyClassTypes.Select(p => p.Name).ToArray();

		var onlyDerivedFromThisAssemblyClassTypesNamesArray = assemblyClassTypes
			.Where(t => !t.IsAbstract)
			.Select(type => GetBaseType(type)?.Name)
			.Where(name => namesArray.Contains(name)).ToArray();

		return onlyDerivedFromThisAssemblyClassTypesNamesArray
			.Distinct()
			.Select(x => (BaseTypeName: x, InheritorCount: onlyDerivedFromThisAssemblyClassTypesNamesArray.Count(y => y == x)))
			.ToArray();
    }

	/// <summary>
	/// Получает базовый тип для класса
	/// </summary>
	/// <param name="type">Тип, для которого необходимо получить базовый тип</param>
	/// <returns>
	/// Первый тип в цепочке наследований. Если наследования нет, возвращает null
	/// </returns>
	/// <example>
	/// Класс A, наследуется от B, B наследуется от C
	/// При вызове GetBaseType(typeof(A)) вернется C
	/// При вызове GetBaseType(typeof(B)) вернется C
	/// При вызове GetBaseType(typeof(C)) вернется C
	/// </example>
	private static Type? GetBaseType(Type type)
	{
		var baseType = type;

		while (baseType.BaseType is not null && baseType.BaseType != typeof(object))
		{
			baseType = baseType.BaseType;
		}

		return baseType == type
			? null
			: baseType;
	}
}
