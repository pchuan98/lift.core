namespace Lift.Core.ImageArray.Extensions;

public static partial class ImageArrayExtension
{
    /// <summary>
    /// 
    /// </summary>
    /// <param name="array"></param>
    /// <returns></returns>
    public static Mat? ToMat(this ImageArray array)
    {
        return array.Flag switch
        {
            ArrayFlag.OpenCv => array?.Object as Mat,
            ArrayFlag.Determinant => (array?.Object as double[,])!.ToMat(),
            _ => throw new InvalidException("The type of ImageArray is not Mat!")
        };
    }



    /// <summary>
    /// Resize the array.
    /// <code>
    /// The mode is:
    /// 
    /// 0 - Nearest
    /// 1 - Linear
    /// 2 - Cubic
    /// 3 - Area
    /// 4 - Lanczos4
    /// 5 - LinearExact
    /// 7 - Max
    /// 8 - WarpFillOutliers
    /// </code>
    /// </summary>
    /// <param name="array"></param>
    /// <param name="width"></param>
    /// <param name="height"></param>
    /// <param name="mode"></param>
    /// <returns></returns>
    public static ImageArray ReSize(this ImageArray array, int width, int height, int mode = 1)
    {
        var mat = array.ToMat();

        mat = mat?.Resize(new Size(width, height), 0, 0, (InterpolationFlags) mode);

        if (mat is null)
            throw new InvalidException();

        // todo 以后这里是需要还原数据类型的
        return new ImageArray(mat);
    }




    #region AnyTypeToMat

    /// <summary>
    /// 
    /// </summary>
    /// <param name="array"></param>
    /// <returns></returns>
    public static Mat ToMat(this double[,] array)
    {
        var rows = array.GetLength(0);
        var cols = array.GetLength(1);

        var mat = new Mat(rows, cols, MatType.CV_64FC1);

        for (var i = 0; i < rows; i++)
        {
            for (var j = 0; j < cols; j++)
            {
                mat.Set(i, j, array[i, j]);
            }
        }

        return mat;
    }

    #endregion
}
