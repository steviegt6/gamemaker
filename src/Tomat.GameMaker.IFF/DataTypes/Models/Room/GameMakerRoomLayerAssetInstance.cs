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
        AssetId = context.Reader.ReadInt32();
        X = context.Reader.ReadInt32();
        Y = context.Reader.ReadInt32();
        ScaleX = context.Reader.ReadSingle();
        ScaleY = context.Reader.ReadSingle();
        Color = context.Reader.ReadInt32();
        AnimationSpeed = context.Reader.ReadSingle();
        PlaybackSpeedType = (GameMakerSpritePlaybackSpeedType)context.Reader.ReadInt32();
        FrameIndex = context.Reader.ReadSingle();
        Rotation = context.Reader.ReadSingle();
    }

    public void Write(SerializationContext context) {
        context.Writer.Write(Name);
        context.Writer.Write(AssetId);
        context.Writer.Write(X);
        context.Writer.Write(Y);
        context.Writer.Write(ScaleX);
        context.Writer.Write(ScaleY);
        context.Writer.Write(Color);
        context.Writer.Write(AnimationSpeed);
        context.Writer.Write((int)PlaybackSpeedType);
        context.Writer.Write(FrameIndex);
        context.Writer.Write(Rotation);
    }
}
