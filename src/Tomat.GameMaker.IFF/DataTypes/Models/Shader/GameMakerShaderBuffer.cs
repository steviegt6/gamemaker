using System;

namespace Tomat.GameMaker.IFF.DataTypes.Models.Shader;

public sealed class GameMakerShaderBuffer : IGameMakerSerializable {
    public Memory<byte> Buffer { get; set; }

    public void Read(DeserializationContext context, int length) {
        Buffer = context.ReadBytes(length);
    }

    public void Write(SerializationContext context) {
        context.Write(Buffer);
    }

    void IGameMakerSerializable.Read(DeserializationContext context) {
        throw new InvalidOperationException("Attempted to write shader buffer without a specified length.");
    }
}
