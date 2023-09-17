namespace Tomat.GameMaker.IFF.DataTypes.Models;

public interface IString : IGameMakerSerializable {
    string Value { get; set; }
}

/// <summary>
///     Represents a string within a GameMaker IFF file.
/// </summary>
internal sealed class GameMakerString : IString {
    /// <summary>
    ///     The value of the string.
    /// </summary>
    public string Value { get; set; } = null!;

    public void Read(DeserializationContext context) {
        // Ignore the length, which is unreliable.
        _ = context.ReadInt32();

        var start = context.Position;
        while (context.Data[context.Position] != 0)
            context.Position++;
        var length = context.Position - start;
        context.Position = start;
        var value = context.ReadChars(length);
        Value = new string(value);
        context.Position++; // Skip the null terminator.
    }

    public void Write(SerializationContext context) {
        var length = context.Encoding.GetByteCount(Value);
        context.Write(length);
        context.Write(context.Encoding.GetBytes(Value));
        context.Write((byte)0); // Null terminator.
    }

    public override string ToString() {
        return Value;
    }
}
