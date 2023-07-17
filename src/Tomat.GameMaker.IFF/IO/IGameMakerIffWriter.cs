using System;
using System.Collections.Generic;
using Tomat.GameMaker.IFF.Chunks;
using Tomat.GameMaker.IFF.DataTypes;

namespace Tomat.GameMaker.IFF.IO;

public interface IGameMakerIffWriter : IGameMakerIffDataHandler {
    /// <summary>
    ///     A map between objects and the addresses in which they are written
    ///     to.
    ///     <br />
    ///     The objects are the values that pointers point to.
    /// </summary>
    Dictionary<IGameMakerSerializable, int> Pointers { get; }

    /// <summary>
    ///     A map between objects and a list of addresses in which they are
    ///     referenced.
    ///     <br />
    ///     The objects are the values that pointers point to.
    /// </summary>
    Dictionary<IGameMakerSerializable, List<(int, bool)>> PointerReferences { get; }

    void Write(Memory<byte> value);

    void Write(byte[] value);

    void Write(char[] value);

    void Write(byte value);

    void Write(bool value, bool wide);

    void Write(short value);

    void Write(ushort value);

    void Write(Int24 value);

    void Write(UInt24 value);

    void Write(int value);

    void Write(uint value);

    void Write(long value);

    void Write(ulong value);

    void Write(float value);

    void Write(double value);

    void Write<T>(GameMakerPointer<T> ptr, bool useTypeOffset = true) where T : IGameMakerSerializable, new();

    void FinalizePointers();
}

public static class GameMakerIffWriterExtensions {
    /// <summary>
    ///     Pads the writer's position to the specified alignment.
    /// </summary>
    /// <param name="writer">The writer to pad.</param>
    /// <param name="align">The alignment to align to.</param>
    public static void Pad(this IGameMakerIffWriter writer, int align) {
        var pad = writer.Position % align;
        if (pad == 0)
            return;

        writer.Write(new byte[align - pad]);
    }

    public static void WriteAt(this IGameMakerIffWriter writer, int position, int value) {
        var oldPos = writer.Position;
        writer.Position = position;
        writer.Write(value);
        writer.Position = oldPos;
    }

    public static int BeginLength(this IGameMakerIffWriter writer) {
        writer.Write(0);
        return writer.Position;
    }

    public static int EndLength(this IGameMakerIffWriter writer, int beginPos) {
        var pos = writer.Position;
        writer.Position = beginPos - 4;
        var length = pos - beginPos;
        writer.Write(length);
        writer.Position = pos;
        return length;
    }
}
