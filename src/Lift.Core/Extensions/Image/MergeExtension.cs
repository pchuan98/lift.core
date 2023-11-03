using System.Diagnostics;
using Lift.Core.Exception;

namespace Lift.Core.Extensions.Image;

/// <summary>
/// 平面拼接和通道合并
/// </summary>
public static class MergeExtension
{
    /// <summary>
    /// 
    /// </summary>
    /// <param name="imgs"></param>
    /// <param name="cols"></param>
    /// <param name="rows"></param>
    /// <param name="thread"></param>
    /// <returns></returns>
    /// <exception cref="InvalidException"></exception>
    /// <exception cref="NotImplementedException"></exception>
    public static ImageArray MergeArray(this IEnumerable<ImageArray> imgs, int cols, int rows, int thread = 1)
    {
        var arrayList = imgs as ImageArray[] ?? imgs.ToArray();

        if (arrayList.Length != cols * rows)
            throw new InvalidException("The len of array list must equal as cols*rows");

        var isImg = arrayList.Aggregate(true, (current, img) => current & img.IsImage());
        if (!isImg) throw new InvalidException("Current input imgs is not image array.");

        var isMat = arrayList.Aggregate(true, (current, img) => current & img.IsMat());
        if (!isMat) throw new InvalidException("The current data structure is not supported yet");

        var mats = arrayList!.Select(w => w.ToMat()).ToArray();

        if (mats.Any(m => m == null) || !mats!.SameShape())
            throw new InvalidException("Currently only supports same shape matching.");

        var lines = new Mat[rows];

        Parallel.For(0, rows, new ParallelOptions()
        {
            MaxDegreeOfParallelism = thread,
        }, (z) =>
        {
            var rowMats = mats!.Skip(z * rows).Take(cols).ToArray();

            var result = rowMats.Aggregate((current, next) =>
            {
                var concat = new Mat();
                Cv2.HConcat(current!, next!, concat);
                return concat;
            });

            lines[z] = result!;
        });

        return new ImageArray(lines.Aggregate((current, next) =>
        {
            var concat = new Mat();
            Cv2.VConcat(current!, next!, concat);
            return concat;
        }));
    }
}
