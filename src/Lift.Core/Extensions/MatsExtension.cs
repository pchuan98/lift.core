using Lift.Core.Common;
using Lift.Core.Exception;


namespace Lift.Core.Extensions;

public static class MatsExtension
{
    public const double DoubleThreshold = 0.00001;

    #region Type Converter

    /// <summary>
    /// 
    /// </summary>
    /// <param name="mats"></param>
    /// <returns></returns>
    public static Mat[] ToF32(this Mat[] mats)
    {
        var ms = new Mat[mats.Length];

        for (var i = 0; i < mats.Length; i++)
            ms[i] = mats[i].ToF32();

        ms[0].GetArray(out float[] aData);
        return ms;
    }

    #endregion


    public static bool SameShape(this Mat[] mats, int thread = 8)
    {
        // todo:后面可以使用多线程+二分法快速完成整个过程

        var isSame = false;
        var count = mats.Length;
        if (count == 0)
            throw new InvalidException("The count of mats can`t zero.");

        var tp = mats[0].Type();
        var size = mats[0].Size();

        var lSame = new bool[count];
        lSame[0] = true;

        Parallel.For(1, count, new ParallelOptions() { MaxDegreeOfParallelism = thread }, (z) =>
        {
            lSame[z] = mats[z].Type() == tp && mats[z].Size() == size;
        });

        isSame = lSame.All(x => x);
        return isSame;
    }

    /// <summary>
    /// 
    /// </summary>
    /// <param name="mats"></param>
    /// <param name="min"></param>
    /// <param name="max"></param>
    /// <param name="thread"></param>
    /// <exception cref="InvalidException"></exception>
    public static void MinMaxLoc(this Mat[] mats, out double min, out double max, int thread = 8)
    {
        min = 0;
        max = 0;

        var count = mats.Length;

        if (count == 0)
            throw new InvalidException("The count of mats can`t zero.");

        var lMin = new double[count];
        var lMax = new double[count];

        Parallel.For(0, count, new ParallelOptions() { MaxDegreeOfParallelism = thread }, (z) =>
        {
            var mat = mats[z];
            mat.MinMaxLoc(out double tMin, out double tMax);

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
                mats[i].MinMaxIdx(out double minval, out double maxval);
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
            System.Array.Copy(data, array, width * height * depth);

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
        System.Array.Copy(data, array, width * height * depth);
    }

    /// <summary>
    /// 
    /// </summary>
    /// <param name="mats"></param>
    /// <param name="scale">等比例放缩</param>
    /// <param name="thread"></param>
    /// <returns></returns>
    public static Mat[] Resize(this Mat[] mats, Vector scale, int thread = 8)
    {
        if (!mats.SameShape())
            throw new InvalidException("The stack image must the same as each others that type and size.");

        if (scale.X == 0 || scale.Y == 0 || scale.Z == 0)
            throw new InvalidException("The resize scale cant be set 0.");

        if (scale.X > 1 || scale.Y > 1 || scale.Z > 1)
            throw new InvalidException("The scale must in 0-1");

        if (Math.Abs(scale.X - 1) < DoubleThreshold &&
            Math.Abs(scale.Y - 1) < DoubleThreshold &&
            Math.Abs(scale.Z - 1) < DoubleThreshold) return mats;

        var x = (int) Math.Ceiling(mats[0].Width * scale.X);
        var y = (int) Math.Ceiling(mats[0].Height * scale.Y);
        var z = (int) Math.Ceiling(mats.Length * scale.Z);

        var newMats = new Mat[z];

        var zList = new int[z];
        for (var i = 0; i < z; i++)
            zList[i] = (int) Math.Floor(i * (1 / scale.Z));

        Parallel.For(0, z, new ParallelOptions() { MaxDegreeOfParallelism = thread }, (i) =>
        {
            var index = zList[i];
            var dst = new Mat();
            var src = mats[index];

            Cv2.Resize(src, dst, new Size(x, y));
            newMats[i] = dst;

        });


        return newMats;
    }


}
