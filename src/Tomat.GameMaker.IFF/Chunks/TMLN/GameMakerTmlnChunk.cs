using Tomat.GameMaker.IFF.DataTypes;
using Tomat.GameMaker.IFF.DataTypes.Models;
using Tomat.GameMaker.IFF.DataTypes.Models.Timeline;

namespace Tomat.GameMaker.IFF.Chunks.TMLN; 

public sealed class GameMakerTmlnChunk : AbstractChunk {
    public const string NAME = "TMLN";
    
    public GameMakerPointerList<GameMakerTimeline>? Timelines { get; set; }

    public GameMakerTmlnChunk(string name, int size) : base(name, size) { }

    public override void Read(DeserializationContext context) {
        Timelines = new GameMakerPointerList<GameMakerTimeline>();
        Timelines.Read(context);
    }

    public override void Write(SerializationContext context) {
        Timelines!.Write(context);
    }
}
