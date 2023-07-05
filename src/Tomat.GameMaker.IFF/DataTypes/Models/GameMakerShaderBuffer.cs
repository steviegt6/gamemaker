﻿using System;
using Tomat.GameMaker.IFF.Chunks;

namespace Tomat.GameMaker.IFF.DataTypes.Models;

public sealed class GameMakerShaderBuffer : IGameMakerSerializable {
    public Memory<byte> Buffer { get; set; }

    public void Read(DeserializationContext context, int length) {
        Buffer = context.Reader.ReadBytes(length);
    }

    public void Write(SerializationContext context) {
        context.Writer.Write(Buffer);
    }

    void IGameMakerSerializable.Read(DeserializationContext context) {
        throw new InvalidOperationException("Attempted to write shader buffer without a specified length.");
    }
}
