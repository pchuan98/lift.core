using System;
using System.Collections.Generic;
using System.Diagnostics;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using Lift.Core.Share.Extensions;

namespace Lift.Core.Autofocus;

/// <summary>
/// 
/// </summary>
public class JAF
{
    /// <summary>
    /// 
    /// </summary>
    public double SizeFirst { get; set; } = 2;

    /// <summary>
    /// 
    /// </summary>
    public int NumFirst { get; set; } = 1;

    /// <summary>
    /// 
    /// </summary>
    public double SizeSecond { get; set; } = 0.2;

    /// <summary>
    /// 
    /// </summary>
    public int NumSecond { get; set; } = 5;

    /// <summary>
    /// 
    /// </summary>
    public double Threshold { get; set; } = 0.2;

    /// <summary>
    /// 
    /// </summary>
    public double CropSize { get; set; } = 0.2;

    /// <summary>
    /// 聚焦评分指数
    /// </summary>
    /// <param name="mat"></param>
    /// <returns></returns>
    public double Score(Mat mat)
    {
        var width = (int) (CropSize * mat.Width);
        var height = (int) (CropSize * mat.Height);

        var ow = (int) ((1 - CropSize) / 2) * mat.Width;
        var oh = (int) ((1 - CropSize) / 2) * mat.Height;

        double sharpNess = 0;

        var mean = mat.MedianBlur(3);

        var kernel = new float[] { 2, 1, 0, 1, 0, -1, 0, -1, -2 }.ToKernel();
        var con = new Mat();
        Cv2.Filter2D(mat, con, -1, kernel);


        for (var i = 0; i < height; i++)
            for (var j = 0; j < width; j++)
                sharpNess += Math.Pow(con.GetValue(ow + i, oh + j), 2);

        return sharpNess;
    }


    #region 相机接口

    public virtual bool Capture(out Mat mat)
    {
        throw new System.Exception();
    }

    #endregion

    #region 电动台接口

    /// <summary>
    /// 
    /// </summary>
    /// <param name="z"></param>
    /// <returns></returns>
    /// <exception cref="System.Exception"></exception>
    public virtual bool GetPosition(out double z)
    {
        throw new System.Exception();
    }

    /// <summary>
    /// 设置当前坐标
    /// note: 要确定到位置
    /// </summary>
    /// <param name="z"></param>
    /// <exception cref="System.Exception"></exception>
    public virtual bool SetPosition(double z)
    {
        throw new System.Exception();
    }

    #endregion

    /// <summary>
    /// 
    /// </summary>
    void Focus()
    {
        double bestZ = 5000;
        double bestScore = 0;
        double currentScore = 0;

        GetPosition(out var currentZ);
        var baseZ = currentZ - SizeFirst * NumFirst;

        SetPosition(baseZ);
        Thread.Sleep(300);

        // rough search
        for (var i = 0; i < 2 * NumFirst + 1; i++)
        {
            SetPosition(baseZ + i * SizeFirst);
            GetPosition(out currentZ);

            Capture(out var capture);
            currentScore = Score(capture);

            if (currentScore > bestScore)
            {
                bestScore = currentScore;
                bestZ = currentZ;
            }
            else if (bestScore - currentScore > Threshold * bestScore && bestZ < 5000)
                break;
        }

        baseZ = bestZ - SizeFirst * NumFirst;
        SetPosition(baseZ);
        Thread.Sleep(100);

        bestScore = 0;

        // fine search
        for (var i = 0; i < 2 * NumSecond + 1; i++)
        {
            SetPosition(baseZ + i * SizeSecond);
            GetPosition(out currentZ);

            Capture(out var capture);
            currentScore = Score(capture);

            if (currentScore > bestScore)
            {
                bestScore = currentScore;
                bestZ = currentZ;
            }
            else if (bestScore - currentScore > Threshold * bestScore && bestZ < 5000)
                break;
        }

        SetPosition(bestZ);
    }
}
