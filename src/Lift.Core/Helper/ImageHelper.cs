using Lift.Core.Common;
using Lift.Core.Constant;
using Lift.Core.Exception;
using Lift.Core.Extensions;
using OpenCvSharp;
using Sharpen;
using Array = Lift.Core.Common.Array;

namespace Lift.Core.Helper;

public static class ImageHelper
{
    /// <summary>
    /// 读取图像元数据
    /// </summary>
    /// <param name="path"></param>
    /// <returns></returns>
    public static ImageMetadata ReadMetadata(string path)
    {
        return new ImageMetadata();
    }

    /// <summary>
    /// 读取图像
    /// </summary>
    /// <param name="path"></param>
    /// <returns></returns>
    /// <exception cref="InvalidException"></exception>
    /// <exception cref="System.Exception"></exception>
    public static Array Read(string path)
    {
        if (string.IsNullOrEmpty(path)) throw new InvalidException("The Path is empty or null.");
        if (!File.Exists(path)) throw new InvalidException($"The Path: {path} not exits.");

        var ext = Path.GetExtension(path);

        return ext switch
        {
            ".tif" or ".tiff" => ReadTiff(path),
            ".png" => ReadDefault(path),
            _ => throw new InvalidException("Not support this file extension.")
        };
    }

    public static void QuickShow(string path)
    {
        var array = Read(path);

        if (array.Object is not Mat[] mats) return;

        var name = $"QuickShow {(mats.Length == 1 ? "" : $"1/{mats.Length}")}";
        Cv2.NamedWindow(name, WindowFlags.AutoSize);

        Cv2.ImShow(name, mats[0].Normalize(65535, normType: NormTypes.MinMax).ToU8());
        Cv2.WaitKey(0);

        // 关闭窗口
        Cv2.DestroyWindow(name);
    }

    #region File


    private static Array ReadTiff(string path)
    {
        Cv2.ImReadMulti(path, out var mats, ImreadModes.Unchanged);

        if (mats.Length == 0) throw new System.Exception("Image error.The stack is 0.");

        var array = new Array(mats[0].Width, mats[0].Height, mats.Length)
        {
            Flag = ArrayFlag.OpenCv,
            Object = mats.Length == 1 ? mats[0] : mats,
            Metadata = ReadMetadata(path)
        };

        return array;
    }

    private static Array ReadDefault(string path)
    {
        var mat = Cv2.ImRead(path, ImreadModes.Unchanged);
        if (mat.Width == 0 || mat.Height == 0)
            throw new System.Exception("The size of image must more than 0.");

        var array = new Array(mat.Width, mat.Height, 1)
        {
            Flag = ArrayFlag.OpenCv,
            Object = mat,
            Metadata = ReadMetadata(path)
        };

        return array;
    }
    #endregion
}
