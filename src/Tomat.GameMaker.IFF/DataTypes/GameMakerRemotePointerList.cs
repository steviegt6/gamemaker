using System.Collections.Generic;
using Tomat.GameMaker.IFF.Chunks;

namespace Tomat.GameMaker.IFF.DataTypes;

/// <summary>
///     A remote pointer list, which is a list of pointers like a
///     <see cref="GameMakerPointerList{T}"/>, but without the guarantee that
///     pointer objects are adjacent to the pointers, meaning serialization will
///     not handling serializing the objects, which should be handled
///     separately.
/// </summary>
/// <typeparam name="T">The GameMaker object type.</typeparam>
public sealed class GameMakerRemotePointerList<T> : List<GameMakerPointer<T>>,
                                                    IGameMakerSerializable where T : IGameMakerSerializable, new() {
    public delegate void ListRead(DeserializationContext context, int index, int count);

    public delegate GameMakerPointer<T> ListElementRead(DeserializationContext context, bool notLast);

    public delegate void ListWrite(SerializationContext context, int index, int count);

    public delegate void ListElementWrite(SerializationContext context, GameMakerPointer<T> element);

    void IGameMakerSerializable.Read(DeserializationContext context) {
        Read(context);
    }

    public void Read(DeserializationContext context, ListRead? beforeRead = null, ListRead? afterRead = null, ListElementRead? elementReader = null) {
        elementReader ??= (ctx, _) => ctx.ReadPointerAndObject<T>();

        var count = context.ReadInt32();
        Capacity = count;

        for (var i = 0; i < count; i++) {
            beforeRead?.Invoke(context, i, count);
            Add(elementReader(context, i != count - 1));
            afterRead?.Invoke(context, i, count);
        }
    }

    void IGameMakerSerializable.Write(SerializationContext context) {
        Write(context);
    }

    public void Write(SerializationContext context, ListWrite? beforeWriter = null, ListWrite? afterWriter = null, ListElementWrite? elementPointerWriter = null) {
        context.Write(Count);

        // Write pointers.
        foreach (var item in this) {
            if (elementPointerWriter is null)
                context.Write(item);
            else
                elementPointerWriter.Invoke(context, item);
        }
    }
}
