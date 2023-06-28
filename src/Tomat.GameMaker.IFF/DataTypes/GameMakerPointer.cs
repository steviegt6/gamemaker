using System;
using Tomat.GameMaker.IFF.Chunks;
using Tomat.GameMaker.IFF.Chunks.Contexts;
using Tomat.GameMaker.IFF.DataTypes.Models;

namespace Tomat.GameMaker.IFF.DataTypes;

public struct GameMakerPointer<T> : IGameMakerPointer<T> where T : IGameMakerSerializable, new() {
    public int PointerOffset => GameMakerPointer.GetPointerOffset(typeof(T));

    public int Address { get; set; }

    public T? Object { get; set; }

    // Address set in GameMakerIffReader::ReadPointer<T>(int) instead.
    /*public void Read(DeserializationContext context) {
        // Address = context.Reader.ReadInt32();
    }*/

    public void ReadObject(DeserializationContext context, bool returnAfter) {
        if (Address == 0) {
            Object = default;
            return;
        }

        if (context.Reader.Pointers.TryGetValue(Address, out var obj)) {
            Object = (T)obj;
        }
        else {
            Object = new T();
            context.Reader.Pointers[Address] = Object;
        }

        var pos = context.Reader.Position;
        context.Reader.Position = Address;

        Object.Read(context);

        if (returnAfter)
            context.Reader.Position = pos;
    }

    public void WriteObject(SerializationContext context) {
        if (Object is null)
            throw new InvalidOperationException("Cannot write null object.");

        context.Writer.Pointers[Object] = context.Writer.Position;
    }
}

public static class GameMakerPointer {
    public static int GetPointerOffset(Type type) {
        return type == typeof(GameMakerString) ? 4 : 0;
    }
}
