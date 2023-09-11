using Tomat.GameMaker.IFF.Chunks;
using Tomat.GameMaker.IFF.DataTypes.Models.String;

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

public sealed class GameMakerAnimationCurve : IGameMakerSerializable {
    // public GameMakerPointer<GameMakerString> Name { get; set; }

    public GameMakerAnimationCurveGraphType GraphType { get; set; }

    public GameMakerList<GameMakerAnimationCurveChannel> Channels { get; set; } = null!;

    // private readonly bool includeName;

    /*public GameMakerAnimationCurve() : this(true) { }

    public GameMakerAnimationCurve(bool includeName) {
        this.includeName = includeName;
    }*/

    public void Read(DeserializationContext context) {
        //Name = includeName ? context.ReadPointerAndObject<GameMakerString>() : GameMakerPointer<GameMakerString>.NULL;

        GraphType = (GameMakerAnimationCurveGraphType)context.ReadInt32();
        Channels = context.ReadList<GameMakerAnimationCurveChannel>();
    }

    public void Write(SerializationContext context) {
        // if (includeName)
        //     context.Write(Name);

        context.Write((int)GraphType);
        context.Write(Channels);
    }
}
