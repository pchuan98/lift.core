using System.Diagnostics;
using System.Security.Cryptography;
using Microsoft.VisualBasic.CompilerServices;
using Point = System.Drawing.Point;

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

    /// <summary>
    /// 并行计算的线程数
    /// </summary>
    public int ThreadCount { get; set; } = 4;
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
    private static Mat? Stitching(this IEnumerable<Mat> mats, StitchParams? param = null)
    {
        param ??= new StitchParams();

        var stitcher = Stitcher.Create(param.IsPanorama ? Stitcher.Mode.Panorama : Stitcher.Mode.Scans);
        stitcher.RegistrationResol = param.RegistrationResol;
        stitcher.SeamEstimationResol = param.SeamEstimationResol;
        //stitcher.CompositingResol = param.CompositingResol;
        stitcher.PanoConfidenceThresh = param.PanoConfidenceThresh;
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
            Debug.WriteLine(e);
        }
        catch (System.Exception e)
        {
            Debug.WriteLine(e);
        }
        finally
        {
            if (result.Width == 0 || result.Height == 0)
            {
                result = null;
                Console.WriteLine("The stitching result is null.");
            }
        }

        var sss = stitcher.EstimateTransform(new []{ Cv2.ImRead(@"F:\.test\stitch2\5-10.jpg") , Cv2.ImRead(@"F:\.test\stitch2\5-11.jpg") });

        return result;
    }

    /// <summary>
    /// 
    /// </summary>
    /// <param name="mats"></param>
    /// <param name="cols"></param>
    /// <param name="rows"></param>
    /// <param name="threshold"></param>
    /// <param name="mergeAll"></param>
    /// <param name="param"></param>
    /// <returns></returns>
    /// <exception cref="System.Exception"></exception>
    public static Mat? Stitching(this IEnumerable<Mat> mats, int cols, int rows, int threshold, bool mergeAll = false, StitchParams? param = null)
    {
        var enumerable = mats as Mat[] ?? mats.ToArray();
        if (enumerable.Count() != cols * rows) throw new System.Exception();

        param ??= new StitchParams();

        var rowCollection = new List<Mat>();
        var colCollection = new List<Mat>();

        var rowStitch = new Mat();
        var colStitch = new Mat();

        var rowTask = Task.Run(() =>
        {
            Parallel.For(0, rows, new ParallelOptions()
            {
                MaxDegreeOfParallelism = param.ThreadCount
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
                    Debug.WriteLine($"[Stitching] - [RowTask] - [{index}] -> {stitch is not null}");
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
            rowStitch ??= new Mat();
            Debug.WriteLine($"[Stitching] - [RowTask] - The row size is {rowStitch.Rows} * {rowStitch.Cols} -> {rowStitch.Rows * rowStitch.Cols}");
        });

        var colTask = Task.Run(() =>
        {
            Parallel.For(0, cols, new ParallelOptions()
            {
                MaxDegreeOfParallelism = param.ThreadCount
            }, index =>
            {
                var temp = new List<Mat>();

                for (var i = 0; i < rows; i++)
                    temp.Add(enumerable[index + cols * i]);

                var stitch = temp.Where(item => item.Max() > threshold).Stitching(param);

                if (stitch != null) colCollection.Add(stitch);

                Debug.WriteLine($"[Stitching] - [ColTask] - [{index}] -> {stitch is not null}");
            });

            Debug.WriteLine($"[Stitching] - [ColTask] - Start stitiching the col images...");
            colStitch = colCollection.Stitching(param);
            colStitch ??= new Mat();
            Debug.WriteLine($"[Stitching] - [ColTask] - The row size is {colStitch.Rows} * {colStitch.Cols} -> {colStitch.Rows * colStitch.Cols}");

        });

        rowTask.Wait();
        colTask.Wait();

        Debug.WriteLine($"[Stitching] - Start stitiching all of the images...");
        var mergeStitch = mergeAll ? new Mat[]
        {
            rowStitch,
            colStitch
        }.Stitching(param) : new Mat();

        mergeStitch ??= new Mat();
        Debug.WriteLine($"[Stitching] - [MergeTask] - The row size is {mergeStitch.Rows} * {mergeStitch.Cols} -> {mergeStitch.Rows * mergeStitch.Cols}");


        var sizeRow = rowStitch.Rows * rowStitch.Cols;
        var sizeCol = colStitch.Rows * colStitch.Cols;
        var sizeMerge = mergeStitch.Rows * mergeStitch.Cols;

        if (sizeRow >= sizeCol && sizeRow >= sizeMerge)
        {
            Debug.WriteLine("The result is row stitch.");
            return rowStitch;
        }
        else if (sizeCol >= sizeRow && sizeCol >= sizeMerge)
        {
            Debug.WriteLine("The result is col stitch.");
            return colStitch;
        }
        else
        {
            Debug.WriteLine("The result is merge stitch.");
            return mergeStitch;
        }
    }
}

/// <summary>
/// 提供拼接
/// </summary>
public interface IStitcherProvider
{
    /// <summary>
    /// 捕获某一帧图像
    /// </summary>
    /// <returns>
    /// mat 某一帧图像，要求为8U类型
    /// x   采集图像的实际坐标
    /// y   采集图像的实际坐标
    /// 
    /// (x,y) 实际为中心点坐标
    ///
    /// row 当前采集的行数
    /// col 当前采集的列数
    /// </returns>
    public (Mat mat, double x, double y, int row, int col) Provide();
}

/// <summary>
/// 拼接器
/// </summary>
public interface IStitcher
{
    /// <summary>
    /// 采集设备的物理单位
    /// </summary>
    public string Unit { get; set; }

    /// <summary>
    /// 最终图像左上角的实际坐标
    /// </summary>
    public (double x, double y) LeftTop { get; set; }

    /// <summary>
    /// 最终图像右下角的实际坐标
    /// </summary>
    public (double x, double y) RightBottom { get; set; }

    /// <summary>
    /// 理论最终图像的宽度
    /// </summary>
    public int Width { get; set; }

    /// <summary>
    /// 理论最终图像的高度
    /// </summary>
    public int Height { get; set; }

    /// <summary>
    /// 拼接需要的列数
    /// </summary>
    public int Cols { get; set; }

    /// <summary>
    /// 拼接需要的行数
    /// </summary>
    public int Rows { get; set; }

    /// <summary>
    /// 重叠部分的像素数
    /// </summary>
    public int Overlap { get; set; }

    /// <summary>
    /// 每个像素对应的理论实际物理距离
    /// </summary>
    public double PerPixelDistance { get; set; }

    /// <summary>
    /// 图像数据提供者
    /// </summary>
    public IStitcherProvider? Provider { get; set; }

    /// <summary>
    /// 某次有新数据后运行
    /// </summary>
    /// <returns>
    /// 返回值不影响最终拼接
    ///
    /// true  - 拼接算法拼接成功
    /// false - 拼接算法拼接失败，使用直接拼接的方法
    /// </returns>
    public bool Step();

    /// <summary>
    /// 拼接结果
    /// </summary>
    public Mat StitchMat { get; set; }
}

/// <summary>
/// 逐帧拼接
/// </summary>
public class ScanStitcher : IStitcher
{
    /// <inheritdoc/>
    public string Unit { get; set; } = "um";

    /// <inheritdoc/>
    public (double x, double y) LeftTop { get; set; }

    /// <inheritdoc/>
    public (double x, double y) RightBottom { get; set; }

    /// <inheritdoc/>
    public int Width { get; set; } = 0;

    /// <inheritdoc/>
    public int Height { get; set; } = 0;

    /// <inheritdoc/>
    public int Cols { get; set; }

    /// <inheritdoc/>
    public int Rows { get; set; }

    /// <inheritdoc/>
    public int Overlap { get; set; }

    /// <inheritdoc/>
    public double PerPixelDistance { get; set; } = 0;

    /// <inheritdoc/>
    public IStitcherProvider? Provider { get; set; }

    /// <inheritdoc/>
    public Mat StitchMat { get; set; } = new Mat();

    /// <inheritdoc/>
    public bool Step()
    {
        if (Provider == null) throw new System.Exception();

        // set background panel
        if (StitchMat.Width == 0 || StitchMat.Height == 0)
            StitchMat = new Mat(new Size(Width, Height), MatType.CV_8UC1, new Scalar(0));

        var (mat, x, y, r, c) = Provider.Provide();

        if (r == 0 && c == 0)
            mat.CopyTo(StitchMat[new Rect(0, 0, mat.Width, mat.Height)]);


        var enableStitch = false;

        if (!enableStitch)
        {

        }
        else
        {

        }

        return false;
    }
}
