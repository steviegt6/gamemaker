using System;
using System.Diagnostics.CodeAnalysis;
using Tomat.GameMaker.IFF.DataTypes.Models;
using Tomat.GameMaker.IFF.IO;

namespace Tomat.GameMaker.IFF.DataTypes;

/// <summary>
///     A pointer to a GameMaker object, storing an integer address to the
///     position of the object in the GameMaker IFF file, as well as (possibly)
///     a reference to the object itself (set by <see cref="ReadPointerObject"/>).
/// </summary>
/// <typeparam name="TInterface">The GameMaker object type.</typeparam>
public struct GameMakerPointer<TInterface> where TInterface : IGameMakerSerializable {
    public static readonly GameMakerPointer<TInterface> NULL = new();

    /// <summary>
    ///     The address of the object in the GameMaker IFF file.
    /// </summary>
    public int Address { get; set; }

    /// <summary>
    ///     The instance of the object being pointed to, if
    ///     <see cref="ReadPointerObject"/> has been called.
    /// </summary>
    private TInterface? ptrObject;

    /// <summary>
    ///     Whether the pointer is null.
    /// </summary>
    public bool IsNull => Address == 0;

    /// <summary>
    ///     Gets or initializes the object that this pointer points to.
    /// </summary>
    /// <param name="reader">The reader.</param>
    /// <returns>
    ///     The pointer object instance, which will be <see langword="null"/> if
    ///     the pointer <see cref="IsNull"/>.
    /// </returns>
    public TInterface? GetOrInitializePointerObject<TImplementation>(IGameMakerIffReader reader) where TImplementation : TInterface, new() {
        if (IsNull) {
            ptrObject = default;
            return ptrObject;
        }

        if (reader.Pointers.TryGetValue(Address, out var obj)) {
            ptrObject = (TInterface)obj;
        }
        else {
            ptrObject = new TImplementation();
            reader.Pointers[Address] = ptrObject;
        }

        return ptrObject;
    }

    /// <summary>
    ///     Reads the object that this pointer points to.
    /// </summary>
    /// <param name="context">The deserialization context.</param>
    /// <param name="returnAfter">
    ///     Whether to return to the original position after reading.
    /// </param>
    public void ReadPointerObject<TImplementation>(DeserializationContext context, bool returnAfter) where TImplementation : TInterface, new() {
        if (IsNull)
            return;

        var obj = GetOrInitializePointerObject<TImplementation>(context);

        if (obj is null)
            throw new InvalidOperationException("Pointer object was null despite pointer not being null!");

        var pos = context.Position;
        context.Position = Address;

        obj.Read(context);

        if (returnAfter)
            context.Position = pos;
    }

    public void WriteObject(SerializationContext context) {
        if (IsNull)
            throw new InvalidOperationException("Cannot write null object.");

        if (ptrObject is null)
            throw new InvalidOperationException("Pointer object was null despite pointer not being null!");

        context.Pointers[ptrObject] = context.Position;
    }

    public bool TryGetObject([NotNullWhen(returnValue: true)] out TInterface? obj) {
        obj = ptrObject;
        return !IsNull;
    }

    public TInterface ExpectObject() {
        if (TryGetObject(out var obj))
            return obj;

        throw new InvalidOperationException("Pointer is null.");
    }

    public override string? ToString() {
        return ptrObject is null ? "<null object>" : ptrObject.ToString();
    }
}

public static class GameMakerPointerExtensions {
    public static int GetPointerOffset(Type type) {
        return type == typeof(GameMakerString) ? 4 : 0;
    }
}
