using Lift.Core.Extensions.Image;
using Lift.Core.Helper;
using OpenCvSharp;

namespace Lift.Core.Test.Extensions.Image
{

    public class MergeExtensionTests
    {
        static string GeneratePicture()
        {
            var col = 10;
            var row = 10;

            var width = 2048;
            var height = 2048;

            var root = Path.Join(Path.GetTempPath(), "Simscop.Test.Image");
            if (!Directory.Exists(root)) Directory.CreateDirectory(root);

            var size = new Size(width, height);

            for (var i = 0; i < col * row; i++)
            {
                if (File.Exists(Path.Join(root, $"{i + 1}.png"))) continue;

                var color = (int) ((i * 1.0 / (col * row)) * 255);
                var image = new Mat(size, MatType.CV_8UC3);
                Cv2.Randu(image, new Scalar(0, 0, 0), new Scalar(256, 256, 256));

                var fontsize = 64;
                var font = HersheyFonts.HersheyPlain;
                var fontweight = 10;

                var textSize = Cv2.GetTextSize(i.ToString(), font, fontsize, fontweight, out int baseline);
                var textPosition = new Point((image.Width - textSize.Width) / 2, (image.Height + textSize.Height) / 2);
                var textColor = 0;

                Cv2.PutText(image,
                    (i + 1).ToString(),
                    textPosition,
                    font,
                    fontsize,
                    new Scalar(textColor, textColor, textColor),
                    fontweight);

                Cv2.ImWrite(Path.Join(root, $"{i + 1}.png"), image);

                image.Release();
            }

            return root;
        }

        [Fact]
        public void MergeArrayTest()
        {
            var root = GeneratePicture();

            var rows = 10;
            var cols = 10;

            var imgs = new ImageArray[cols * rows];
            for (var i = 0; i < cols * rows; i++)
                imgs[i] = ImageHelper.Read(Path.Join(root, $"{i + 1}.png"));

            var merge = imgs.MergeArray(cols, rows, 32);

            Assert.True(imgs[0].Width * cols == merge.Width);
            Assert.True(imgs[0].Height * rows == merge.Height);
        }
    }
}
