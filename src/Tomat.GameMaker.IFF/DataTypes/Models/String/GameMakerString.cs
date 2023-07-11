using System.IO;
using Tomat.GameMaker.IFF.Chunks;

namespace Tomat.GameMaker.IFF.DataTypes.Models.String;

/// <summary>
///     Represents a string within a GameMaker IFF file.
/// </summary>
public sealed class GameMakerString : IGameMakerSerializable {
    /// <summary>
    ///     The value of the string.
    /// </summary>
    public string? Value { get; set; }

    public void Read(DeserializationContext context) {
        // TODO: Don't ignore length. Apparently unreliable - why? Having length
        // would make this probably maybe somewhat faster.
        _ = context.ReadInt32(); // Ignore the length.
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
        if (Value is null)
            throw new IOException("Cannot serialize a null string.");

        var length = context.Encoding.GetByteCount(Value);
        context.Write(length);
        context.Write(context.Encoding.GetBytes(Value));
        context.Write((byte)0); // Null terminator.
    }

    public override string? ToString() {
        return Value;
    }
}
