using System;
using Tomat.GameMaker.IFF.Chunks;

namespace Tomat.GameMaker.IFF.DataTypes.Models;

public sealed class GameMakerAudio : IGameMakerSerializable {
    public Memory<byte> Data { get; set; }

    public void Read(DeserializationContext context) {
        var len = context.Reader.ReadInt32();
        Data = context.Reader.ReadBytes(len);
    }

    public void Write(SerializationContext context) {
        context.Writer.Write(Data.Length);
        context.Writer.Write(Data);
    }
}
