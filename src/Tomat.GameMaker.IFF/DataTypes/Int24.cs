namespace Tomat.GameMaker.IFF.DataTypes;

/// <summary>
///     Represents a 24-bit signed integer.
/// </summary>
public readonly struct Int24  {
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
    public static Int24 MaxValue => 8388607;

    /// <summary>
    ///     Represents the minimum value of the data type.
    /// </summary>
    public static Int24 MinValue => -8388608;

    public Int24(byte b0, byte b1, byte b2) {
        this.b0 = b0;
        this.b1 = b1;
        this.b2 = b2;
    }

    public static implicit operator Int24(int value) {
        return new Int24((byte)(value & 0xFF), (byte)((value >> 8) & 0xFF), (byte)((value >> 16) & 0xFF));
    }

    public static implicit operator int(Int24 value) {
        var res = value.b0 | (value.b1 << 8) | (value.b2 << 16);

        if ((res & 0x800000) != 0)
            res |= unchecked((int)0xFF000000);

        return res;
    }
}
