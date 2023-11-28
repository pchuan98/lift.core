namespace Lift.Core.ImageArray.Algorithm;

/// <summary>
/// 
/// </summary>
public static partial class Algorithm
{
    /// <summary>
    /// The PSNR value obtained by comparing the source (src) to the destination (dst).
    /// </summary>
    /// <param name="src"></param>
    /// <param name="dst"></param>
    /// <param name="r"></param>
    /// <returns></returns>
    /// <exception cref="InvalidException"></exception>
    public static double PSNR(this ImageArray src, ImageArray dst, double r = 255)
    {
        var imgSrc = src.ToMat()?.ToF32();
        var imgDst = dst.ToMat()?.ToF32();

        if (imgSrc == null || imgDst == null)
            throw new InvalidException("The image must not be empty.");

        if (imgSrc.Size() != imgDst.Size())
            throw new InvalidException("The input images must have the same dimensions.");

        return Cv2.PSNR(imgSrc, imgDst, r);
    }
}
