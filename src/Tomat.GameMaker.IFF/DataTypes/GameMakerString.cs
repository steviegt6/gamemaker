using System.IO;
using Tomat.GameMaker.IFF.Chunks;
using Tomat.GameMaker.IFF.Chunks.Contexts;

namespace Tomat.GameMaker.IFF.DataTypes;

/// <summary>
///     Represents a string within a GameMaker IFF file.
/// </summary>
public sealed class GameMakerString : IGameMakerSerializable {
    /// <summary>
    ///     The value of the string.
    /// </summary>
    public string? Value { get; set; }

    public int Read(DeserializationContext context) {
        // TODO: Don't ignore length. Apparently unreliable - why? Having length
        // would make this probably maybe somewhat faster.
        _ = context.Reader.ReadInt32(); // Ignore the length.
        var start = context.Reader.Position;
        while (context.Reader.Data[context.Reader.Position] != 0)
            context.Reader.Position++;
        var length = context.Reader.Position - start;
        var value = context.Reader.ReadChars(length);
        Value = new string(value);
        context.Reader.Position++; // Skip the null terminator.
        return sizeof(int) + length + sizeof(byte);
    }

    public int Write(SerializationContext context) {
        if (Value is null)
            throw new IOException("Cannot serialize a null string.");

        var length = Value.Length;
        context.Writer.Write(length);
        context.Writer.Write(Value.ToCharArray());
        context.Writer.Write((byte)0); // Null terminator.
        return sizeof(int) + length + sizeof(byte);
    }
}
