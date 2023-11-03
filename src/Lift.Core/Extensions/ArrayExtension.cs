using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Lift.Core.Extensions;

public static class ArrayExtension
{
    /// <summary>
    /// 
    /// </summary>
    /// <param name="array"></param>
    /// <returns></returns>
    public static Mat? ToMat(this ImageArray array)
        => array?.Object as Mat;
}
