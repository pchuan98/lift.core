using Lift.Core.Exception;


namespace Lift.Core.Extensions;

public enum MatTypeAsInt
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

public static class MatExtension
{
    public static Mat ToU8(this Mat mat)
    {
        var rec = new Mat();
        var type = mat.Type();

        if (type == MatType.CV_32FC1)
            mat.ConvertTo(rec, MatType.CV_8U, byte.MaxValue);
        if (type == MatType.CV_16UC1)
            mat.ConvertTo(rec, MatType.CV_8U, 1.0 / (byte.MaxValue + 2));
        else throw new InvalidException($"Not support type : {type}");

        var t = rec.Type();
        rec.GetArray(out byte[] data);
        mat.GetArray(out ushort[] vvData);

        return rec;
    }

    public static Mat ToF32(this Mat mat)
    {
        var tempType = mat.Type();
        var tempInt = (int) mat.Type();
        return (int) mat.Type() switch
        {
            (int) MatTypeAsInt.CV_8UC1 => MatConverter.Channel1.U8ToF32(mat),
            (int) MatTypeAsInt.CV_8UC3 => MatConverter.Channel3.U8ToF32(mat),
            (int) MatTypeAsInt.CV_16UC1 => MatConverter.Channel1.U16ToF32(mat),
            (int) MatTypeAsInt.CV_32FC1 => mat,
            _ => throw new System.Exception(),
        };
    }



    public static bool Depth(this Mat mat, Type type)
    {
        return true;
    }
}


internal static class MatConverter
{
    public static class Channel1
    {
        public static Mat U8ToF32(Mat mat)
        {
            var rec = new Mat();
            mat.ConvertTo(rec, MatType.CV_32FC1, 1.0f / byte.MaxValue);

            return rec;
        }

        public static Mat U16ToF32(Mat mat)
        {
            var rec = new Mat();
            mat.ConvertTo(rec, MatType.CV_32FC1, 1.0f / ushort.MaxValue);

            return rec;
        }
    }

    public static class Channel3
    {
        public static Mat U8ToF32(Mat mat)
        {
            var gray = new Mat();
            // to gray
            Cv2.CvtColor(mat, gray, ColorConversionCodes.BGR2GRAY);

            var rec = new Mat();
            gray.ConvertTo(rec, MatType.CV_32FC1, 1.0f / byte.MaxValue);

            return rec;
        }
    }
}
