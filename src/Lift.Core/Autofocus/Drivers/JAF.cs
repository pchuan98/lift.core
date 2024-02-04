using System.Runtime.CompilerServices;

namespace Lift.Core.Autofocus.Drivers;

/// <summary>
/// 
/// </summary>
public class Jaf
{
    /// <summary>
    /// 粗找步进
    /// </summary>
    public double FirstStep { get; set; } = 2;

    /// <summary>
    /// 第一次寻找次数
    /// </summary>
    public int FirstCount { get; set; } = 1;

    /// <summary>
    /// 精找步进
    /// </summary>
    public double SecondStep { get; set; } = 0.2;

    /// <summary>
    /// 第二次精找次数
    /// </summary>
    public int SeccondCount { get; set; } = 5;

    /// <summary>
    /// 
    /// </summary>
    public double Threshold { get; set; } = 0.2;

    /// <summary>
    /// 评估的选取范围
    /// </summary>
    public double CropSize { get; set; } = 0.5;

    /// <summary>
    /// 最小的z，也是起始点
    /// </summary>
    public double MinZ { get; set; } = -75;

    /// <summary>
    /// 最大z
    /// </summary>
    public double MaxZ { get; set; } = 75;

    /// <summary>
    /// 聚焦评分指数
    /// </summary>
    /// <param name="mat"></param>
    /// <returns></returns>
    public virtual double Score(Mat mat)
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
                sharpNess += Math.Pow(con.GetValue(oh + i, ow + j), 2);

        return sharpNess;
    }


    #region 相机接口

    /// <summary>
    /// 
    /// </summary>
    /// <param name="mat"></param>
    /// <returns></returns>
    /// <exception cref="System.Exception"></exception>
    public virtual bool Capture(out Mat mat) => throw new System.Exception();

    #endregion

    #region 电动台接口

    /// <summary>
    /// 
    /// </summary>
    /// <param name="z"></param>
    /// <returns></returns>
    /// <exception cref="System.Exception"></exception>
    public virtual bool GetPosition(out double z) => throw new System.Exception();

    /// <summary>
    /// 设置当前坐标
    /// note: 要确定到位置
    /// </summary>
    /// <param name="z"></param>
    /// <exception cref="System.Exception"></exception>
    public virtual bool SetPosition(double z) => throw new System.Exception();



    #endregion

    /// <summary>
    /// 聚焦
    /// </summary>
    public virtual void Focus()
    {
        double bestZ = 5000;
        double bestScore = 0;
        double currentScore = 0;

        GetPosition(out var currentZ);
        var baseZ = currentZ - FirstStep * FirstCount;

        SetPosition(baseZ);
        Thread.Sleep(300);

        // rough search
        for (var i = 0; i < 2 * FirstCount + 1; i++)
        {
            var z = baseZ + i * FirstStep;
            if (z <= MinZ || z >= MaxZ) continue;

            SetPosition(z);
            GetPosition(out currentZ);

            Capture(out var capture);
            currentScore = Score(capture);

            if (currentScore > bestScore)
            {
                bestScore = currentScore;
                bestZ = currentZ;
            }
            else if (bestScore - currentScore > Threshold * bestScore)
                break;
        }

        baseZ = bestZ - SecondStep * SeccondCount;
        SetPosition(baseZ);
        Thread.Sleep(100);

        bestScore = 0;

        // fine search
        for (var i = 0; i < 2 * SeccondCount + 1; i++)
        {
            var z = baseZ + i * SecondStep;
            if (z <= MinZ || z >= MaxZ) continue;

            SetPosition(baseZ + i * SecondStep);
            GetPosition(out currentZ);

            Capture(out var capture);
            currentScore = Score(capture);

            if (currentScore > bestScore)
            {
                bestScore = currentScore;
                bestZ = currentZ;
            }
            else if (bestScore - currentScore > Threshold * bestScore)
                break;
        }

        SetPosition(bestZ);
    }
}
