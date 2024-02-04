using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Windows;
using System.Windows.Controls;
using System.Windows.Data;
using System.Windows.Documents;
using System.Windows.Input;
using System.Windows.Media;
using System.Windows.Media.Imaging;
using System.Windows.Shapes;
using Lift.Core.ImageArray.Extensions;
using OpenCvSharp;
using OpenCvSharp.WpfExtensions;

namespace Test.Wpf;

/// <summary>
/// Interaction logic for ChannelView.xaml
/// </summary>
public partial class ChannelView
{
    public ChannelView()
    {
        InitializeComponent();

        var merge = (new[]{
            Cv2.ImRead(@"E:\.test\BPAE405.tif", ImreadModes.Unchanged)
                .MinMaxNorm().ToU8().Apply(ColorMaps.Blue),
            Cv2.ImRead(@"E:\.test\BPAE488.tif", ImreadModes.Unchanged)
                .MinMaxNorm().ToU8().Apply(ColorMaps.Green),
            Cv2.ImRead(@"E:\.test\BPAE525.tif", ImreadModes.Unchanged)
                .MinMaxNorm().ToU8().Apply(ColorMaps.Red)
        }).MergeChannelAsMax();
        Viewer.ImageSource = BitmapFrame.Create(merge.ToBitmapSource());
    }
}
