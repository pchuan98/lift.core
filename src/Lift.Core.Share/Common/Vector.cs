using System;
using System.Collections.Generic;
using System.Linq;
using System.Reflection.PortableExecutable;
using System.Text;
using System.Threading.Tasks;

namespace Lift.Core.Common;

public struct Vector
{
    public int Id => Guid.NewGuid().GetHashCode();

    public double X { get; set; }

    public double Y { get; set; }

    public double Z { get; set; }

    public Vector(double x, double y, double z)
    {
        X = x;
        Y = y;
        Z = z;
    }

    public static Vector operator +(Vector a, Vector b)
        => new(a.X + b.X, a.Y + b.Y, a.Z + b.Z);

    public static Vector operator -(Vector a, Vector b)
        => new(a.X - b.X, a.Y - b.Y, a.Z - b.Z);

    public static Vector operator *(Vector a, Vector b)
        => new(a.X * b.X, a.Y * b.Y, a.Z * b.Z);
    public static Vector operator *(Vector a, double v)
        => new(a.X * v, a.Y * v, a.Z * v);

    public static Vector operator /(Vector a, Vector b)
        => new(a.X / b.X, a.Y / b.Y, a.Z / b.Z);

    public static Vector operator /(Vector a, double v)
        => new(a.X / v, a.Y / v, a.Z / v);

    public static bool operator ==(Vector a, Vector b)
        => Math.Abs(a.X - b.X) < double.Epsilon && Math.Abs(a.Y - b.Y) < double.Epsilon && Math.Abs(a.Z - b.Z) < double.Epsilon;

    public static bool operator !=(Vector a, Vector b)
        => !(a == b);

    public static Vector Zero()
        => new(0, 0, 0);

    public static Vector One()
        => new(1, 1, 1);

    public double Min()
        => Math.Min(X, Math.Min(Y, Z));

    public double Max()
        => Math.Max(X, Math.Max(Y, Z));

    public override bool Equals(object? obj)
    {
        if (obj is not Vector vec) return false;
        return this == vec && ReferenceEquals(this, vec);
    }

    public override int GetHashCode()
        => Id.GetHashCode();
}

public static class VectorExtension
{
    public static (int width, int height, int depth) ToInt3(this Vector vector)
        => ((int) vector.X, (int) vector.Y, (int) vector.Z);

    public static (double x, double y, double z) ToDoubleTuple(this Vector vector)
        => (vector.X, vector.Y, vector.Z);

    public static (float x, float y, float z) ToFloat3(this Vector vector)
        => ((float) vector.X, (float) vector.Y, (float) vector.Z);

    public static bool IsZero(this Vector vector)
        => vector == Vector.Zero();

    public static bool IsOne(this Vector vector)
        => vector == Vector.One();

    public static double Dot(Vector v1, Vector v2)
        => v1.X * v2.Y + v1.Y * v2.Y + v1.Z * v2.Z;

    public static Vector Orthogonalization(this Vector vector)
        => vector / vector.Max();

}
