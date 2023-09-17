using Tomat.GameMaker.IFF.DataTypes.Models.ParticleSystem;

namespace Tomat.GameMaker.IFF.DataTypes.Models.Room;

public sealed class GameMakerRoomParticleSystem : IGameMakerSerializable {
    public GameMakerPointer<GameMakerString> Name { get; set; }

    public GameMakerPointer<GameMakerParticleSystem> ParticleSystem { get; set; }

    public int X { get; set; }

    public int Y { get; set; }

    public float ScaleX { get; set; }

    public float ScaleY { get; set; }

    public uint Color { get; set; }

    public float Rotation { get; set; }

    public void Read(DeserializationContext context) {
        Name = context.ReadPointerAndObject<GameMakerString>();
        ParticleSystem = context.ReadPointerAndObject<GameMakerParticleSystem>();
        X = context.ReadInt32();
        Y = context.ReadInt32();
        ScaleX = context.ReadSingle();
        ScaleY = context.ReadSingle();
        Color = context.ReadUInt32();
        Rotation = context.ReadSingle();
    }

    public void Write(SerializationContext context) {
        context.Write(Name);
        context.Write(ParticleSystem);
        context.Write(X);
        context.Write(Y);
        context.Write(ScaleX);
        context.Write(ScaleY);
        context.Write(Color);
        context.Write(Rotation);
    }
}
