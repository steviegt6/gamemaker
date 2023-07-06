using Tomat.GameMaker.IFF.Chunks;
using Tomat.GameMaker.IFF.DataTypes.Models.Sequence.Keyframes;
using Tomat.GameMaker.IFF.DataTypes.Models.String;

namespace Tomat.GameMaker.IFF.DataTypes.Models.Track;

public sealed class GameMakerTrackStringKeyframes : GameMakerTrackKeyframes {
    public GameMakerList<GameMakerKeyframe<GameMakerString>>? Strings { get; set; }

    public override void Read(DeserializationContext context) {
        Strings = new GameMakerList<GameMakerKeyframe<GameMakerString>>();
        Strings.Read(context);
    }

    public override void Write(SerializationContext context) {
        Strings!.Write(context);
    }
}
