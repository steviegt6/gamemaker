﻿using System.Collections.Generic;
using Tomat.GameMaker.IFF.DataTypes;
using Tomat.GameMaker.IFF.DataTypes.Models;
using Tomat.GameMaker.IFF.DataTypes.Models.String;
using Tomat.GameMaker.IFF.IO;

namespace Tomat.GameMaker.IFF.Chunks.FEAT;

public sealed class GameMakerFeatChunk : AbstractChunk {
    public const string NAME = "FEAT";

    public List<GameMakerPointer<GameMakerString>>? FeatureFlags { get; set; }

    public GameMakerFeatChunk(string name, int size) : base(name, size) { }

    public override void Read(DeserializationContext context) {
        context.Pad(4);

        var count = context.ReadInt32();
        FeatureFlags = new List<GameMakerPointer<GameMakerString>>(count);
        for (var i = 0; i < count; i++)
            FeatureFlags.Add(context.ReadPointerAndObject<GameMakerString>());
    }

    public override void Write(SerializationContext context) {
        context.Pad(4);

        context.Write(FeatureFlags!.Count);
        foreach (var flag in FeatureFlags)
            context.Write(flag);
    }
}
