namespace Lift.Core.ImageArray;

/// <summary>
/// 向量标记
/// </summary>
[Flags]
public enum ArrayFlag
{
    /// <summary>
    /// 原生数据类型
    /// </summary>
    None = 0,

    /// <summary>
    /// opencv作为中介对象
    /// </summary>
    OpenCv = 1,

    /// <summary>
    /// 一维数据
    /// </summary>
    Serial = 2,

    /// <summary>
    /// 二维数据
    /// </summary>
    Determinant = 4,

    /// <summary>
    /// 
    /// </summary>
    Image = OpenCv,
}
