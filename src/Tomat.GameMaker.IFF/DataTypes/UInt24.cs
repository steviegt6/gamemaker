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
    public static UInt24 MaxValue => 16777215;

    /// <summary>
    ///     Represents the minimum value of the data type.
    /// </summary>
    public static UInt24 MinValue => 0;

    public UInt24(byte b0, byte b1, byte b2) {
        this.b0 = b0;
        this.b1 = b1;
        this.b2 = b2;
    }

    public static implicit operator UInt24(int value) {
        return new UInt24((byte)(value & 0xFF), (byte)((value >> 8) & 0xFF), (byte)((value >> 16) & 0xFF));
    }

    public static implicit operator int(UInt24 value) {
        return value.b0 | (value.b1 << 8) | (value.b2 << 16);
    }
}
