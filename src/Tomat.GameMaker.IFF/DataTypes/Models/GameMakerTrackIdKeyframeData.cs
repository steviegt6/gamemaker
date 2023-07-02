using Tomat.GameMaker.IFF.Chunks;

namespace Tomat.GameMaker.IFF.DataTypes.Models;

public sealed class GameMakerTrackIdKeyframeData : IGameMakerSerializable {
    public int Id { get; set; }

    public void Read(DeserializationContext context) {
        Id = context.Reader.ReadInt32();
    }

    public void Write(SerializationContext context) {
        context.Writer.Write(Id);
    }
}
