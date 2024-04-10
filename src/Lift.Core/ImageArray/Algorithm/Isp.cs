using System;
using System.Collections.Generic;
using System.Diagnostics;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using Lift.Core.Autofocus;
using static OpenCvSharp.LineIterator;

namespace Lift.Core.ImageArray.Algorithm;

// note: isp 相关算法

/// <summary>
/// 
/// </summary>
public static partial class Algorithm
{
    /// <summary>
    /// 判断是否是坏点
    /// </summary>
    /// <param name="mat"></param>
    /// <param name="row"></param>
    /// <param name="col"></param>
    /// <param name="radius"></param>
    /// <param name="threshold"></param>
    /// <returns></returns>
    static bool IsDeadPixel(Mat mat, int row, int col, int radius, double threshold)
    {
        var diameter = 2 * radius + 1;
        var roi = new Mat(mat, new Rect(col - radius, row - radius, diameter, diameter));
        var center = roi.GetValue(radius, radius);

        var dead = true;

        for (var i = 0; i < diameter; i++)
        {
            for (var j = 0; j < diameter; j++)
            {
                if (i == radius && j == radius) continue;
                if (!dead) return dead;

                dead &= (Math.Abs(center - roi.GetValue(i, j)) > threshold);
            }
        }

        return dead;
    }

    /// <summary>
    /// 去除坏点
    /// 
    /// todo 当前检测算法无法处理最边上的异常点
    /// todo 另外当前检测仅仅针对单通道情况
    /// </summary>
    /// <param name="mat">the source image</param>
    /// <param name="radius">the detect radius</param>
    /// <param name="threshold">threshold to determine whether it is dead pixel</param>
    /// <param name="mode">
    /// dead pixel repair algorithm
    /// 1 - mean (only)
    /// 2 - gaussian
    /// </param>
    /// <param name="thread">count of parallel</param>
    /// <param name="suspicious_threshold"></param>
    /// <returns></returns>
    public static Mat DeadPixelCorrection(this Mat mat,
        int radius = 2,
        double threshold = 50,
        int mode = 0,
        int thread = 8,
        double suspicious_threshold = 0.01)
    {
        var deadCount = 0;
        var done = false;

        var verbose = true;

        var cols = mat.Cols;
        var rows = mat.Rows;

        var count = cols * rows;

        if (cols < radius * 2 + 1 || rows < radius * 2 + 1)
            throw new System.Exception("The radius is too large or the mat is too small.");

        var result = mat.Clone();
        result.MinMaxLoc(out double min, out double max);

        var suspicious = new List<(int row, int col)>();
        var suspiciousCount = (int) (cols * rows * suspicious_threshold);

        var sort = new Mat();
        Cv2.SortIdx(mat.Reshape(1, 1), sort, SortFlags.Ascending);

        Parallel.For(0, suspiciousCount, new ParallelOptions()
        {
            MaxDegreeOfParallelism = thread
        }, item =>
        {

        });

        for (int i = 0; i < suspiciousCount; i++)
        {
            var index = (int) sort.GetValue(0, i);
            var row = index / cols;
            var col = index % cols;

            if (!(row < radius || col < radius || row > rows - radius - 1 || col > cols - radius - 1))
                suspicious.Add((row, col));

            index = (int) sort.GetValue(0, count - i);
            row = index / cols;
            col = index % cols;

            if (!(row < radius || col < radius || row > rows - radius - 1 || col > cols - radius - 1))
                suspicious.Add((row, col));
        }


        Parallel.ForEach(suspicious, new ParallelOptions()
        {
            MaxDegreeOfParallelism = thread
        }, item =>
        {
            var (row, col) = item;
            var isDead = IsDeadPixel(result, row, col, radius, threshold);

            if (!isDead)
                return;
             
            deadCount += 1;

            // mean
            var area = new List<double>();
            for (var i = row - radius; i < row + radius; i++)
                for (var j = col - radius; j < col + radius; j++)
                {
                    if (i == row && j == col) continue;
                    area.Add(result.GetValue(i, j));
                }

            area.Sort();
            result.SetValue(row, col, area[area.Count / 2]);
        });

        Console.WriteLine($"{deadCount}");
        return result;
    }
}
