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

    public delegate void ListElementWrite<T>(SerializationContext context, T element);

    public static void WriteList<T>(this SerializationContext context, List<T> list, ListWrite? beforeWrite = null, ListWrite? afterWrite = null, ListElementWrite<T>? elementWriter = null) where T : IGameMakerSerializable, new() {
        context.Writer.Write(list.Count);

        for (var i = 0; i < list.Count; i++) {
            beforeWrite?.Invoke(context, i, list.Count);

            if (elementWriter is null)
                list[i].Write(context);
            else
                elementWriter.Invoke(context, list[i]);

            afterWrite?.Invoke(context, i, list.Count);
        }
    }

    public static void WritePointerList<T>(
        this SerializationContext context,
        List<GameMakerPointer<T>> list,
        ListWrite? beforeWriter = null,
        ListWrite? afterWriter = null,
        ListElementWrite<GameMakerPointer<T>>? elementWriter = null,
        ListElementWrite<GameMakerPointer<T>>? elementPointerWriter = null
    ) where T : IGameMakerSerializable, new() {
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
