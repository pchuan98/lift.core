namespace Lift.Core.ImageArray.Extensions;

public static partial class ImageArrayExtension
{
    /// <summary>
    /// 
    /// </summary>
    /// <param name="array"></param>
    /// <returns></returns>
    public static Mat? ToMat(this ImageArray array)
        => array.IsMat() ? array?.Object as Mat : throw new InvalidException("The type of ImageArray is not Mat!");
}
