namespace Lift.Core.ImageArray;

[Flags]
public enum ImageType
{
    /// <summary>
    /// 
    /// </summary>
    Unknow = 0,

    /// <summary>
    /// 0-255 (byte)
    /// </summary>
    Unsigned8 = 1,

    /// <summary>
    /// -128-127 (sbyte)
    /// </summary>
    Signed8 = 2,

    /// <summary>
    /// 0-65532 (ushort)
    /// </summary>
    Unsigned16 = 4,

    /// <summary>
    /// -32768-32767 (short)
    /// </summary>
    Signed16 = 8,

    /// <summary>
    /// -2^31 - 2^31-1 (int) 
    /// </summary>
    Unsigned32 = 16,

    /// <summary>
    /// 0.0 - 1.0 (float)
    /// </summary>
    Signed32 = 32,

    /// <summary>
    /// 0.0 - 1.0 (double)
    /// </summary>
    Unsigned64 = 64,

    /// <summary>
    /// 8-bit 
    /// </summary>
    Bit8 = Signed8 | Unsigned8,

    /// <summary>
    /// 16-bit
    /// </summary>
    Bit16 = Signed16 | Unsigned16,

    /// <summary>
    /// 
    /// </summary>
    Bit32 = Signed32 | Unsigned32,

    /// <summary>
    /// 
    /// </summary>
    Bit64 = Unsigned64

}
