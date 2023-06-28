using Tomat.GameMaker.IFF.Chunks;
using Tomat.GameMaker.IFF.Chunks.Contexts;

namespace Tomat.GameMaker.IFF.DataTypes;

/// <summary>
///     A pointer to a GameMaker object, storing an integer address to the
///     position of the object in the GameMaker IFF file, as well as (possibly)
///     a reference to the object itself.
/// </summary>
/// <seealso cref="GameMakerPointer{T}"/>
public interface IGameMakerPointer {
    /// <summary>
    ///     The offset of the pointer. This is currently only used by GameMaker
    ///     strings, which are offset by four bytes since the game directly
    ///     points to the string contents, skipping over the length.
    /// </summary>
    int PointerOffset { get; }

    /// <summary>
    ///     The address of the object in the GameMaker IFF file.
    /// </summary>
    int Address { get; set; }

    /// <summary>
    ///     The instance of the object being pointed to, if it has been read.
    /// </summary>
    IGameMakerSerializable? Object { get; set; }
}

/// <summary>
///     A pointer to a GameMaker object, storing an integer address to the
///     position of the object in the GameMaker IFF file, as well as (possibly)
///     a reference to the object itself.
/// </summary>
/// <seealso cref="GameMakerPointer{T}"/>
public interface IGameMakerPointer<T> : IGameMakerPointer where T : IGameMakerSerializable, new() {
    /// <summary>
    ///     The instance of the object being pointed to, if it has been read.
    /// </summary>
    new T? Object { get; set; }

    IGameMakerSerializable? IGameMakerPointer.Object {
        get => Object!;
        set => Object = (T?)value;
    }
}
