using System.Collections.Generic;
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
    public delegate void ListRead(DeserializationContext context, int index, int count);

    public delegate GameMakerPointer<T> ListElementRead<T>(DeserializationContext context, bool notLast) where T : IGameMakerSerializable, new();

    public static GameMakerPointer<T> ReadPointerAndObject<T>(this DeserializationContext context, int addr, bool returnAfter, bool useTypeOffset = true) where T : IGameMakerSerializable, new() {
        var ptr = context.Reader.ReadPointer<T>(addr, useTypeOffset);
        ptr.ReadObject(context, returnAfter);
        return ptr;
    }

    public static List<GameMakerPointer<T>> ReadPointerList<T>(this DeserializationContext context, ListRead? beforeRead = null, ListRead? afterRead = null, ListElementRead<T>? elementReader = null)
        where T : IGameMakerSerializable, new() {
        elementReader ??= (ctx, _) => ctx.ReadPointerAndObject<T>(ctx.Reader.ReadInt32(), returnAfter: true);

        var count = context.Reader.ReadInt32();
        var list = new List<GameMakerPointer<T>>(count);

        for (var i = 0; i < count; i++) {
            beforeRead?.Invoke(context, i, count);
            list.Add(elementReader(context, i != count - 1));
            afterRead?.Invoke(context, i, count);
        }

        return list;
    }
}
