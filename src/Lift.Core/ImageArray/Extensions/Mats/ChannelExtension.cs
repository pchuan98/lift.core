namespace Lift.Core.ImageArray.Extensions;

/// <summary>
/// 
/// </summary>
public static partial class MatsExtension
{
    /// <summary>
    /// 按照最大值来合并所有的图像
    /// </summary>
    /// <param name="mats"></param>
    /// <returns></returns>
    public static Mat MergeChannelAsMax(this IEnumerable<Mat> mats)
    {
        var array = mats.ToArray();

        if (array.Any(item => item.Channels() != 3))
            throw new System.Exception("All mat types must be three-channel.");

        if (!(array.SameShape() && array.SameType()))
            throw new System.Exception("All merged data must have the same size.");


        var ch1 = new List<Mat>();
        var ch2 = new List<Mat>();
        var ch3 = new List<Mat>();

        foreach (var img in array)
        {
            var channels = img.Split();
            ch1.Add(channels[0]);
            ch2.Add(channels[1]);
            ch3.Add(channels[2]);
        }

        var chMax1 = ch1.MaxMat()!;
        var chMax2 = ch2.MaxMat()!;
        var chMax3 = ch3.MaxMat()!;

        var merge = new Mat();
        Cv2.Merge(new[] { chMax1, chMax2, chMax3 }, merge);

        return merge;
    }
}
