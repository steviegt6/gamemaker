using Tomat.GameMaker.IFF.Chunks;

namespace Tomat.GameMaker.IFF.DataTypes.Models.Object; 

public sealed class GameMakerObjectPhysicsVertex : IGameMakerSerializable {
    public float X { get; set; }

    public float Y { get; set; }

    public void Read(DeserializationContext context) {
        X = context.Reader.ReadSingle();
        Y = context.Reader.ReadSingle();
    }

    public void Write(SerializationContext context) {
        context.Writer.Write(X);
        context.Writer.Write(Y);
    }
}
