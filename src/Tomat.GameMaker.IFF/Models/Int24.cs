namespace Tomat.GameMaker.IFF.Models;

/// <summary>
///     Represents a 24-bit signed integer.
/// </summary>
public struct Int24 {
    private readonly byte b0;
    private readonly byte b1;
    private readonly byte b2;

    public static Int24 MaxValue => (Int24)MAX_VALUE;

    public static Int24 MinValue => (Int24)MIN_VALUE;

    public const int MAX_VALUE = 8388607;
    public const int MIN_VALUE = -8388608;

    public Int24(byte b0, byte b1, byte b2) {
        this.b0 = b0;
        this.b1 = b1;
        this.b2 = b2;
    }

    public static explicit operator Int24(int value) {
        return new Int24(
            (byte)(value & 0xFF),
            (byte)((value >> 8) & 0xFF),
            (byte)((value >> 16) & 0xFF)
        );
    }

    public static explicit operator int(Int24 value) {
        var result = value.b0 | (value.b1 << 8) | (value.b2 << 16);

        if ((result & 0x800000) != 0)
            result |= unchecked((int)0xFF000000);

        return result;
    }
}
