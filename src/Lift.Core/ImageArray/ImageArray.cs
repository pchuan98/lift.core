using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Lift.Core.ImageArray;

public partial class ImageArray : IDisposable
{
    /// <summary>
    /// 
    /// </summary>
    /// <param name="mat"></param>
    public ImageArray(Mat mat)
    {
        Width = mat.Width;
        Height = mat.Height;
        StackCount = 0;

        Object = mat;
        Flag = ArrayFlag.OpenCv;
    }

    public ImageArray(Mat[] mats)
    {
        Width = mats[0].Width;
        Height = mats[0].Height;
        StackCount = mats.Length;

        Object = mats;
        Flag = ArrayFlag.OpenCv;
    }
}
