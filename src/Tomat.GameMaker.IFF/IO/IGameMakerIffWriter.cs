using System;
using System.Collections.Generic;
using Tomat.GameMaker.IFF.Chunks;
using Tomat.GameMaker.IFF.DataTypes;

namespace Tomat.GameMaker.IFF.IO; 

public interface IGameMakerIffWriter : IGameMakerIffDataHandler {


    Dictionary<IGameMakerSerializable, int> Pointers { get; }

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

    public static void EndLength(this IGameMakerIffWriter writer, int beginPos) {
        var pos = writer.Position;
        writer.Position = beginPos - 4;
        writer.Write(pos - beginPos);
        writer.Position = pos;
    }
}
