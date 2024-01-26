namespace Lift.Core.ImageArray.Extensions;

/// <summary>
/// MatType的init等价结果
/// </summary>
internal enum MatTypeAsInt
{
    CV_8UC1 = 0,
    CV_8UC2 = -1,
    CV_8UC3 = 16,
    CV_8UC4 = -1,
    CV_8SC1 = -1,
    CV_8SC2 = -1,
    CV_8SC3 = -1,
    CV_8SC4 = -1,
    CV_16UC1 = 2,
    CV_16UC2 = -1,
    CV_16UC3 = -1,
    CV_16UC4 = -1,
    CV_16SC1 = -1,
    CV_16SC2 = -1,
    CV_16SC3 = -1,
    CV_16SC4 = -1,
    CV_32SC1 = -1,
    CV_32SC2 = -1,
    CV_32SC3 = -1,
    CV_32SC4 = -1,
    CV_32FC1 = 5,
    CV_32FC2 = -1,
    CV_32FC3 = -1,
    CV_32FC4 = -1,
    CV_64FC1 = -1,
    CV_64FC2 = -1,
    CV_64FC3 = -1,
    CV_64FC4 = -1,
}

/// <summary>
/// 
/// </summary>
public static class MatRange
{
    /// <summary>
    /// 
    /// </summary>
    public const int MinU8 = byte.MinValue;

    /// <summary>
    /// 
    /// </summary>
    public const int MaxU8 = byte.MaxValue;

    /// <summary>
    /// 
    /// </summary>
    public const int MinS8 = sbyte.MinValue;

    /// <summary>
    /// 
    /// </summary>
    public const int MaxS8 = sbyte.MaxValue;

    /// <summary>
    /// 
    /// </summary>
    public const int MinU16 = ushort.MinValue;

    /// <summary>
    /// 
    /// </summary>
    public const int MaxU16 = ushort.MaxValue;

    /// <summary>
    /// 
    /// </summary>
    public const int MinS16 = short.MinValue;

    /// <summary>
    /// 
    /// </summary>
    public const int MaxS16 = short.MaxValue;

    /// <summary>
    /// 
    /// </summary>
    public const int MinS32 = int.MinValue;

    /// <summary>
    /// 
    /// </summary>
    public const int MaxS32 = int.MaxValue;

    /// <summary>
    /// 
    /// </summary>
    public const float MinF32 = 0f;

    /// <summary>
    /// 
    /// </summary>
    public const float MaxF32 = 1f;

    /// <summary>
    /// 
    /// </summary>
    public const double MinF64 = 0;

    /// <summary>
    /// 
    /// </summary>
    public const double MaxF64 = 1;
}

/// <summary>
/// 
/// </summary>
public static class MatDepth
{
    /// <summary>
    /// 
    /// </summary>
    public const int U8 = 0;

    /// <summary>
    /// 
    /// </summary>
    public const int S8 = 1;

    /// <summary>
    /// 
    /// </summary>
    public const int U16 = 2;

    /// <summary>
    /// 
    /// </summary>
    public const int S16 = 3;

    /// <summary>
    /// 
    /// </summary>
    public const int S32 = 4;

    /// <summary>
    /// 
    /// </summary>
    public const int F32 = 5;

    /// <summary>
    /// 
    /// </summary>
    public const int F64 = 6;
}
