namespace Lift.Core.ImageArray.Extensions;

/// <summary>
/// 
/// </summary>
public static partial class ImageArrayExtension
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
        if (imgs.Aggregate(true, (current, img) => current && img.IsMat()))
            return MergeTools.MergeWithMats(imgs.Select(img => img.Object as Mat).ToArray()!, cols, rows, thread);
        throw new NotSupportedException("");
    }
}

internal static class MergeTools
{
    internal static ImageArray MergeWithMats(Mat[] mats, int cols, int rows, int thread = 1)
    {
        if (mats.Any(m => m == null) || !mats!.SameShape(thread))
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
