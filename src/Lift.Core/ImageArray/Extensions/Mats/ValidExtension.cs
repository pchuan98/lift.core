using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Lift.Core.ImageArray.Extensions;

public static partial class MatsExtension
{
    /// <summary>
    /// 
    /// </summary>
    /// <param name="mats"></param>
    /// <param name="thread"></param>
    /// <returns></returns>
    /// <exception cref="InvalidException"></exception>
    public static bool SameShape(this Mat[] mats, int thread = 8)
    {
        // todo:后面可以使用多线程+二分法快速完成整个过程

        var isSame = false;
        var count = mats.Length;
        if (count == 0)
            throw new InvalidException("The count of mats can`t zero.");

        var tp = mats[0].Type();
        var size = mats[0].Size();

        var lSame = new bool[count];
        lSame[0] = true;

        Parallel.For(1, count, new ParallelOptions() { MaxDegreeOfParallelism = thread }, (z) =>
        {
            lSame[z] = mats[z].Type() == tp && mats[z].Size() == size;
        });

        isSame = lSame.All(x => x);
        return isSame;
    }

}
