using System;
using Tomat.GameMaker.IFF.Chunks;

namespace Tomat.GameMaker.IFF.DataTypes.Models.Audio;

public sealed class GameMakerAudio : IGameMakerSerializable {
    public Memory<byte> Data { get; set; }

    public void Read(DeserializationContext context) {
        var len = context.ReadInt32();
        Data = context.ReadBytes(len);
    }

    public void Write(SerializationContext context) {
        context.Write(Data.Length);
        context.Write(Data);
    }
}
