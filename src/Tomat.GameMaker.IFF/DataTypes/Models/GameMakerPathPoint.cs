using Tomat.GameMaker.IFF.Chunks;

namespace Tomat.GameMaker.IFF.DataTypes.Models; 

public sealed class GameMakerPathPoint : IGameMakerSerializable {
    public float X { get; set; }

    public float Y { get; set; }

    public float Speed { get; set; }

    public void Read(DeserializationContext context) {
        X = context.Reader.ReadSingle();
        Y = context.Reader.ReadSingle();
        Speed = context.Reader.ReadSingle();
    }

    public void Write(SerializationContext context) {
        context.Writer.Write(X);
        context.Writer.Write(Y);
        context.Writer.Write(Speed);
    }
}
