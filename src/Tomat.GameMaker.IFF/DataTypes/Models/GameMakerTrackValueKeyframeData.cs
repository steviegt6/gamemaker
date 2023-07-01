using Tomat.GameMaker.IFF.Chunks;

namespace Tomat.GameMaker.IFF.DataTypes.Models; 

public sealed class GameMakerTrackValueKeyframeData : IGameMakerSerializable {
    public int Value { get; set; }

    public void Read(DeserializationContext context) {
        Value = context.Reader.ReadInt32();
    }

    public void Write(SerializationContext context) {
        context.Writer.Write(Value);
    }
}
