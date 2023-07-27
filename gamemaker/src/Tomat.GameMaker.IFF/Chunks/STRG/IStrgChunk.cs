﻿using Tomat.GameMaker.IFF.DataTypes;
using Tomat.GameMaker.IFF.DataTypes.Models.String;

namespace Tomat.GameMaker.IFF.Chunks.STRG;

public interface IStrgChunk : IGameMakerChunk {
    GameMakerPointerList<GameMakerString> Strings { get; set; }
}
