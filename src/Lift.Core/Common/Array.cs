using System;
using System.Collections.Generic;
using System.Linq;
using System.Net.WebSockets;
using System.Security.Principal;
using System.Text;
using System.Threading.Tasks;
using Lift.Core.Constant;
using OpenCvSharp.Internal.Vectors;

namespace Lift.Core.Common;


/// <summary>
/// -------------------------------------------------------
/// 1D - serial
/// -------------------------------------------------------
/// width       =       n
/// height      =       0
/// depth       =       0
///
/// valid : IsSerial = true
/// arrya such as : a1,a2,a3,a4,...,...
///
/// -------------------------------------------------------
/// 1D - discrete
/// -------------------------------------------------------
/// width       =       n
/// height      =       0
/// depth       =       0
///
/// valid : IsSerial = false
/// array such as : (i1,a1),(i2,a2),...,...
///
/// -------------------------------------------------------
/// 2D - serial
/// -------------------------------------------------------
/// width       =       n
/// height      =       m
/// depth       =       0
///
/// valid : IsSerial = true
/// array such as :
/// 
///         a11,a12,a13,...,a1n,
///         ...             ...,
///         am1,am2,am3,...,amn,
///
/// -------------------------------------------------------
/// 2D - discrete
/// -------------------------------------------------------
/// width       =       n
/// height      =       m
/// depth       =       0
///
/// valid : IsSerial = false
/// array such as :
///
///         (x,y,value),...,
/// 
/// -------------------------------------------------------
/// 3D - serial
/// -------------------------------------------------------
/// width       =       n
/// height      =       m
/// depth       =       l
///
/// valid : IsSerial = true
/// array such as :
///
///         (w,h,d) = (col,row,z) = (x,y,z)
///
/// -------------------------------------------------------
/// 3D - discrete
/// -------------------------------------------------------
/// width       =       n
/// height      =       m
/// depth       =       l
///
/// valid : IsSerial = false
/// array such as :
///
///         (x,y,z,u),...,
/// 
/// </summary>
public class Array : IDisposable
{
    public Array(int width, int height, int depth)
    {
        Width = width;
        Height = height;
        Depth = depth;
    }

    public Array(Mat mat)
    {
        Width = mat.Width;
        Height = mat.Height;
        Depth = 1;

        Object = mat;
        Flag = ArrayFlag.OpenCv;
    }

    /// <summary>
    /// 
    /// </summary>
    public ArrayFlag Flag { get; set; } = ArrayFlag.None;

    /// <summary>
    /// 
    /// </summary>
    public int Height { get; internal set; }

    /// <summary>
    /// 
    /// </summary>
    public int Width { get; internal set; }

    /// <summary>
    /// 
    /// </summary>
    public int Depth { get; internal set; }

    /// <summary>
    /// 
    /// </summary>
    public Vector Size => new Vector(Height, Width, Depth);

    /// <summary>
    /// 数据是否连续
    /// </summary>
    public bool IsSerial { get; set; } = true;

    /// <summary>
    /// 数据元信息，这里使用json作为中间解析库
    /// </summary>
    public ImageMetadata Metadata { get; set; } = new ImageMetadata();

    /// <summary>
    /// 内容主体
    /// </summary>
    public object? Object { get; set; } = null;

    /// <summary>
    /// 
    /// </summary>
    public void Dispose()
    {
        GC.SuppressFinalize(this);
    }
}
