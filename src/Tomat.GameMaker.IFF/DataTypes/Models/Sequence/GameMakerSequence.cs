using System.Collections.Generic;
using Tomat.GameMaker.IFF.DataTypes.Models.Sequence.Keyframes;
using Tomat.GameMaker.IFF.DataTypes.Models.Sprite;
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

    public GameMakerList<GameMakerKeyframe<GameMakerBroadcastMessage>> BroadcastMessages { get; set; } = null!;

    public GameMakerList<GameMakerTrack> Tracks { get; set; } = null!;

    public Dictionary<int, GameMakerPointer<GameMakerString>>? FunctionIds { get; set; }

    public GameMakerList<GameMakerKeyframe<GameMakerMoment>> Moments { get; set; } = null!;

    public void Read(DeserializationContext context) {
        Name = context.ReadPointerAndObject<GameMakerString>();
        PlaybackType = (GameMakerSequencePlaybackType)context.ReadUInt32();
        PlaybackSpeed = context.ReadSingle();
        PlaybackSpeedType = (GameMakerSpritePlaybackSpeedType)context.ReadInt32();
        Length = context.ReadSingle();
        OriginX = context.ReadInt32();
        OriginY = context.ReadInt32();
        Volume = context.ReadSingle();

        BroadcastMessages = context.ReadList<GameMakerKeyframe<GameMakerBroadcastMessage>>();
        Tracks = context.ReadList<GameMakerTrack>();

        var functionIdCount = context.ReadInt32();
        FunctionIds = new Dictionary<int, GameMakerPointer<GameMakerString>>(functionIdCount);

        for (var i = 0; i < functionIdCount; i++) {
            var functionId = context.ReadInt32();
            var functionName = context.ReadPointerAndObject<GameMakerString>();
            FunctionIds.Add(functionId, functionName);
        }

        Moments = context.ReadList<GameMakerKeyframe<GameMakerMoment>>();
    }

    public void Write(SerializationContext context) {
        context.Write(Name);
        context.Write((uint)PlaybackType);
        context.Write(PlaybackSpeed);
        context.Write((int)PlaybackSpeedType);
        context.Write(Length);
        context.Write(OriginX);
        context.Write(OriginY);
        context.Write(Volume);
        context.Write(BroadcastMessages);
        context.Write(Tracks);
        context.Write(FunctionIds!.Count);

        foreach (var (functionId, functionName) in FunctionIds) {
            context.Write(functionId);
            context.Write(functionName);
        }

        context.Write(Moments);
    }
}
