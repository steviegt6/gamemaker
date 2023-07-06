using System.Collections.Generic;
using Tomat.GameMaker.IFF.Chunks;
using Tomat.GameMaker.IFF.DataTypes.Models.Sequence.Keyframes;
using Tomat.GameMaker.IFF.DataTypes.Models.Sprite;
using Tomat.GameMaker.IFF.DataTypes.Models.String;
using Tomat.GameMaker.IFF.DataTypes.Models.Track;

namespace Tomat.GameMaker.IFF.DataTypes.Models.Sequence;

public sealed class GameMakerSequence : IGameMakerSerializable {
    public GameMakerPointer<GameMakerString> Name { get; set; }

    public GameMakerSequencePlaybackType PlaybackType { get; set; }

    public float PlaybackSpeed { get; set; }

    public GameMakerSpritePlaybackSpeedType PlaybackSpeedType { get; set; }

    public float Length { get; set; }

    public int OriginX { get; set; }

    public int OriginY { get; set; }

    public float Volume { get; set; }

    public GameMakerList<GameMakerKeyframe<GameMakerBroadcastMessage>>? BroadcastMessages { get; set; }

    public GameMakerList<GameMakerTrack>? Tracks { get; set; }

    public Dictionary<int, GameMakerPointer<GameMakerString>>? FunctionIds { get; set; }

    public GameMakerList<GameMakerKeyframe<GameMakerMoment>>? Moments { get; set; }

    public void Read(DeserializationContext context) {
        Name = context.ReadPointerAndObject<GameMakerString>();
        PlaybackType = (GameMakerSequencePlaybackType)context.Reader.ReadUInt32();
        PlaybackSpeed = context.Reader.ReadSingle();
        PlaybackSpeedType = (GameMakerSpritePlaybackSpeedType)context.Reader.ReadInt32();
        Length = context.Reader.ReadSingle();
        OriginX = context.Reader.ReadInt32();
        OriginY = context.Reader.ReadInt32();
        Volume = context.Reader.ReadSingle();

        BroadcastMessages = new GameMakerList<GameMakerKeyframe<GameMakerBroadcastMessage>>();
        BroadcastMessages.Read(context);

        Tracks = new GameMakerList<GameMakerTrack>();
        Tracks.Read(context);

        var functionIdCount = context.Reader.ReadInt32();
        FunctionIds = new Dictionary<int, GameMakerPointer<GameMakerString>>(functionIdCount);

        for (var i = 0; i < functionIdCount; i++) {
            var functionId = context.Reader.ReadInt32();
            var functionName = context.ReadPointerAndObject<GameMakerString>();
            FunctionIds.Add(functionId, functionName);
        }

        Moments = new GameMakerList<GameMakerKeyframe<GameMakerMoment>>();
        Moments.Read(context);
    }

    public void Write(SerializationContext context) {
        context.Writer.Write(Name);
        context.Writer.Write((uint)PlaybackType);
        context.Writer.Write(PlaybackSpeed);
        context.Writer.Write((int)PlaybackSpeedType);
        context.Writer.Write(Length);
        context.Writer.Write(OriginX);
        context.Writer.Write(OriginY);
        context.Writer.Write(Volume);

        BroadcastMessages!.Write(context);

        Tracks!.Write(context);

        context.Writer.Write(FunctionIds!.Count);

        foreach (var (functionId, functionName) in FunctionIds) {
            context.Writer.Write(functionId);
            context.Writer.Write(functionName);
        }

        Moments!.Write(context);
    }
}
