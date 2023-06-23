namespace Tomat.GameMaker.IFF.DataTypes;

/// <summary>
///     Represents a 24-bit unsigned integer.
/// </summary>
public readonly struct UInt24  {
    /// <summary>
    ///     The size of the data type, in bytes.
    /// </summary>
    public const int SIZE = 3;

    private readonly byte b0;
    private readonly byte b1;
    private readonly byte b2;

    /// <summary>
    ///     Represents the maximum value of the data type.
    /// </summary>
    public static UInt24 MaxValue => MAX_VALUE;

    /// <summary>
    ///     Represents the minimum value of the data type.
    /// </summary>
    public static UInt24 MinValue => MIN_VALUE;
    
    public const uint MAX_VALUE = 16777215;
    public const uint MIN_VALUE = 0;

    public UInt24(byte b0, byte b1, byte b2) {
        this.b0 = b0;
        this.b1 = b1;
        this.b2 = b2;
    }

    public static implicit operator UInt24(uint value) {
        return new UInt24((byte)(value & 0xFF), (byte)((value >> 8) & 0xFF), (byte)((value >> 16) & 0xFF));
    }

    public static implicit operator uint(UInt24 value) {
        return value.b0 | (uint)(value.b1 << 8) | (uint)(value.b2 << 16);
    }
}
