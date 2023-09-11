using System.Collections.Generic;
using Tomat.GameMaker.IFF.Chunks;

namespace Tomat.GameMaker.IFF.DataTypes.Models.AnimationCurve;

// model GMAnimationCurve {
//     int32 graphType;
//     int32 numChannels;
//     array {
//         GMAnimCurveChannel
//     } channels;
// }

public interface IAnimationCurve : IGameMakerSerializable {
    AnimationCurveGraphType GraphType { get; set; }

    List<IAnimationCurveChannel> Channels { get; }
}

internal sealed class GameMakerAnimationCurve : IAnimationCurve {
    public AnimationCurveGraphType GraphType { get; set; }

    public List<IAnimationCurveChannel> Channels { get; set; } = null!;

    public void Read(DeserializationContext context) {
        GraphType = (AnimationCurveGraphType)context.ReadInt32();
        Channels = context.ReadList<IAnimationCurveChannel, GameMakerAnimationCurveChannel>();
    }

    public void Write(SerializationContext context) {
        context.Write((int)GraphType);
        context.WriteList(Channels);
    }
}
