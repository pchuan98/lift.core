using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Lift.Core.ImageArray.Extensions;

// hack: I feel like there may be something wrong with S32. Is there an upper and lower limit for int?

public static partial class MatExtension
{
    /// <summary>
    /// 
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
    /// 
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
    /// 最大最小归一化
    /// </summary>
    /// <param name="mat"></param>
    /// <returns></returns>
    public static Mat MinMaxNorm(this Mat mat)
    {
        var alpha = GetAlpha(mat);
        var beta = GetBeta(mat);

        var recall = mat.Normalize(alpha, beta, NormTypes.MinMax);

        return recall;
    }
}
