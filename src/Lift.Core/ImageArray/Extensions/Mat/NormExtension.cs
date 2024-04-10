using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using static System.Net.Mime.MediaTypeNames;

namespace Lift.Core.ImageArray.Extensions;

// hack: I feel like there may be something wrong with S32. Is there an upper and lower limit for int?

public static partial class MatExtension
{
    /// <summary>
    /// Max value
    /// </summary>
    /// <param name="mat"></param>
    /// <returns></returns>
    /// <exception cref="System.Exception"></exception>
    static double GetAlpha(Mat mat)
        => mat.Depth() switch
        {
            MatDepth.U8 => MatRange.MaxU8,
            MatDepth.S8 => MatRange.MaxU8,
            MatDepth.U16 => MatRange.MaxU16,
            MatDepth.S16 => MatRange.MaxU16,
            MatDepth.S32 => MatRange.MaxS32,
            MatDepth.F32 => MatRange.MaxF32,
            MatDepth.F64 => MatRange.MaxF64,
            _ => throw new System.Exception()
        };

    /// <summary>
    /// Min value
    /// </summary>
    /// <param name="mat"></param>
    /// <returns></returns>
    /// <exception cref="System.Exception"></exception>
    static double GetBeta(Mat mat)
        => mat.Depth() switch
        {
            MatDepth.U8 => 0,
            MatDepth.S8 => MatRange.MinS8,
            MatDepth.U16 => 0,
            MatDepth.S16 => MatRange.MinS16,
            MatDepth.S32 => MatRange.MinS32,
            MatDepth.F32 => MatRange.MinF32,
            MatDepth.F64 => MatRange.MinF64,
            _ => throw new System.Exception()
        };

    /// <summary>
    /// 范围归一化，当min和max置空，默认为最大最小归一化
    /// </summary>
    /// <param name="mat"></param>
    /// <param name="min"></param>
    /// <param name="max"></param>
    /// <returns></returns>
    /// <exception cref="System.Exception"></exception>
    public static Mat RangeNorm(this Mat mat, double? min = null, double? max = null)
        => min is null && max is null
            ? mat.Normalize(GetAlpha(mat), GetBeta(mat), NormTypes.MinMax)
            : min is { } dmin && max is { } dmax
                ? (((mat.Threshold(dmin, dmax, dmin, dmax) - dmin) / (dmax - dmin)) * GetAlpha(mat)).ToMat()
                : throw new System.Exception("The min and max value must not null.");

    /// <summary>
    /// 
    /// </summary>
    /// <param name="mat"></param>
    /// <returns></returns>
    public static Mat HistNorm(this Mat mat)
    {
        var result = new Mat();
        Cv2.EqualizeHist(mat, result);
        return result;
    }

    /// <summary>
    /// Set all pixels smaller than min to val, and ensure that values ​​larger than min remain unchanged
    /// </summary>
    /// <param name="mat"></param>
    /// <param name="min"></param>
    /// <param name="val">replace value</param>
    /// <returns></returns>
    public static Mat MinThreshold(this Mat mat, double min, double val = 0)
    {
        var threshold = new Mat();

        if (val == 0)
            Cv2.Threshold(mat, threshold, min, 0, ThresholdTypes.Tozero);
        else
        {
            var alpha = GetAlpha(mat);
            var mask = new Mat();
            Cv2.Threshold(mat, mask, min, alpha, ThresholdTypes.Binary);
            threshold = new Mat(mat.Size(), mat.Type(), new Scalar(val));
            mat.CopyTo(threshold, mask);
        }
        return threshold;
    }

    /// <summary>
    /// Set all pixels greater than max to val, and ensure that values ​​greater than max remain unchanged
    /// </summary>
    /// <param name="mat"></param>
    /// <param name="max"></param>
    /// <param name="val"></param>
    /// <returns></returns>
    public static Mat MaxThreshold(this Mat mat, double max, double val = 0)
    {
        var threshold = new Mat();

        if (val == 0)
            Cv2.Threshold(mat, threshold, max, 0, ThresholdTypes.TozeroInv);
        else
        {
            var alpha = GetAlpha(mat);
            var mask = new Mat();
            Cv2.Threshold(mat, mask, max, alpha, ThresholdTypes.BinaryInv);
            threshold = new Mat(mat.Size(), mat.Type(), new Scalar(val));
            mat.CopyTo(threshold, mask);
        }
        return threshold;
    }

    /// <summary>
    /// 阈值化
    /// </summary>
    /// <param name="mat"></param>
    /// <param name="min"></param>
    /// <param name="max"></param>
    /// <param name="minVal"></param>
    /// <param name="maxVal"></param>
    /// <returns></returns>
    public static Mat Threshold(this Mat mat, double min, double max, double minVal = 0, double maxVal = 1)
        => mat.MinThreshold(min, minVal).MaxThreshold(max, maxVal);
}
