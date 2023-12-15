namespace Lift.Core.ImageArray;

/**
 * The image orientation is as follows:
 * ------- x
 * |
 * |
 * y
 *
 * The original data is rows (first) and columns, such:
 * 0 1 2 3 4 5 6
 * 7 8 9 10 ...
 */

/// <summary>
/// 
/// </summary>
public partial class ImageArray : IDisposable
{
    /// <summary>
    /// 
    /// </summary>
    /// <param name="width"></param>
    /// <param name="height"></param>
    /// <param name="depth"></param>
    public ImageArray(int width, int height, int depth)
    {
        Width = width;
        Height = height;
        StackCount = depth;
    }

    public ImageArray(double[,] array)
    {
        Width = array.GetLength(0);
        Height = array.GetLength(1);
        StackCount = 0;

        Object = array;
        Flag = ArrayFlag.Determinant;
    }


    /// <summary>
    /// image object data type
    /// </summary>
    public ArrayFlag Flag { get; set; } = ArrayFlag.None;

    /// <summary>
    /// 
    /// </summary>
    public ImageType ImageType { get; set; } = ImageType.Unknow;

    /// <summary>
    /// 
    /// </summary>
    public int ChannelCount { get; set; } = 0;

    /// <summary>
    /// image original height
    /// </summary>
    public int Height { get; internal set; }

    /// <summary>
    /// height
    /// </summary>
    public int Rows => Height;

    /// <summary>
    /// height
    /// </summary>
    public int Y => Height;

    /// <summary>
    /// image original width
    /// </summary>
    public int Width { get; internal set; }

    /// <summary>
    /// width
    /// </summary>
    public int Cols => Width;

    /// <summary>
    /// width
    /// </summary>
    public int X => Width;

    /// <summary>
    /// the count of stack image
    /// </summary>
    public int StackCount { get; internal set; }

    /// <summary>
    /// stack count
    /// </summary>
    public int Z => StackCount;

    /// <summary>
    /// x,y,z
    /// </summary>
    public Vector Size => new Vector(X, Y, Z);

    /// <summary>
    /// metadata(json format)
    /// </summary>
    public ImageMetadata Metadata { get; set; } = new ImageMetadata();

    /// <summary>
    /// Set the corresponding format according to ImageFlag
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
