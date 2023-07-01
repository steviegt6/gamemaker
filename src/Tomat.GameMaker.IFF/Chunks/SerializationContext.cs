using System.Collections.Generic;
using Tomat.GameMaker.IFF.DataTypes;
using Tomat.GameMaker.IFF.IO;

namespace Tomat.GameMaker.IFF.Chunks;

/// <summary>
///     Represents a context for serializing data to a GameMaker IFF file.
/// </summary>
/// <param name="Writer">
///     The writer used to write to a GameMaker IFF file.
/// </param>
/// <param name="IffFile">The GameMaker IFF file being written to.</param>
/// <param name="VersionInfo">
///     The version information of the GameMaker IFF file being read from.
/// </param>
public sealed record SerializationContext(GameMakerIffWriter Writer, GameMakerIffFile IffFile, GameMakerVersionInfo VersionInfo);

public static class SerializationContextExtensions {
    public delegate void ListWrite(SerializationContext context, int index, int count);

    public delegate void ListElementWrite<T>(SerializationContext context, GameMakerPointer<T> element) where T : IGameMakerSerializable, new();

    public static void WritePointerList<T>(this SerializationContext context, List<GameMakerPointer<T>> list, ListWrite? beforeWriter, ListWrite? afterWriter, ListElementWrite<T>? elementWriter, ListElementWrite<T>? elementPointerWriter)
        where T : IGameMakerSerializable, new() {
        context.Writer.Write(list.Count);

        // Write pointers.
        foreach (var item in list) {
            if (elementPointerWriter is null)
                context.Writer.Write(item);
            else
                elementPointerWriter.Invoke(context, item);
        }

        // Write elements.
        for (var i = 0; i < list.Count; i++) {
            beforeWriter?.Invoke(context, i, list.Count);

            if (elementWriter is null) {
                list[i].WriteObject(context);
                list[i].Object!.Write(context);
            }
            else {
                elementWriter.Invoke(context, list[i]);
            }

            afterWriter?.Invoke(context, i, list.Count);
        }
    }
}
