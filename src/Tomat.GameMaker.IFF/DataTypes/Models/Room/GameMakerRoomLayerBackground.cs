using Tomat.GameMaker.IFF.Chunks;
using Tomat.GameMaker.IFF.DataTypes.Models.Sprite;

namespace Tomat.GameMaker.IFF.DataTypes.Models.Room;

public sealed class GameMakerRoomLayerBackground : IGameMakerSerializable {
    public bool Visible { get; set; }

    public bool Foreground { get; set; }

    public int SpriteId { get; set; }

    public bool TileHorizontal { get; set; }

    public bool TileVertical { get; set; }

    public bool Stretch { get; set; }

    public int Color { get; set; }

    public float FirstFrame { get; set; }

    public float AnimationSpeed { get; set; }

    public GameMakerSpritePlaybackSpeedType PlaybackSpeedType { get; set; }

    public void Read(DeserializationContext context) {
        Visible = context.Reader.ReadBoolean(wide: true);
        Foreground = context.Reader.ReadBoolean(wide: true);
        SpriteId = context.Reader.ReadInt32();
        TileHorizontal = context.Reader.ReadBoolean(wide: true);
        TileVertical = context.Reader.ReadBoolean(wide: true);
        Stretch = context.Reader.ReadBoolean(wide: true);
        Color = context.Reader.ReadInt32();
        FirstFrame = context.Reader.ReadSingle();
        AnimationSpeed = context.Reader.ReadSingle();
        PlaybackSpeedType = (GameMakerSpritePlaybackSpeedType)context.Reader.ReadInt32();
    }

    public void Write(SerializationContext context) {
        context.Writer.Write(Visible, wide: true);
        context.Writer.Write(Foreground, wide: true);
        context.Writer.Write(SpriteId);
        context.Writer.Write(TileHorizontal, wide: true);
        context.Writer.Write(TileVertical, wide: true);
        context.Writer.Write(Stretch, wide: true);
        context.Writer.Write(Color);
        context.Writer.Write(FirstFrame);
        context.Writer.Write(AnimationSpeed);
        context.Writer.Write((int)PlaybackSpeedType);
    }
}
