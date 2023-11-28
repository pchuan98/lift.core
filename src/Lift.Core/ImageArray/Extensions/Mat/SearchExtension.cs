namespace Lift.Core.ImageArray.Extensions;

public static partial class MatExtension
{
    public static (int row, int col) SearchRowPrecedence(this Mat mat, double value)
    {
        var width = mat.Width;
        var height = mat.Height;

        if (mat.Type() == MatType.CV_8UC1)
            for (var i = 0; i < height; i++)
            {
                for (var j = 0; j < width; j++)
                {
                    var v = mat.Get<byte>(i, j);
                    if (DoubleBox.Equal(v, value)) return (i, j);
                }
            }

        else if (mat.Type() == MatType.CV_32FC1)
            for (var i = 0; i < height; i++)
            {
                for (var j = 0; j < width; j++)
                {
                    var v = mat.Get<float>(i, j);
                    if (DoubleBox.Equal(v, value)) return (i, j);
                }
            }
        else
            throw new NotSupportedException("");

        return (-1, -1);
    }
}
