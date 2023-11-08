namespace Lift.Core.Common;

/**
 * Pixel        : 图像元素，图像的最小单位
 * Resolution   : 图像分辨率，图像的宽高
 * W & H & D    : 图像的宽高深度，是图像的物理信息
 */


/// <summary>
/// 图像的元数据
/// </summary>
public class ImageMetadata
{
    /// <summary>
    /// size of each pixel
    /// </summary>
    public Vector PerPixelSize { get; set; } = new(0, 0, 0);

    /// <summary>
    /// pixel size physical unit
    /// </summary>
    public string Uint { get; set; } = "um";

    /// <summary>
    /// non-standard image data
    /// </summary>
    public Dictionary<string, ImageMetadata>? Metadatas { get; set; }
}
