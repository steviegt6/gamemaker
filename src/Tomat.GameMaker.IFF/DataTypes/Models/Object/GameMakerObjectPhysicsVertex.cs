namespace Tomat.GameMaker.IFF.DataTypes.Models.Object; 

public sealed class GameMakerObjectPhysicsVertex : IGameMakerSerializable {
    public float X { get; set; }

    public float Y { get; set; }

    public void Read(DeserializationContext context) {
        X = context.ReadSingle();
        Y = context.ReadSingle();
    }

    public void Write(SerializationContext context) {
        context.Write(X);
        context.Write(Y);
    }
}
