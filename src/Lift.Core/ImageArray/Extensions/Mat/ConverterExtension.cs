﻿using System.Reflection.PortableExecutable;
using System.Runtime.InteropServices;

namespace Lift.Core.ImageArray.Extensions;

public static partial class MatExtension
{
    /// <summary>
    /// Converter to CV_8U,使用最大最小归一化来保证整体精度损失最小
    /// </summary>
    /// <param name="mat"></param>
    /// <returns></returns>
    /// <exception cref="NotSupportedException"></exception>
    public static Mat ToU8(this Mat mat)
    {
        var rec = new Mat();
        var type = mat.Type();

        switch (type.Depth)
        {
            case MatDepth.U8:
                return mat;
            case MatDepth.S8:
                (mat + MatRange.MaxS8)
                    .ToMat()
                    .ConvertTo(rec, MatType.CV_8U, (double) MatRange.MaxU8 / MatRange.MaxU16);
                break;
            case MatDepth.U16:
                mat.ConvertTo(rec, MatType.CV_8U, (double) MatRange.MaxU8 / MatRange.MaxU16);
                break;
            case MatDepth.S16:
                (mat + MatRange.MaxS16)
                    .ToMat()
                    .ConvertTo(rec, MatType.CV_8U, (double) MatRange.MaxU8 / MatRange.MaxU16);
                break;
            case MatDepth.S32:
                mat.MinMaxLoc(out double min, out double max);
                mat.ConvertTo(rec, MatType.CV_8U, 255.0 / max);
                break;
            case MatDepth.F32:
                mat.ConvertTo(rec, MatType.CV_8U, MatRange.MaxU8);
                break;
            case MatDepth.F64:
                mat.ConvertTo(rec, MatType.CV_8U, MatRange.MaxU8);
                break;
            default:
                break;
        }

        return rec;
    }

    public static Mat ToU8C3(this Mat mat)
    {
        var u8 = mat.ToU8();
        Cv2.CvtColor(u8, u8, ColorConversionCodes.GRAY2BGR);
        return u8;
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

    /// <summary>
    /// 读取Raw格式数据
    /// </summary>
    /// <param name="data"></param>
    /// <param name="t"></param>
    /// <param name="rows"></param>
    /// <param name="cols"></param>
    /// <returns></returns>
    public static Mat FromRaw(this byte[] data, MatType t, int rows, int cols)
    {
        var img = new Mat(new Size(cols, rows), t);
        var handle = GCHandle.Alloc(data, GCHandleType.Pinned);

        Array.Copy(data, data, data.Length);

        Marshal.Copy(data, 0, img.Data, data.Length);
        handle.Free();

        return img;
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
