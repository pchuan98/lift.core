namespace Lift.Core.ImageArray.Extensions;

/// <summary>
/// 
/// </summary>
public static partial class MatsExtension
{
    /// <summary>
    /// 
    /// </summary>
    /// <param name="mats"></param>
    /// <param name="min"></param>
    /// <param name="max"></param>
    /// <param name="thread"></param>
    /// <exception cref="InvalidException"></exception>
    public static void MinMaxLoc(this IEnumerable<Mat> mats, out double min, out double max, int thread = 8)
    {
        var matsArray = mats.ToArray();

        min = 0;
        max = 0;

        var count = matsArray.Length;

        if (count == 0)
            throw new InvalidException("The count of mats can`t zero.");

        var lMin = new double[count];
        var lMax = new double[count];

        Parallel.For(0, count, new ParallelOptions() { MaxDegreeOfParallelism = thread }, (z) =>
        {
            var mat = matsArray[z];
            mat.MinMaxLoc(out var tMin, out double tMax);

            lMin[z] = tMin;
            lMax[z] = tMax;
        });

        min = lMin.Min();
        max = lMax.Max();
    }



    /// <summary>
    /// 从左到右，从上到下，从低到高
    /// </summary>
    /// <param name="mats"></param>
    /// <param name="array"></param>
    /// <param name="isNorm"></param>
    /// <param name="thread"></param>
    /// <exception cref="InvalidException"></exception>
    /// <exception cref="System.Exception"></exception>
    public static void ToFloatArray(this Mat[] mats, out float[] array, bool isNorm = false, int thread = 8)
    {
        if (!mats.SameShape())
            throw new InvalidException("The stack image must the same as each others that type and size.");

        var header = mats[0];

        var width = header.Width;
        var height = header.Height;
        var depth = mats.Length;

        var lenght = width * height * depth;
        var data = new float[lenght];
        var channel = header.Channels();

        if (channel != 1) throw new System.Exception();

        // todo 内存拼接(mat.Ptr())，后面再修改，先使用简单版本

        var d = typeof(byte);

        var min = double.MaxValue;
        var max = double.MinValue;

        if (isNorm)
        {
            for (var i = 0; i < depth; i++)
            {
                mats[i].MinMaxIdx(out var minval, out var maxval);
                min = minval < min ? minval : min;
                max = maxval > max ? maxval : max;
            }

            min -= 0.0000001;
            max += 0.0000001;

            var range = max - min;

            Parallel.For(0, depth, new ParallelOptions() { MaxDegreeOfParallelism = thread }, (z) =>
            {
                var mat = mats[z];
                var index = z * width * height;

                var rmat = ((mat - min) / range).ToMat();

                for (var y = 0; y < height; y++)

                    for (var x = 0; x < width; x++, ++index)
                        data[index] = rmat.At<float>(y, x);

            });

            array = new float[lenght];
            Array.Copy(data, array, width * height * depth);

            return;
        }

        Parallel.For(0, depth, new ParallelOptions() { MaxDegreeOfParallelism = thread }, (z) =>
        {
            var mat = mats[z];
            var index = z * width * height;

            for (var y = 0; y < height; y++)

                for (var x = 0; x < width; x++, ++index)
                    data[index] = mat.At<float>(y, x);

        });

        array = new float[lenght];
        Array.Copy(data, array, width * height * depth);
    }

    /// <summary>
    /// 
    /// </summary>
    /// <param name="mats"></param>
    /// <returns></returns>
    public static Mat? MaxMat(this IEnumerable<Mat> mats)
    {
        var array = mats.ToArray();

        if (!array.SameShape())
            throw new InvalidException("The stack image must the same as each others that type and size.");

        if (array.Any(item => item.Channels() != 1))
            throw new System.Exception();

        if (array.Length == 0) return null;
        if (array.Length == 1) return array[0];

        var max = array[0].Clone();

        for (var i = 1; i < array.Length; i++)
            Cv2.Max(array[i], max, max);

        return max;
    }
}
