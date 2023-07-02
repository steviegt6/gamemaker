using Tomat.GameMaker.IFF.Chunks;

namespace Tomat.GameMaker.IFF.DataTypes.Models;

public sealed class GameMakerTrackAudioKeyframes : GameMakerTrackKeyframes {
    public GameMakerList<GameMakerKeyframe<GameMakerTrackAudioKeyframeData>>? Keyframes { get; set; }

    public override void Read(DeserializationContext context) {
        Keyframes = new GameMakerList<GameMakerKeyframe<GameMakerTrackAudioKeyframeData>>();
        Keyframes.Read(context);
    }

    public override void Write(SerializationContext context) {
        Keyframes!.Write(context);
    }
}
