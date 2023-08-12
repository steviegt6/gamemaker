using Tomat.GameMaker.IFF.DataTypes;
using Tomat.GameMaker.IFF.DataTypes.Models.TextureGroupInfo;

namespace Tomat.GameMaker.IFF.Chunks.TGIN;

public interface ITginChunk : IGameMakerChunk {
    int ChunkVersion { get; set; }

    GameMakerPointerList<GameMakerTextureGroupInfo> TextureGroupInfos { get; set; }
}
