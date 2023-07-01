﻿using Tomat.GameMaker.IFF.Chunks;

namespace Tomat.GameMaker.IFF.DataTypes.Models;

public sealed class GameMakerConstant : IGameMakerSerializable {
    public GameMakerPointer<GameMakerString> Name { get; set; }

    public GameMakerPointer<GameMakerString> Value { get; set; }

    public void Read(DeserializationContext context) {
        Name = context.ReadPointerAndObject<GameMakerString>(context.Reader.ReadInt32(), returnAfter: true);
        Value = context.ReadPointerAndObject<GameMakerString>(context.Reader.ReadInt32(), returnAfter: true);
    }

    public void Write(SerializationContext context) {
        context.Writer.Write(Name);
        context.Writer.Write(Value);
    }
}
