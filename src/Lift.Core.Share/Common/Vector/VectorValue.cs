namespace Lift.Core.Common;

public partial class Vector
{
    /// <summary>
    /// 
    /// </summary>
    /// <returns></returns>
    public static Vector Zero()
        => new(0, 0, 0);

    /// <summary>
    /// 
    /// </summary>
    /// <returns></returns>
    public static Vector One()
        => new(1, 1, 1);

    /// <summary>
    /// 
    /// </summary>
    /// <returns></returns>
    public double Min()
        => Math.Min(X, Math.Min(Y, Z));

    /// <summary>
    /// 
    /// </summary>
    /// <returns></returns>
    public double Max()
        => Math.Max(X, Math.Max(Y, Z));


}
