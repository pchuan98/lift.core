using System.Diagnostics;
using System.Runtime.CompilerServices;
using Lift.Core.Share.Extensions;
using OpenCvSharp;
using OpenCvSharp.Detail;
using Range = System.Range;

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

    /// <summary>
    /// 是否进行球形矫正
    /// </summary>
    public bool IsPanorama { get; set; } = false;

    /// <summary>
    /// 0 - 1,图像的缩放率
    /// </summary>
    public double RegistrationResol { get; set; } = 1;

    /// <summary>
    /// 结合处的缝痕计算因子
    /// </summary>
    public double SeamEstimationResol { get; set; } = 0.1;

    /// <summary>
    /// 曝光设置
    /// </summary>
    public double CompositingResol { get; set; } = -1;

    /// <summary>
    /// 0 - 1,置信度
    /// </summary>
    public double PanoConfidenceThresh { get; set; } = 1;

    /// <summary>
    /// 是否进行波形矫正
    /// </summary>
    public bool WaveCorrection { get; set; } = true;

    /// <summary>
    /// 是否水平波形矫正
    /// </summary>
    public bool HorizontalWaveCorrectKind { get; set; } = true;
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
        param ??= new StitchParams();

        var stitcher = Stitcher.Create(param.IsPanorama ? Stitcher.Mode.Panorama : Stitcher.Mode.Scans);

        stitcher.RegistrationResol = param.RegistrationResol;
        //stitcher.SeamEstimationResol = param.SeamEstimationResol;
        //stitcher.CompositingResol = param.CompositingResol;
        //stitcher.PanoConfidenceThresh = param.PanoConfidenceThresh;
        //stitcher.WaveCorrection = param.WaveCorrection;
        //stitcher.WaveCorrectKind = param.HorizontalWaveCorrectKind
        //    ? WaveCorrectKind.Horizontal
        //    : WaveCorrectKind.Vertical;

        var enumerable = mats as Mat[] ?? mats.ToArray();

        if (enumerable.Length == 0) return null;
        if (enumerable.Length == 1) return enumerable[0];

        var result = new Mat();
        try
        {

            var recall = stitcher.Stitch(enumerable, result) == Stitcher.Status.OK;
            if (!recall) return null;
        }
        catch (OpenCvSharpException e)
        {
            //Console.WriteLine(e);
        }
        

        return result;
    }

    /// <summary>
    /// 
    /// </summary>
    /// <param name="mats"></param>
    /// <param name="cols"></param>
    /// <param name="rows"></param>
    /// <param name="threshold"></param>
    /// <param name="priority"></param>
    /// <param name="param"></param>
    /// <returns></returns>
    /// <exception cref="System.Exception"></exception>
    public static Mat? Stitching(this IEnumerable<Mat> mats, int cols, int rows, int threshold, int priority = -1, StitchParams? param = null)
    {
        var enumerable = mats as Mat[] ?? mats.ToArray();
        if (enumerable.Count() != cols * rows) throw new System.Exception();

        var stitcher = Stitcher.Create(Stitcher.Mode.Scans);

        var rowCollection = new List<Mat>();
        var colCollection = new List<Mat>();

        var rowStitch = new Mat();
        var colStitch = new Mat();

        if (priority == 1)
        {

        }
        else if (priority == 2)
        {

        }

        var rowTask = Task.Run(() =>
        {
            Parallel.For(0, rows, new ParallelOptions()
            {
                MaxDegreeOfParallelism = 1
            }, index =>
            {
                try
                {
                    var stitch = enumerable
                        .Skip(index * cols)
                        .Take(cols)
                        .Where(item => item.Max() > threshold)
                        .Stitching(param);
                    if (stitch != null) rowCollection.Add(stitch);
                    Debug.WriteLine($"[Stitching] - [RowTask] - [{index}] -> {stitch is null}");
                }
                catch (OpenCVException e)
                {
                    Console.WriteLine(e.ToString());
                }
                catch (NullReferenceException e)
                {
                    Console.WriteLine(e.ToString());
                }
            });

            Debug.WriteLine($"[Stitching] - [RowTask] - Start stitiching the row images...");
            rowStitch = rowCollection.Stitching(param);
        });

        var colTask = Task.Run(() =>
        {
            Parallel.For(0, cols, new ParallelOptions()
            {
                MaxDegreeOfParallelism = 1
            }, index =>
            {
                var temp = new List<Mat>();

                for (var i = 0; i < rows; i++)
                    temp.Add(enumerable[index + cols * i]);

                var stitch = temp.Where(item => item.Max() > threshold).Stitching(param);

                if (stitch != null) colCollection.Add(stitch);

                Debug.WriteLine($"[Stitching] - [ColTask] - [{index}] -> {stitch is null}");
            });

            Debug.WriteLine($"[Stitching] - [ColTask] - Start stitiching the col images...");
            colStitch = colCollection.Stitching(param);
        });

        rowTask.Wait();
        colTask.Wait();

        rowStitch.SaveImage($@"E:\.test\result6\00.jpg");
        colStitch.SaveImage($@"E:\.test\result6\01.jpg");

        Debug.WriteLine($"[Stitching] - Start stitiching all of the images...");


        return new Mat[]
        {
            rowStitch,
            colStitch
        }.Stitching(param);
    }
}
