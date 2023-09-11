using Tomat.GameMaker.IFF.DataTypes.Models.Timeline;

namespace Tomat.GameMaker.IFF.Chunks.TMLN;

internal sealed class GameMakerTmlnChunk : AbstractChunk,
                                           ITmlnChunk {
    public const string NAME = "TMLN";

    public GameMakerPointerList<GameMakerTimeline> Timelines { get; set; } = null!;

    public GameMakerTmlnChunk(string name, int size, int startPosition) : base(name, size, startPosition) { }

    public override void Read(DeserializationContext context) {
        Timelines = context.ReadPointerList<GameMakerTimeline>();
    }

    public override void Write(SerializationContext context) {
        context.Write(Timelines);
    }
}
