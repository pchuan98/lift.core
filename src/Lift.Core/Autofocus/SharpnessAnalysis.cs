namespace Lift.Core.Autofocus;

/// <summary>
/// 
/// </summary>
public static class SharpnessAnalysis
{
    /// <summary>
    /// 核函数转换
    ///
    /// 行优先
    /// </summary>
    /// <returns></returns>
    public static Mat ToKernel(this float[] kernel)
    {
        var size = (int) Math.Sqrt(kernel.Length);

        var mat = new Mat(size, size, MatType.CV_32FC1);

        for (var i = 0; i < size; i++)
            for (var j = 0; j < size; j++)
                mat.Set<float>(i, j, kernel[i * size + j]);


        return mat;
    }

    /// <summary>
    /// 获取值 行优先
    /// </summary>
    /// <param name="mat"></param>
    /// <param name="i0"></param>
    /// <param name="i1"></param>
    /// <returns></returns>
    public static double GetValue(this Mat mat, int row, int col)
    {
        if (mat.Channels() != 1) throw new System.Exception();

        return mat.Depth() switch
        {
            MatDepth.U8 => (double) mat.Get<byte>(row, col),
            MatDepth.S8 => (double) mat.Get<sbyte>(row, col),
            MatDepth.U16 => (double) mat.Get<ushort>(row, col),
            MatDepth.S16 => (double) mat.Get<short>(row, col),
            MatDepth.S32 => (double) mat.Get<int>(row, col),
            MatDepth.F32 => (double) mat.Get<float>(row, col),
            MatDepth.F64 => mat.Get<double>(row, col),
            _ => throw new System.Exception()
        };
    }

    /// <summary>
    /// 设置值
    /// </summary>
    /// <param name="mat"></param>
    /// <param name="row"></param>
    /// <param name="col"></param>
    /// <param name="value"></param>
    public static void SetValue(this Mat mat, int row, int col, double value)
    {
        if(mat.Channels() != 1) throw new System.Exception();

        switch (mat.Depth())
        {
            case MatDepth.U8:
                mat.Set<byte>(row, col, (byte) value);
                break;
            case MatDepth.S8:
                mat.Set<sbyte>(row, col, (sbyte) value);
                break;
            case MatDepth.U16:
                mat.Set<ushort>(row, col, (ushort) value);
                break;
            case MatDepth.S16:
                mat.Set<short>(row, col, (short) value);
                break;
            case MatDepth.S32:
                mat.Set<int>(row, col, (int) value);
                break;
            case MatDepth.F32:
                mat.Set<float>(row, col, (float) value);
                break;
            case MatDepth.F64:
                mat.Set<double>(row, col, value);
                break;
            default:
                throw new System.Exception();
        }
    }

    /// <summary>
    /// <para>
    /// https://github.com/micro-manager/micro-manager/blob/9d673cb096702f0cac71ab9a74ad3f95663383a2/libraries/ImageProcessing/src/main/java/org/micromanager/imageprocessing/ImgSharpnessAnalysis.java#L265
    /// </para>
    /// 
    /// Modified version of the algorithm used by the AutoFocus JAF code in Micro-Manager's
    /// Autofocus.java by Pakpoom Subsoontorn &amp; Hernan Garcia. Looks for diagonal edges in both
    /// directions, then combines them (RMS). (Original algorithm only looked for edges in one
    /// diagonal direction). Similar to Edges algorithm except it does no normalization by original
    /// intensity and adds a median filter before edge detection.
    /// </summary>
    /// <param name="mat"></param>
    /// <returns></returns>
    public static double ComputeMedianEdges(this Mat mat)
    {
        var height = mat.Height;
        var width = mat.Width;

        var sum = 0.0;

        var ken1 = new float[] { 2, 1, 0, 1, 0, -1, 0, -1, -2 }.ToKernel();
        var ken2 = new float[] { 0, 1, 2, -1, 0, 1, -2, -1, 0 }.ToKernel();

        var mean = mat.MedianBlur(3);

        var con1 = new Mat();
        Cv2.Filter2D(mat, con1, -1, ken1);
        var con2 = new Mat();
        Cv2.Filter2D(mean, con2, -1, ken2);

        for (var i = 0; i < height; i++)
            for (var j = 0; j < width; j++)
                sum += Math.Sqrt(Math.Pow(con1.GetValue(i, j), 2)
                                 + Math.Pow(con2.GetValue(i, j), 2));

        return sum;
    }
}
