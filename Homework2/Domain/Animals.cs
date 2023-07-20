namespace Fuse8_ByteMinds.SummerSchool.Domain;

/// <summary>
/// Животное
/// </summary>
public abstract class Animal
{
	/// <summary>
	/// true - если животное является другом человека
	/// </summary>
	public virtual bool IsHumanFriend => false;

	/// <summary>
	/// true - если у животного большой вес
	/// </summary>
	public abstract bool HasBigWeight { get; }

	/// <summary>
	/// Как говорит животное
	/// </summary>
	/// <returns>Возвращает звук, который говорит животное</returns>
	public abstract string WhatDoesSay();
}

/// <summary>
/// Собака
/// </summary>
public abstract class Dog : Animal
{
	public override bool IsHumanFriend => true;

	public override string WhatDoesSay() => "гав";

    public override bool HasBigWeight => false;
}

/// <summary>
/// Лиса
/// </summary>
public class Fox : Animal
{
	public override bool HasBigWeight => false;

    public override string WhatDoesSay() => "ми-ми-ми";
}

/// <summary>
/// Чихуахуа
/// </summary>
public class Chihuahua : Dog
{
}

/// <summary>
/// Хаски
/// </summary>
public class Husky : Dog
{
    public new string WhatDoesSay() => "ауф";

    public override bool HasBigWeight => true;
}
