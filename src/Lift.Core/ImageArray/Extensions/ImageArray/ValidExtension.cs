using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Lift.Core.ImageArray.Extensions;

public static partial class ImageArrayExtension
{
    /// <summary>
    /// 数据对象是否是Mat
    /// </summary>
    /// <param name="array"></param>
    /// <returns></returns>
    public static bool IsMat(this ImageArray array) => array.Flag == ArrayFlag.OpenCv;
}
