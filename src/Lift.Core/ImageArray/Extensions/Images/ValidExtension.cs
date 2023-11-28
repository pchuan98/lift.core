using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Lift.Core.ImageArray.Extensions;

public static partial class ImagesExtension
{
    public static bool IsMat(this ImageArray[] images)
        => images.All(img => img.IsMat());
}
