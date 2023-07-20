using Tomat.GameMaker.IFF.Chunks;
using Tomat.GameMaker.IFF.DataTypes.Models.Sequence.Keyframes;
using Tomat.GameMaker.IFF.DataTypes.Models.String;

namespace Tomat.GameMaker.IFF.DataTypes.Models.Track;

public sealed class GameMakerTrackStringKeyframes : GameMakerTrackKeyframes {
    public GameMakerList<GameMakerKeyframe<GameMakerString>> Strings { get; set; } = null!;

    public override void Read(DeserializationContext context) {
        Strings = context.ReadList<GameMakerKeyframe<GameMakerString>>();
    }

    public override void Write(SerializationContext context) {
        context.Write(Strings);
    }
}
