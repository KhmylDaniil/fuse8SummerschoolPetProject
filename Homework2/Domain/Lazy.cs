﻿namespace Fuse8_ByteMinds.SummerSchool.Domain;


/// <summary>
/// Вариант, который вызывает экшен при первом обращении к свойству Value (буквальное требование ДЗ)
/// </summary>
public class Lazy<TValue>
{
    private TValue _value;
    private bool _isInvoked;

    public TValue? Value
    {
        get
        {
            if (!_isInvoked)
            {
                _value = Func.Invoke();
                _isInvoked = true;
            }
            return _value;
        }
    }

    private Func<TValue> Func { get; }

    public Lazy(Func<TValue> func)
    {
        Func = func;
    }
}
