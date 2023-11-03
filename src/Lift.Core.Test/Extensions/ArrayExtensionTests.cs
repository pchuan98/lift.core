using Lift.Core.Constant;
using OpenCvSharp;

namespace Lift.Core.Test.Extensions
{
    using System;
    using Lift.Core.Common;
    using Lift.Core.Extensions;
    using Xunit;

    public static class ArrayExtensionTests
    {
        [Fact]
        public static void TestToMat()
        {
            var array = new ImageArray(100, 100, 1);
            array.Flag = ArrayFlag.OpenCv;

            var mat = new Mat(new Size(100, 100), MatType.CV_8UC1);
            array.Object = mat;

            var result = array.ToMat();

            Assert.True(result!.GetType() == typeof(Mat));
        }


    }
}
