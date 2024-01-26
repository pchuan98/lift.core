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
}
