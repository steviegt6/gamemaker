using Tomat.GameMaker.IFF.Chunks;
using Tomat.GameMaker.IFF.DataTypes.Models.Sprite;
using Tomat.GameMaker.IFF.DataTypes.Models.String;

namespace Tomat.GameMaker.IFF.DataTypes.Models.Room;

public sealed class GameMakerRoomLayerAssetInstance : IGameMakerSerializable {
    public GameMakerPointer<GameMakerString> Name { get; set; }

    public int AssetId { get; set; }

    public int X { get; set; }

    public int Y { get; set; }

    public float ScaleX { get; set; }

    public float ScaleY { get; set; }

    public int Color { get; set; }

    public float AnimationSpeed { get; set; }

    public GameMakerSpritePlaybackSpeedType PlaybackSpeedType { get; set; }

    public float FrameIndex { get; set; }

    public float Rotation { get; set; }

    public void Read(DeserializationContext context) {
        Name = context.ReadPointerAndObject<GameMakerString>();
        AssetId = context.ReadInt32();
        X = context.ReadInt32();
        Y = context.ReadInt32();
        ScaleX = context.ReadSingle();
        ScaleY = context.ReadSingle();
        Color = context.ReadInt32();
        AnimationSpeed = context.ReadSingle();
        PlaybackSpeedType = (GameMakerSpritePlaybackSpeedType)context.ReadInt32();
        FrameIndex = context.ReadSingle();
        Rotation = context.ReadSingle();
    }

    public void Write(SerializationContext context) {
        context.Write(Name);
        context.Write(AssetId);
        context.Write(X);
        context.Write(Y);
        context.Write(ScaleX);
        context.Write(ScaleY);
        context.Write(Color);
        context.Write(AnimationSpeed);
        context.Write((int)PlaybackSpeedType);
        context.Write(FrameIndex);
        context.Write(Rotation);
    }
}
