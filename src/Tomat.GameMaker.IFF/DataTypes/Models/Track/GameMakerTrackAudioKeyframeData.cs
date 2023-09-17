namespace Tomat.GameMaker.IFF.DataTypes.Models.Track;

public sealed class GameMakerTrackAudioKeyframeData : IGameMakerSerializable {
    public int Id { get; set; }

    public int UnknownInt32 { get; set; } // Expected to be zero.

    public int Mode { get; set; }

    public void Read(DeserializationContext context) {
        Id = context.ReadInt32();
        UnknownInt32 = context.ReadInt32();
        Mode = context.ReadInt32();
    }

    public void Write(SerializationContext context) {
        context.Write(Id);
        context.Write(UnknownInt32);
        context.Write(Mode);
    }
}
