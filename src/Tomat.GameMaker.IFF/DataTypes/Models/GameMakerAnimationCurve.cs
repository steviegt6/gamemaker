using Tomat.GameMaker.IFF.Chunks;

namespace Tomat.GameMaker.IFF.DataTypes.Models;

public sealed class GameMakerAnimationCurve : IGameMakerSerializable {
    public GameMakerPointer<GameMakerString> Name { get; set; }

    public GameMakerAnimationCurveGraphType GraphType { get; set; }

    public GameMakerList<GameMakerChannel>? Channels { get; set; }

    private readonly bool includeName;

    public GameMakerAnimationCurve() {
        includeName = true;
    }

    public GameMakerAnimationCurve(bool includeName) {
        this.includeName = includeName;
    }

    public void Read(DeserializationContext context) {
        Name = includeName ? context.ReadPointerAndObject<GameMakerString>() : GameMakerPointer<GameMakerString>.NULL;
        GraphType = (GameMakerAnimationCurveGraphType)context.Reader.ReadUInt32();

        Channels = new GameMakerList<GameMakerChannel>();
        Channels.Read(context);
    }

    public void Write(SerializationContext context) {
        if (includeName)
            context.Writer.Write(Name);
        context.Writer.Write((uint)GraphType);
        Channels!.Write(context);
    }
}
