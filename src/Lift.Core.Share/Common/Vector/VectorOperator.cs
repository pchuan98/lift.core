namespace Lift.Core.Common;

/// <summary>
/// 向量的四则运算和比较
/// </summary>
public partial class Vector
{
    #region +

    /// <summary>
    /// 
    /// </summary>
    /// <param name="a"></param>
    /// <param name="b"></param>
    /// <returns></returns>
    public static Vector operator +(Vector a, Vector b)
        => new Vector(a.X + b.X, a.Y + b.Y, a.Z + b.Z);

    #endregion

    #region -

    /// <summary>
    /// 向量之间相减
    /// </summary>
    /// <param name="a"></param>
    /// <param name="b"></param>
    /// <returns></returns>
    public static Vector operator -(Vector a, Vector b)
    => new(a.X - b.X, a.Y - b.Y, a.Z - b.Z);


    #endregion

    #region *

    /// <summary>
    /// 向量之间相乘
    /// </summary>
    /// <param name="a"></param>
    /// <param name="b"></param>
    /// <returns></returns>
    public static Vector operator *(Vector a, Vector b)
        => new(a.X * b.X, a.Y * b.Y, a.Z * b.Z);

    /// <summary>
    /// 向量乘值
    /// </summary>
    /// <param name="a"></param>
    /// <param name="v"></param>
    /// <returns></returns>
    public static Vector operator *(Vector a, double v)
        => new(a.X * v, a.Y * v, a.Z * v);

    #endregion

    #region /

    /// <summary>
    /// 向量之间相除
    /// </summary>
    /// <param name="a"></param>
    /// <param name="b"></param>
    /// <returns></returns>
    public static Vector operator /(Vector a, Vector b)
        => new(a.X / b.X, a.Y / b.Y, a.Z / b.Z);

    /// <summary>
    /// 向量除值
    /// </summary>
    /// <param name="a"></param>
    /// <param name="v"></param>
    /// <returns></returns>
    public static Vector operator /(Vector a, double v)
        => new(a.X / v, a.Y / v, a.Z / v);

    #endregion

    #region == !=

    /// <summary>
    /// 向量是否相等比较
    /// </summary>
    /// <param name="a"></param>
    /// <param name="b"></param>
    /// <returns></returns>
    public static bool operator ==(Vector a, Vector b)
        => DoubleBox.Equal(a.X, b.X) && DoubleBox.Equal(b.X, b.Y) && DoubleBox.Equal(a.Z, b.Z);

    /// <summary>
    /// 向量是否不等比较
    /// </summary>
    /// <param name="a"></param>
    /// <param name="b"></param>
    /// <returns></returns>
    public static bool operator !=(Vector a, Vector b)
        => !(a == b);

    #endregion

}
