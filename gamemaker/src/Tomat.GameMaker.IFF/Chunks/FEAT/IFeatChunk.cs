using System.Collections.Generic;
using Tomat.GameMaker.IFF.DataTypes;
using Tomat.GameMaker.IFF.DataTypes.Models.String;

namespace Tomat.GameMaker.IFF.Chunks.FEAT;

public interface IFeatChunk : IGameMakerChunk {
    List<GameMakerPointer<GameMakerString>> FeatureFlags { get; set; }
}
