using Tomat.GameMaker.IFF.DataTypes;
using Tomat.GameMaker.IFF.DataTypes.Models.AnimationCurve;

namespace Tomat.GameMaker.IFF.Chunks.ARV;

public sealed class GameMakerAcrvChunk : AbstractChunk {
    public const string NAME = "ACRV";

    public GameMakerPointerList<GameMakerAnimationCurve>? AnimationCurves { get; set; }

    public GameMakerAcrvChunk(string name, int size) : base(name, size) { }

    public override void Read(DeserializationContext context) {
        AnimationCurves = new GameMakerPointerList<GameMakerAnimationCurve>();
        AnimationCurves.Read(context);
    }

    public override void Write(SerializationContext context) {
        AnimationCurves!.Write(context);
    }
}
