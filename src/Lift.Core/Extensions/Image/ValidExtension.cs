using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using Lift.Core.ImageArray;

namespace Lift.Core.Extensions.Image;

public static class ValidExtension
{
    public static bool IsImage(this ImageArray array)
        => (array.Flag & ArrayFlag.Image) != 0;

    public static bool IsMat(this ImageArray array) => array.Flag == ArrayFlag.OpenCv;
}
