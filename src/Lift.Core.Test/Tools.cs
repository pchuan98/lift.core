using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using OpenCvSharp;

namespace Lift.Core.Test;

public static class ImageTools
{
    public static void GenerateImage(string path, MatType type)
    {
        var width = 1024;
        var height = 1024;

        var ramdom = new Random();

        var mat = new Mat(new Size(width, height), type);
        Cv2.Randu(mat, Scalar.Black, Scalar.White);

        mat.SaveImage(path);
    }

    public static void GenerateImages(string path, int count, MatType type)
    {
        var width = 1024;
        var height = 1024;

        var ramdom = new Random();

        var mats = new Mat[count];

        for (var i = 0; i < count; i++)
        {
            var mat = new Mat(new Size(width, height), type);
            Cv2.Randu(mat, Scalar.Black, Scalar.White);
            mats[i] = mat;
        }

        Cv2.ImWrite(path, mats);

    }
}
