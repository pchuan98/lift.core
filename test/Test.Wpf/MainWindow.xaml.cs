using System.IO;
using System.Windows.Media.Imaging;
using CommunityToolkit.Mvvm.ComponentModel;
using Lift.Core.ImageArray.Algorithm;
using Lift.Core.ImageArray.Extensions;
using OpenCvSharp;
using OpenCvSharp.WpfExtensions;

namespace Test.Wpf;

/// <summary>
/// Interaction logic for MainWindow.xaml
/// </summary>
public partial class MainWindow
{
    public MainWindow()
    {
        InitializeComponent();

        DataContext = new MainViewModel();
    }
}


public partial class MainViewModel : ObservableObject
{
    [ObservableProperty]
    private BitmapFrame? _frame;

    [ObservableProperty]
    private int _overlap = 1527;

    [ObservableProperty]
    private int _offset = 6;

    [ObservableProperty]
    private int _max = 0;

    partial void OnOverlapChanged(int value) => Update();

    partial void OnOffsetChanged(int value) => Update();

    public Mat[] Mats { get; set; }

    public MainViewModel()
    {
        var mats = new List<string>()
            {
                @"E:\.test\stitch3\X_-18500_Y_-1500.TIF",
                @"E:\.test\stitch3\X_-17000_Y_-1500.TIF",
            }
            .Select(item => Cv2.ImRead(item));

        Mats = mats.ToArray();
        Max = Mats[0].Width;

        Update();
    }

    void Update() => Frame = BitmapFrame.Create(Mats[0].MergeFromRight(Mats[1], Overlap, Offset).ToBitmapSource());
    //void Update() => Frame = BitmapFrame.Create(Mats.Stitching().ToBitmapSource());
}
