﻿using Tomat.GameMaker.IFF.DataTypes;
using Tomat.GameMaker.IFF.DataTypes.Models.String;

namespace Tomat.GameMaker.IFF.Chunks.INST;

public interface IInstChunk : IGameMakerChunk {
    GameMakerPointerList<GameMakerString> InstanceNames { get; set; }
}