namespace Lift.Core.Common;

/// <summary>
///  向量值的类型转换
/// </summary>
public partial class Vector
{
    #region Converter

    public (int width, int height, int depth) ToInt3()
        => ((int) X, (int) Y, (int) Z);

    public (double x, double y, double z) ToDouble3()
        => (X, Y, Z);

    public (float x, float y, float z) ToFloat3()
        => ((float) X, (float) Y, (float) Z);

    #endregion

    #region Valid

    public bool IsZero()
        => this == Vector.Zero();

    #endregion

    //    public static (int width, int height, int depth) ToInt3(this Vector vector)
    //        => ((int) vector.X, (int) vector.Y, (int) vector.Z);

    //    public static (double x, double y, double z) ToDoubleTuple(this Vector vector)
    //        => (vector.X, vector.Y, vector.Z);

    //    public static (float x, float y, float z) ToFloat3(this Vector vector)
    //        => ((float) vector.X, (float) vector.Y, (float) vector.Z);

    //    public static bool IsZero(this Vector vector)
    //        => vector == Vector.Zero();

    //    public static bool IsOne(this Vector vector)
    //        => vector == Vector.One();

    //    public static double Dot(Vector v1, Vector v2)
    //        => v1.X * v2.Y + v1.Y * v2.Y + v1.Z * v2.Z;

    //    public static Vector Orthogonalization(this Vector vector)
    //        => vector / vector.Max();

}


