using Tomat.GameMaker.IFF.DataTypes;
using Tomat.GameMaker.IFF.IO;

namespace Tomat.GameMaker.IFF.Chunks;

/// <summary>
///     Represents a context for serializing data to a GameMaker IFF file.
/// </summary>
/// <param name="Reader">
///     The reader used to read from a GameMaker IFF file.
/// </param>
/// <param name="IffFile">The GameMaker IFF file being read from.</param>
/// <param name="VersionInfo">
///     The version information of the GameMaker IFF file being read from.
/// </param>
public sealed record DeserializationContext(GameMakerIffReader Reader, GameMakerIffFile IffFile, GameMakerVersionInfo VersionInfo);

public static class DeserializationContextExtensions {
    public static GameMakerPointer<T> ReadPointerAndObject<T>(this DeserializationContext context, int addr, bool returnAfter = true, bool useTypeOffset = true) where T : IGameMakerSerializable, new() {
        var ptr = context.Reader.ReadPointer<T>(addr, useTypeOffset);
        ptr.ReadObject(context, returnAfter);
        return ptr;
    }

    public static GameMakerPointer<T> ReadPointerAndObject<T>(this DeserializationContext context, bool returnAfter = true, bool useTypeOffset = true) where T : IGameMakerSerializable, new() {
        return context.ReadPointerAndObject<T>(context.Reader.ReadInt32(), returnAfter, useTypeOffset);
    }
}
