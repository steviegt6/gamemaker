using Tomat.GameMaker.IFF.DataTypes.Models.Timeline;

namespace Tomat.GameMaker.IFF.Chunks.TMLN;

public interface ITmlnChunk : IGameMakerChunk {
    GameMakerPointerList<GameMakerTimeline> Timelines { get; set; }
}
