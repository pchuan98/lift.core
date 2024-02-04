using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Lift.Core.ImageArray.Extensions;

/// <summary>
/// 
/// </summary>
public static class ColorMaps
{

    private static Mat? _gray = null;

    /// <summary>
    /// 
    /// </summary>
    public static Mat Gray
    {
        get
        {
            if (_gray != null)
                return _gray!;
            
            _gray = new Mat(256, 1, MatType.CV_8UC3);
            for (var i = 0; i < 256; i++)
                _gray.Set(i, 0, new Vec3b((byte) i, (byte) i, (byte) i));
            return _gray!;
        }
    }

    private static Mat? _green = null;

    /// <summary>
    /// 
    /// </summary>
    public static Mat Green
    {
        get
        {
            if (_green != null)
                return _green!;
            
            _green = new Mat(256, 1, MatType.CV_8UC3);
            for (var i = 0; i < 256; i++)
                _green.Set(i, 0, new Vec3b((byte) 0, (byte) i, (byte) 0));
            return _green!;
        }
    }



    private static Mat? _red = null;

    /// <summary>
    /// 
    /// </summary>
    public static Mat Red
    {
        get
        {
            if (_red != null)
                return _red!;
            

            _red = new Mat(256, 1, MatType.CV_8UC3);
            for (var i = 0; i < 256; i++)
                _red.Set(i, 0, new Vec3b((byte) 0, (byte) 0, (byte) i));
            return _red!;
        }
    }

    private static Mat? _blue = null;

    /// <summary>
    /// 
    /// </summary>
    public static Mat Blue
    {
        get
        {
            if (_blue != null)
                return _blue!;
            

            _blue = new Mat(256, 1, MatType.CV_8UC3);
            for (var i = 0; i < 256; i++)
                _blue.Set(i, 0, new Vec3b((byte) i, (byte) 0, (byte) 0));
            return _blue!;
        }
    }


    private static Mat? _purple = null;

    /// <summary>
    /// 
    /// </summary>
    public static Mat Pruple
    {
        get
        {
            if (_purple != null)
                return _purple!;
            
            _purple = new Mat(256, 1, MatType.CV_8UC3);
            for (var i = 0; i < 256; i++)
                _purple.Set(i, 0, new Vec3b((byte) i, (byte) 0, (byte) i));
            return _purple!;
        }
    }
}

public static partial class MatExtension
{
    /// <summary>
    /// 伪彩设置
    /// </summary>
    /// <param name="mat"></param>
    /// <param name="color"></param>
    /// <returns></returns>
    public static Mat Apply(this Mat mat, Mat color)
    {
        var res = new Mat();
        Cv2.ApplyColorMap(mat, res, color);
        return res;
    }
}
