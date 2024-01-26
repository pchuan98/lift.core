using System;
using System.Collections.Generic;
using System.Text;

namespace Lift.Core.Share.Extensions;

/// <summary>
/// 对原生的Linq语法的扩展
/// </summary>
public static class LinqExtension
{
    /// <summary>
    /// 遍历所有的结果并且操作
    /// </summary>
    /// <typeparam name="T"></typeparam>
    /// <param name="source"></param>
    /// <param name="action"></param>
    public static IEnumerable<T> Do<T>(this IEnumerable<T> source, Action<T> action)
    {
        var enumerable = source as T[] ?? source.ToArray();
        foreach (var item in enumerable)
            action(item);

        return enumerable;
    }

    /// <summary>
    /// 遍历所有结果并且操作（带index）
    /// </summary>
    /// <typeparam name="T"></typeparam>
    /// <param name="source"></param>
    /// <param name="action"></param>
    public static IEnumerable<T> Do<T>(this IEnumerable<T> source, Action<int, T> action)
    {
        var index = 0;
        var enumerable = source as T[] ?? source.ToArray();
        foreach (var item in enumerable)
            action(index++, item);

        return enumerable;
    }

    /// <summary>
    /// 排序
    /// </summary>
    /// <typeparam name="T"></typeparam>
    /// <param name="source"></param>
    /// <param name="func"></param>
    /// <returns></returns>
    public static IEnumerable<T> Sort<T>(this IEnumerable<T> source, Func<T, T, int> func)
        // note: return value of Compare<T>.Create is `int`
        //  the rule is Negative numbers are less than, 0 is equal to, positive numbers are greater than
        => source.OrderBy(item => item, Comparer<T>.Create((left, right) => func(right, left)));
}
