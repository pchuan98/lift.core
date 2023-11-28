namespace Lift.Core.Common;

/// <summary>
///  向量取值
/// </summary>
public partial class Vector
{
    /// <summary>
    /// 取零
    /// </summary>
    /// <returns></returns>
    public static Vector Zero()
        => new(0, 0, 0);

    /// <summary>
    /// 取一
    /// </summary>
    /// <returns></returns>
    public static Vector One()
        => new(1, 1, 1);

    /// <summary>
    /// 取最小值
    /// </summary>
    /// <returns></returns>
    public double Min()
        => Math.Min(X, Math.Min(Y, Z));

    /// <summary>
    /// 取最大值
    /// </summary>
    /// <returns></returns>
    public double Max()
        => Math.Max(X, Math.Max(Y, Z));


}
