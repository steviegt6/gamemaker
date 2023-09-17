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
        Visible = context.ReadBoolean(wide: true);
        Foreground = context.ReadBoolean(wide: true);
        SpriteId = context.ReadInt32();
        TileHorizontal = context.ReadBoolean(wide: true);
        TileVertical = context.ReadBoolean(wide: true);
        Stretch = context.ReadBoolean(wide: true);
        Color = context.ReadInt32();
        FirstFrame = context.ReadSingle();
        AnimationSpeed = context.ReadSingle();
        PlaybackSpeedType = (GameMakerSpritePlaybackSpeedType)context.ReadInt32();
    }

    public void Write(SerializationContext context) {
        context.Write(Visible, wide: true);
        context.Write(Foreground, wide: true);
        context.Write(SpriteId);
        context.Write(TileHorizontal, wide: true);
        context.Write(TileVertical, wide: true);
        context.Write(Stretch, wide: true);
        context.Write(Color);
        context.Write(FirstFrame);
        context.Write(AnimationSpeed);
        context.Write((int)PlaybackSpeedType);
    }
}
