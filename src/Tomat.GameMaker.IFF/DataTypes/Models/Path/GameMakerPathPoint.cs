namespace Tomat.GameMaker.IFF.DataTypes.Models.Path; 

public sealed class GameMakerPathPoint : IGameMakerSerializable {
    public float X { get; set; }

    public float Y { get; set; }

    public float Speed { get; set; }

    public void Read(DeserializationContext context) {
        X = context.ReadSingle();
        Y = context.ReadSingle();
        Speed = context.ReadSingle();
    }

    public void Write(SerializationContext context) {
        context.Write(X);
        context.Write(Y);
        context.Write(Speed);
    }
}
