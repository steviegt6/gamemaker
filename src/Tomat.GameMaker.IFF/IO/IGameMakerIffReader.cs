using System;
using System.Collections.Generic;
using Tomat.GameMaker.IFF.Chunks;
using Tomat.GameMaker.IFF.DataTypes;

namespace Tomat.GameMaker.IFF.IO;

public interface IGameMakerIffReader : IGameMakerIffDataHandler {
    /// <summary>
    ///     A map of pointer addresses to the objects they point to.
    /// </summary>
    Dictionary<int, IGameMakerSerializable> Pointers { get; }

    Memory<byte> ReadBytes(int count);

    char[] ReadChars(int count);

    byte ReadByte();

    bool ReadBoolean(bool wide);

    short ReadInt16();

    ushort ReadUInt16();

    Int24 ReadInt24();

    UInt24 ReadUInt24();

    int ReadInt32();

    uint ReadUInt32();

    long ReadInt64();

    ulong ReadUInt64();

    float ReadSingle();

    double ReadDouble();

    GameMakerPointer<T> ReadPointer<T>(int addr, bool useTypeOffset = true) where T : IGameMakerSerializable, new();
}

public static class GameMakerIffReaderExtensions {
    public static Guid ReadGuid(this IGameMakerIffReader reader) {
        return new Guid(reader.ReadBytes(16).Span);
    }

    public static void Pad(this IGameMakerIffReader reader, int align) {
        var pad = reader.Position % align;
        if (pad == 0)
            return;

        reader.Position += align - pad;
    }

    public static GameMakerPointer<T> ReadPointer<T>(this IGameMakerIffReader reader, bool useTypeOffset = true) where T : IGameMakerSerializable, new() {
        return reader.ReadPointer<T>(reader.ReadInt32(), useTypeOffset);
    }
}
