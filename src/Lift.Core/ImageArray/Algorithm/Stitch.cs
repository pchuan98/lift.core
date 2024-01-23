using OpenCvSharp.Detail;

namespace Lift.Core.ImageArray.Algorithm;

public class StitchParams
{
    public StitchParams()
    {
        // 默认参数
        // 1. 透视变换
        // 2. 重叠区域为 15
        // 3. 估计旋转
        // 4. 估计相机参数
        // 5. 估计尺度
        // 6. 估计曝光
        // 7. 估计白平衡
        // 8. 估计相机响应
        // 9. 估计曝光
    }


}

/// <summary>
/// 
/// </summary>
public static partial class Algorithm
{
    /// <summary>
    /// 从左到右拼接图像
    /// </summary>
    /// <param name="mats"></param>
    /// <param name="param"></param>
    /// <returns></returns>
    public static Mat? Stitching(this IEnumerable<Mat> mats, StitchParams? param = null)
    {
        var stitcher = Stitcher.Create();
        stitcher.WaveCorrectKind = WaveCorrectKind.Horizontal;

        var result1 = new Mat();
        var status = stitcher.Stitch(mats, result1);

        return result1;
    }
}
