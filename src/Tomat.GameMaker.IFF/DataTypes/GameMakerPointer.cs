using System;
using Tomat.GameMaker.IFF.Chunks;
using Tomat.GameMaker.IFF.Chunks.Contexts;
using Tomat.GameMaker.IFF.DataTypes.Models;

namespace Tomat.GameMaker.IFF.DataTypes;

/// <summary>
///     A pointer to a GameMaker object, storing an integer address to the
///     position of the object in the GameMaker IFF file, as well as (possible)
///     a reference to the object itself (set by <see cref="ReadObject"/>).
/// </summary>
/// <typeparam name="T">The GameMaker object type.</typeparam>
public struct GameMakerPointer<T> where T : IGameMakerSerializable, new() {
    /// <summary>
    ///     The offset of the pointer. This is currently only used by GameMaker
    ///     strings (<see cref="GameMakerString"/>), which are offset by four
    ///     bytes since the game points directly to the string contents,
    ///     skipping the 4-byte integer denoting its length.
    /// </summary>
    public int PointerOffset => GameMakerPointer.GetPointerOffset(typeof(T));

    /// <summary>
    ///     The address of the object in the GameMaker IFF file.
    /// </summary>
    public int Address { get; set; }

    /// <summary>
    ///     The instance of the object being pointed to, if
    ///     <see cref="ReadObject"/> has been called.
    /// </summary>
    public T? Object { get; set; }

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
