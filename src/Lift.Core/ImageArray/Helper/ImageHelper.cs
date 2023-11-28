using System.ComponentModel.DataAnnotations;
using Lift.Core.ImageArray.Extensions;

namespace Lift.Core.ImageArray.Helper;

/// <summary>
/// 1. Read and Write
/// </summary>
public static class ImageHelper
{
    /// <summary>
    /// read image with opencv
    /// </summary>
    /// <returns></returns>
    public static ImageArray ReadByOpencv(string path)
    {
        if (string.IsNullOrEmpty(path)) throw new InvalidException("The Path is empty or null.");
        if (!File.Exists(path)) throw new InvalidException($"The Path: {path} not exits.");

        var ext = Path.GetExtension(path);

        return ext switch
        {
            ".tif" or ".tiff" => OpencvImageHelper.ReadTiff(path),
            ".png" => OpencvImageHelper.ReadDefault(path),
            ".bmp" => OpencvImageHelper.ReadDefault(path),
            ".jpg" => OpencvImageHelper.ReadDefault(path),
            _ => throw new NotSupportedException("Not support this file extension.")
        };

    }
}


public static class OpencvImageHelper
{
    /// <summary>
    /// 
    /// </summary>
    /// <param name="path"></param>
    /// <returns></returns>
    /// <exception cref="System.Exception"></exception>
    internal static ImageArray ReadTiff(string path)
    {
        // todo: 这里要添加一下自动判断是否属于多帧数据，不是多帧应该直接使用单帧的方式，因为有一定概率误判导致将彩色图像当多帧了
        Cv2.ImReadMulti(path, out var mats, ImreadModes.Unchanged);

        if (mats.Length == 0) throw new ValidationException("Image error.The stack is 0.");

        var array = new ImageArray(mats[0].Width, mats[0].Height, mats.Length)
        {
            Flag = ArrayFlag.OpenCv,
            Object = mats,
            Width = mats[0].Width,
            Height = mats[0].Height,
            StackCount = mats.Length,
            Metadata = ReadMetadata(path)
        };

        return array;
    }

    internal static ImageArray ReadDefault(string path)
    {
        var mat = Cv2.ImRead(path, ImreadModes.Unchanged);
        if (mat.Width == 0 || mat.Height == 0)
            throw new ValidationException("The size of image must more than 0.");

        var array = new ImageArray(mat.Width, mat.Height, 1)
        {
            Flag = ArrayFlag.OpenCv,
            Object = mat,
            Width = mat.Width,
            Height = mat.Height,
            StackCount = 0,
            Metadata = ReadMetadata(path)
        };

        return array;
    }

    internal static ImageMetadata ReadMetadata(string path)
    {
        return new ImageMetadata();
    }

    public static void Show(string path)
    {
        var array = ImageHelper.ReadByOpencv(path);

        if (array.Object is Mat[] mats)
        {
            var name = $"QuickShow {(mats.Length == 1 ? "" : $"1/{mats.Length}")}";
            Cv2.NamedWindow(name, WindowFlags.AutoSize);

            Cv2.ImShow(name, mats[0].Normalize(65535, normType: NormTypes.MinMax).ToU8());
            Cv2.WaitKey(0);

            // 关闭窗口
            Cv2.DestroyWindow(name);
        }
        else if (array.Object is Mat mat)
        {
            var name = $"QuickShow";
            Cv2.NamedWindow(name, WindowFlags.AutoSize);

            Cv2.ImShow(name, mat.Normalize(65535, normType: NormTypes.MinMax).ToU8());
            Cv2.WaitKey(0);

            // 关闭窗口
            Cv2.DestroyWindow(name);
        }
        else throw new InvalidException("The array object is not Mat.");
    }
}
