﻿namespace Lift.Core.ImageArray.Extensions;

public static partial class MatExtension
{
    /// <summary>
    /// Converter to CV_8U,使用最大最小归一化来保证整体精度损失最小
    /// </summary>
    /// <param name="mat"></param>
    /// <param name="toGray"></param>
    /// <returns></returns>
    /// <exception cref="NotSupportedException"></exception>
    public static Mat ToU8(this Mat mat, bool toGray = false)
    {
        var rec = new Mat();
        var type = mat.Type();

        if (type == MatType.CV_8UC1)
            return mat;
        if (type == MatType.CV_32FC1)
            mat.ConvertTo(rec, MatType.CV_8U, byte.MaxValue);
        else if (type == MatType.CV_16UC1)
            mat.ConvertTo(rec, MatType.CV_8U, 1.0 / (byte.MaxValue + 2));
        else if (type == MatType.CV_64FC1)
            rec = MatConverter.Channel1.F64ToU8(mat);
        else throw new NotSupportedException($"Not support type : {type}");

        return rec;
    }

    /// <summary>
    /// 
    /// </summary>
    /// <param name="mat"></param>
    /// <returns></returns>
    /// <exception cref="System.Exception"></exception>
    public static Mat ToF32(this Mat mat)
    {
        var tempType = mat.Type();
        var tempInt = (int) mat.Type();
        return (int) mat.Type() switch
        {
            (int) MatTypeAsInt.CV_8UC1 => MatConverter.Channel1.U8ToF32(mat),
            (int) MatTypeAsInt.CV_8UC3 => MatConverter.Channel3.U8ToF32(mat),
            (int) MatTypeAsInt.CV_16UC1 => MatConverter.Channel1.U16ToF32(mat),
            (int) MatTypeAsInt.CV_32FC1 => mat,
            _ => throw new System.Exception(),
        };
    }
}

/// <summary>
/// 转换集成工具
/// </summary>
internal static class MatConverter
{
    /// <summary>
    /// 单通道转换情况
    /// </summary>
    public static class Channel1
    {
        public static Mat U8ToF32(Mat mat)
        {
            var rec = new Mat();
            mat.ConvertTo(rec, MatType.CV_32FC1, 1.0f / byte.MaxValue);

            return rec;
        }

        public static Mat U16ToF32(Mat mat)
        {
            var rec = new Mat();
            mat.ConvertTo(rec, MatType.CV_32FC1, 1.0f / ushort.MaxValue);

            return rec;
        }

        public static Mat F32ToU8(Mat mat)
        {
            var rec = new Mat();
            mat.ConvertTo(rec, MatType.CV_8UC1, byte.MaxValue);

            return rec;
        }

        public static Mat F64ToU8(Mat mat)
        {
            var rec = new Mat();
            mat.Normalize(normType: NormTypes.MinMax).ConvertTo(rec, MatType.CV_8UC1, byte.MaxValue);

            rec.GetArray(out byte[] v);

            return rec;
        }
    }

    /// <summary>
    /// 三通道转换情况
    /// </summary>
    public static class Channel3
    {
        public static Mat U8ToF32(Mat mat)
        {
            var gray = new Mat();
            // to gray
            Cv2.CvtColor(mat, gray, ColorConversionCodes.BGR2GRAY);

            var rec = new Mat();
            gray.ConvertTo(rec, MatType.CV_32FC1, 1.0f / byte.MaxValue);

            return rec;
        }
    }
}
