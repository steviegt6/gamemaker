namespace Tomat.GameMaker.IFF.DataTypes.Models.Track;

public sealed class GameMakerTrackValueKeyframeData : IGameMakerSerializable {
    public int Value { get; set; }

    public void Read(DeserializationContext context) {
        Value = context.ReadInt32();
    }

    public void Write(SerializationContext context) {
        context.Write(Value);
    }
}
