namespace Lift.Core.Common;

/// <summary>
/// 向量
/// </summary>
public partial class Vector
{
    /// <summary>
    /// 
    /// </summary>
    public int Id => Guid.NewGuid().GetHashCode();

    /// <summary>
    /// 
    /// </summary>
    public double X { get; set; }

    public double Y { get; set; }

    public double Z { get; set; }

    public Vector(double x, double y, double z)
    {
        X = x;
        Y = y;
        Z = z;
    }

    public override bool Equals(object? obj)
    {
        if (obj is not Vector vec) return false;
        return this == vec && ReferenceEquals(this, vec);
    }

    public override int GetHashCode()
        => Id.GetHashCode();

}


//public static class VectorExtension
//{


//}

