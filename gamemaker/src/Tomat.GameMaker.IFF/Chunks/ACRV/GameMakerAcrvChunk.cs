﻿using System.IO;
using Tomat.GameMaker.IFF.DataTypes;
using Tomat.GameMaker.IFF.DataTypes.Models.AnimationCurve;

namespace Tomat.GameMaker.IFF.Chunks.ACRV;

public sealed class GameMakerAcrvChunk : AbstractChunk {
    public const string NAME = "ACRV";

    public GameMakerPointerList<GameMakerAnimationCurve> AnimationCurves { get; set; } = null!;

    public GameMakerAcrvChunk(string name, int size) : base(name, size) { }

    public override void Read(DeserializationContext context) {
        var chunkVersion = context.ReadInt32();
        if (chunkVersion != 1)
            throw new InvalidDataException($"Expected chunk version 1, got {chunkVersion}.");

        AnimationCurves = context.ReadPointerList<GameMakerAnimationCurve>();
    }

    public override void Write(SerializationContext context) {
        context.Write(1);
        context.Write(AnimationCurves);
    }
}