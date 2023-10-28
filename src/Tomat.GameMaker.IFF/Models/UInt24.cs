namespace Tomat.GameMaker.IFF.Models;

/// <summary>
///     Represents a 24-bit unsigned integer.
/// </summary>
public struct UInt24 {
    private readonly byte b0;
    private readonly byte b1;
    private readonly byte b2;

    public static UInt24 MaxValue => (UInt24)MAX_VALUE;

    public static UInt24 MinValue => (UInt24)MIN_VALUE;

    public const int MAX_VALUE = 8388607;
    public const int MIN_VALUE = -8388608;

    public UInt24(byte b0, byte b1, byte b2) {
        this.b0 = b0;
        this.b1 = b1;
        this.b2 = b2;
    }

    public static explicit operator UInt24(int value) {
        return new UInt24(
            (byte)(value & 0xFF),
            (byte)((value >> 8) & 0xFF),
            (byte)((value >> 16) & 0xFF)
        );
    }

    public static explicit operator int(UInt24 value) {
        return value.b0 | (value.b1 << 8) | (value.b2 << 16);
    }
}
