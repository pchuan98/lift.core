using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using Microsoft.VisualBasic;

namespace Lift.Core.ImageArray.Extensions;

/// <summary>
/// 
/// </summary>
public static partial class MatsExtension
{
    /// <summary>
    /// 
    /// </summary>
    /// <param name="mats"></param>
    /// <param name="thread"></param>
    /// <returns></returns>
    /// <exception cref="InvalidException"></exception>
    public static bool SameShape(this IEnumerable<Mat> mats, int thread = 8)
    {
        // todo:后面可以使用多线程+二分法快速完成整个过程
        var array = mats.ToArray();

        var isSame = false;
        var count = array.Length;
        if (count == 0)
            throw new InvalidException("The count of mats can`t zero.");

        var tp = array[0].Type();
        var size = array[0].Size();

        var lSame = new bool[count];
        lSame[0] = true;

        Parallel.For(1, count, new ParallelOptions() { MaxDegreeOfParallelism = thread }, (z) =>
        {
            lSame[z] = array[z].Type() == tp && array[z].Size() == size;
        });

        isSame = lSame.All(x => x);
        return isSame;
    }

    /// <summary>
    /// 
    /// </summary>
    /// <param name="mats"></param>
    /// <returns></returns>
    public static bool SameType(this IEnumerable<Mat> mats)
    {
        var array = mats.ToArray();

        if (array.Length <= 1) return true;
        var type = array[0].Type();

        return array.All(item => item.Type() == type);
    }

}
