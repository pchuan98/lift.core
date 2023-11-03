using OpenCvSharp;

namespace Lift.Core.Test.Helper
{
    using System;
    using Lift.Core.Helper;
    using Xunit;

    public static class ImageHelperTests
    {
        public static string ImagePath = Path.Join(Path.GetTempPath(), "test.tif");

        [Fact]
        public static void CanCallRead()
        {
            
            ImageTools.GenerateImages(ImagePath, 20, MatType.CV_16UC1);

            // Act
            var result = ImageHelper.Read(ImagePath);

            ImageHelper.QuickShow(ImagePath);

            // Assert
            //throw new NotImplementedException("Create or modify test");
        }

        [Theory]
        [InlineData(null)]
        [InlineData("")]
        [InlineData("   ")]
        public static void CannotCallReadWithInvalidPath(string value)
        {
            Assert.Throws<ArgumentNullException>(() => ImageHelper.Read(value));
        }
    }
}
