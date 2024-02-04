using System.Diagnostics;
using System.Windows.Input;
using System.Windows.Media.Imaging;
using CommunityToolkit.Mvvm.ComponentModel;
using CommunityToolkit.Mvvm.Input;
using CommunityToolkit.Mvvm.Messaging;
using Lift.Core.ImageArray.Algorithm;
using Lift.Core.ImageArray.Extensions;
using Lift.UI.Controls;
using OpenCvSharp;
using OpenCvSharp.WpfExtensions;

namespace Test.Wpf;

/// <summary>
/// 像素坐标变化
/// </summary>
/// <param name="X"></param>
/// <param name="Y"></param>
public record CurrentPositionMessage(int X, int Y);

/// <summary>
/// 反算电动台坐标
/// </summary>
/// <param name="X"></param>
/// <param name="Y"></param>
public record MappingMoveMessage(double X, double Y);

/// <summary>
/// Interaction logic for StitcherView.xaml
/// </summary>
public partial class StitcherView
{
    public StitcherView()
    {
        InitializeComponent();

        this.DataContext = new StitcherViewModel();
    }

    private void UIElement_OnMouseRightButtonDown(object sender, MouseButtonEventArgs e)
    {
        if (sender is not ImageViewer viewer) return;

        var pos = viewer.ImageCurrentPosition;

        var (x, y) = pos;

        if (x == -1 || y == -1) return;

        WeakReferenceMessenger.Default.Send(nameof(CurrentPositionMessage), new CurrentPositionMessage(x, y));
    }
}

public class CameraStitcherProvider : IStitcherProvider
{
    public int Count { get; set; } = 0;

    public List<string> Paths = new();

    public CameraStitcherProvider()
    {
        for (var i = 0; i < 6; i++)
            for (var j = 0; j < 20; j++)
                Paths.Add($@"E:\.test\stitch\{i + 1}-{j + 1}.TIF");
    }

    /// <inheritdoc/>
    (Mat mat, double x, double y, int row, int col) IStitcherProvider.Provide()
    {
        var path = Paths[Count++];
        var name = System.IO.Path.GetFileNameWithoutExtension(path);

        var pos = name.Split("-");
        var c = int.Parse(pos[0]);
        var r = int.Parse(pos[1]);
        var x = c * (2560 / 2);
        var y = r * (2160 / 2);

        Debug.WriteLine($"{x} - {y}");

        var mat = Cv2.ImRead(path, ImreadModes.Unchanged);
        mat = mat.ToU8();

        return new(mat, x, y, r - 1, c - 1);
    }
}

public partial class StitcherViewModel : ObservableObject
{
    public IStitcher Scanner = new ScanStitcher()
    {
        Provider = new CameraStitcherProvider(),
        PerPixelDistance = 1,
        Width = 5000,
        Height = 5000,
        LeftTop = (0, 0),
        RightBottom = (5000, 5000)
    };

    [ObservableProperty]
    private BitmapFrame _frame;

    [RelayCommand]
    void Step()
    {
        Scanner.Step();

        Frame = BitmapFrame.Create(Scanner.StitchMat.ToBitmapSource());
    }
}
