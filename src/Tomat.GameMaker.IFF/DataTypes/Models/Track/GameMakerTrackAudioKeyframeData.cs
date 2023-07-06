using Tomat.GameMaker.IFF.Chunks;

namespace Tomat.GameMaker.IFF.DataTypes.Models.Track;

public sealed class GameMakerTrackAudioKeyframeData : IGameMakerSerializable {
    public int Id { get; set; }

    public int UnknownInt32 { get; set; } // Expected to be zero.

    public int Mode { get; set; }

    public void Read(DeserializationContext context) {
        Id = context.Reader.ReadInt32();
        UnknownInt32 = context.Reader.ReadInt32();
        Mode = context.Reader.ReadInt32();
    }

    public void Write(SerializationContext context) {
        context.Writer.Write(Id);
        context.Writer.Write(UnknownInt32);
        context.Writer.Write(Mode);
    }
}
