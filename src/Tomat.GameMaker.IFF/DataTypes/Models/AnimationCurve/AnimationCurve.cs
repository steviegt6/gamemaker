using System.Collections.Generic;
using Tomat.GameMaker.IFF.Chunks;

namespace Tomat.GameMaker.IFF.DataTypes.Models.AnimationCurve; 

/* model GMAnimationCurve {
 * #if false // TODO: I don't believe names are ever actually included...
 *     string* name;
 * #endif
 *     int32 graphType;
 *     int32 channelCount;
 *     GMAnimCurveChannel[] channels;
 * }
 */

public interface IAnimationCurve : IGameMakerSerializable {
    // GameMakerPointer<GameMakerString> Name { get; set; }

    AnimationCurveGraphType GraphType { get; set; }

    int ChannelCount { get; }

    List<IAnimationCurveChannel> Channels { get; }
}

internal sealed class GameMakerAnimationCurve : IAnimationCurve {
    // public GameMakerPointer<GameMakerString> Name { get; set; }

    public AnimationCurveGraphType GraphType { get; set; }

    public int ChannelCount => Channels.Count;

    public List<IAnimationCurveChannel> Channels { get; set; } = null!;

    // private readonly bool includeName;

    /*public GameMakerAnimationCurve() : this(true) { }

    public GameMakerAnimationCurve(bool includeName) {
        this.includeName = includeName;
    }*/

    public void Read(DeserializationContext context) {
        //Name = includeName ? context.ReadPointerAndObject<GameMakerString>() : GameMakerPointer<GameMakerString>.NULL;

        GraphType = (AnimationCurveGraphType)context.ReadInt32();
        Channels = context.ReadList<IAnimationCurveChannel, GameMakerAnimationCurveChannel>();
    }

    public void Write(SerializationContext context) {
        // if (includeName)
        //     context.Write(Name);

        context.Write((int)GraphType);
        context.WriteList(Channels);
    }
}
