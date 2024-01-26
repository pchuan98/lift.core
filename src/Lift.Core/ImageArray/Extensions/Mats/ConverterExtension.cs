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
    /// <returns></returns>
    public static Mat[] ToF32(this Mat[] mats)
    {
        var ms = new Mat[mats.Length];

        for (var i = 0; i < mats.Length; i++)
            ms[i] = mats[i].ToF32();

        return ms;
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

        if (Math.Abs(scale.X - 1) < DoubleBox.Threshold &&
            Math.Abs(scale.Y - 1) < DoubleBox.Threshold &&
            Math.Abs(scale.Z - 1) < DoubleBox.Threshold) return mats;

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
