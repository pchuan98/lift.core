using System;
using System.Collections.Generic;
using System.Diagnostics;
using System.Linq;
using System.Numerics;
using System.Text;
using System.Threading.Tasks;

namespace Lift.Core.ImageArray.Extensions;

public static partial class MatExtension
{
    /// <summary>
    /// 
    /// </summary>
    /// <param name="left"></param>
    /// <param name="right"></param>
    /// <param name="overlap"></param>
    /// <param name="offset">
    ///a     Vertical offset of the right image
    /// if the value > 0, the right image will be moved down
    /// </param>
    /// <returns></returns>
    public static Mat? MergeFromRight(this Mat left, Mat right, int overlap, int offset = 0)
    {
        if (left.Type() != right.Type()) return null;

        Debug.WriteLine($"{overlap}");

        var width = right.Width + left.Width - overlap;
        var height = Math.Max(left.Height, right.Height) + Math.Abs(offset);

        var result = new Mat(new Size(width, height), left.Type());

        left.CopyTo(result[new Rect(0, offset < 0 ? -offset : 0, left.Width, left.Height)]);
        right.CopyTo(result[new Rect(left.Width - overlap, offset > 0 ? offset : 0, right.Width, right.Height)]);

        var center = left.Width - overlap;
        var threshold = 10;
        var roi = new Rect(center - 10, 0, 2 * threshold, height);

        var gaussian = new Mat();
        Cv2.GaussianBlur(new Mat(result, roi), gaussian, new Size(3, 3), 0);


        //gaussian.CopyTo(result[roi]);
        return result;
    }

    /// <summary>
    /// 自动从左到右拼接
    /// </summary>
    /// <param name="left"></param>
    /// <param name="right"></param>
    /// <param name="overlap"></param>
    /// <param name="offset"></param>
    /// <returns></returns>
    public static Mat? AutoMergeFromRight(this Mat left, Mat right, int overlap, int offset)
    {
        if (left.Type() != right.Type())
            throw new System.Exception("The format of mat must be consistant.");

        if (left is { Width: 0, Height: 0 } && right.Width != 0 && right.Height != 0) return right;
        if(right is { Width: 0, Height: 0 } ) return left;

        return null;
    }
}
