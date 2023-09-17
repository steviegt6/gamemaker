using System.Collections.Generic;
using Tomat.GameMaker.IFF.DataTypes;
using Tomat.GameMaker.IFF.DataTypes.Models;
using Tomat.GameMaker.IFF.DataTypes.Models.AssetTag;

namespace Tomat.GameMaker.IFF.Chunks.TAGS;

public interface ITagsChunk : IGameMakerChunk {
    public int ChunkVersion { get; set; }

    public List<GameMakerPointer<GameMakerString>> Tags { get; set; }

    public GameMakerPointerList<GameMakerAssetTag> AssetTags { get; set; }
}
