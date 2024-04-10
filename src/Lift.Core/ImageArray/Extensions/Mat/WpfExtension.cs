namespace Lift.Core.ImageArray.Extensions;

public static partial class MatExtension
{
    /// <summary>
    /// 
    /// </summary>
    /// <param name="mat"></param>
    /// <param name="scale"></param>
    /// <returns></returns>
    public static Mat Resize(this Mat mat, double scale)
    {
        var width = mat.Width;
        var height = mat.Height;

        return mat.Resize(new Size(width * scale, height * scale));
    }

    public static void Show(this Mat mat, double size = 1000, double interval = 0.1)
    {
        var scale = new Mat();
        var cols = mat.Cols;
        var rows = mat.Rows;

        double sc = cols;
        double rs = rows;

        double s = 1;

        while (sc > size || rs > size)
        {
            sc = s * cols;
            rs = s * rows;

            s -= 0.1;
        }

        Cv2.Resize(mat, scale, new Size((int) sc, (int) rs));

        Cv2.ImShow("test", scale);
        Cv2.WaitKey(0);
    }
}
