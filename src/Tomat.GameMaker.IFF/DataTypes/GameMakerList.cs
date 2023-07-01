using System.Collections.Generic;
using Tomat.GameMaker.IFF.Chunks;

namespace Tomat.GameMaker.IFF.DataTypes;

public sealed class GameMakerList<T> : List<T>,
                                       IGameMakerSerializable where T : IGameMakerSerializable, new() {
    public delegate void ListRead(DeserializationContext context, int index, int count);

    public delegate T ListElementRead(DeserializationContext context, bool notLast);

    public delegate void ListWrite(SerializationContext context, int index, int count);

    public delegate void ListElementWrite(SerializationContext context, T element);

    void IGameMakerSerializable.Read(DeserializationContext context) {
        Read(context);
    }

    public void Read(DeserializationContext context, ListRead? beforeRead = null, ListRead? afterRead = null, ListElementRead? elementReader = null) {
        var count = context.Reader.ReadInt32();
        Capacity = count;

        for (var i = 0; i < count; i++) {
            beforeRead?.Invoke(context, i, count);

            T elem;

            if (elementReader is null) {
                elem = new T();
                elem.Read(context);
            }
            else {
                elem = elementReader(context, i != count - 1);
            }

            Add(elem);

            afterRead?.Invoke(context, i, count);
        }
    }

    void IGameMakerSerializable.Write(SerializationContext context) {
        Write(context);
    }

    public void Write(SerializationContext context, ListWrite? beforeWrite = null, ListWrite? afterWrite = null, ListElementWrite? elementWriter = null) {
        context.Writer.Write(Count);

        for (var i = 0; i < Count; i++) {
            beforeWrite?.Invoke(context, i, Count);

            if (elementWriter is null)
                this[i].Write(context);
            else
                elementWriter.Invoke(context, this[i]);

            afterWrite?.Invoke(context, i, Count);
        }
    }
}
