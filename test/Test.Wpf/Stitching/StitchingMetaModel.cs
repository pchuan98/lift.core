using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using Lift.Core.Share.Common;
using OpenCvSharp;

namespace Test.Wpf.Stitching;

/// <summary>
/// 每次采集后需要提供的拼接图片元信息
///
/// NOTE: 这里不做任何关于图像切割相关的操作
/// </summary>
public class StitchingMetaModel
{
    public StitchingMetaModel(Mat src)
    {
        Src = src.Clone();
    }

    /// <summary>
    /// 原始数据对象
    /// </summary>
    public Mat Src { get; set; }

    /// <summary>
    /// 裂缝边缘是否处理，如果处理将自动使用处理算法
    ///
    /// 顺序依据Thickness的判断顺序
    /// </summary>
    public Bool4 SeamMask { get; set; } = new();

    /// <summary>
    /// 边缘处理算法需要用的参数
    /// </summary>
    public object? SeamMaskParams { get; set; }

    /// <summary>
    /// 当前采样在整体的图像中的行数
    /// </summary>
    public int Row { get; set; } = -1;

    /// <summary>
    /// 当前采样在整体的图像中的列数
    /// </summary>
    public int Column { get; set; } = -1;
}
