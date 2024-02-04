namespace Lift.Core.ImageArray.Extensions;

public static partial class MatExtension
{
    /// <summary>
    /// 
    /// </summary>
    /// <param name="mat"></param>
    /// <returns></returns>
    public static double Min(this Mat mat)
    {
        mat.MinMaxLoc(out double min, out double max);

        return min;
    }

    /// <summary>
    /// 
    /// </summary>
    /// <param name="mat"></param>
    /// <returns></returns>
    public static double Max(this Mat mat)
    {
        mat.MinMaxLoc(out double min, out double max);
        return max;
    }

    /// <summary>
    /// 范围外值为0
    /// </summary>
    /// <param name="mat"></param>
    /// <param name="min"></param>
    /// <param name="max"></param>
    /// <returns></returns>
    public static Mat RangeIn(this Mat mat, double min, double max)
    {
        var mask = new Mat();
        Cv2.InRange(mat, new Scalar(min, min, min), new Scalar(max, max, max), mask);

        var result = new Mat();
        Cv2.BitwiseAnd(mat, mat, result, mask);

        return result;
    }
}
