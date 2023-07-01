using System.Collections.Generic;
using System.IO;
using Tomat.GameMaker.IFF.Chunks;

namespace Tomat.GameMaker.IFF.DataTypes.Models;

public sealed class GameMakerTrack : IGameMakerSerializable {
    public GameMakerPointer<GameMakerString> ModelName { get; set; }

    public GameMakerPointer<GameMakerString> Name { get; set; }

    public int BuiltinName { get; set; }

    public GameMakerTrackTraits TrackTraits { get; set; }

    public bool IsCreationTrack { get; set; }

    public List<int>? Tags { get; set; }

    public List<GameMakerTrack>? Tracks { get; set; }

    public GameMakerTrackKeyframes? TrackKeyframes { get; set; }

    public List<IGameMakerSerializable>? OwnedResources { get; set; }

    public List<GameMakerPointer<GameMakerString>>? OwnedResourceTypes { get; set; }

    public void Read(DeserializationContext context) {
        ModelName = context.ReadPointerAndObject<GameMakerString>();
        Name = context.ReadPointerAndObject<GameMakerString>();
        BuiltinName = context.Reader.ReadInt32();
        TrackTraits = (GameMakerTrackTraits)context.Reader.ReadInt32();
        IsCreationTrack = context.Reader.ReadBoolean(wide: true);

        var tagCount = context.Reader.ReadInt32();
        var ownedResourceCount = context.Reader.ReadInt32();
        var trackCount = context.Reader.ReadInt32();

        Tags = new List<int>(tagCount);
        for (var i = 0; i < tagCount; i++)
            Tags.Add(context.Reader.ReadInt32());

        OwnedResources = new List<IGameMakerSerializable>(ownedResourceCount);
        OwnedResourceTypes = new List<GameMakerPointer<GameMakerString>>(ownedResourceCount);

        for (var i = 0; i < ownedResourceCount; i++) {
            var str = context.ReadPointerAndObject<GameMakerString>();
            OwnedResourceTypes.Add(str);

            switch (str.Object!.Value) {
                case "GMAnimCurve":
                    var curve = new GameMakerAnimationCurve();
                    curve.Read(context);
                    OwnedResources.Add(curve);
                    break;

                default:
                    throw new InvalidDataException("Unknown owned resource type: " + str.Object!.Value);
            }
        }

        Tracks = new List<GameMakerTrack>(trackCount);

        for (var i = 0; i < trackCount; i++) {
            var track = new GameMakerTrack();
            track.Read(context);
            Tracks.Add(track);
        }

        switch (ModelName.Object!.Value) {
            case "GMAudioTrack":
                var keyframes = new GameMakerTrackAudioKeyframes();
                keyframes.Read(context);
                TrackKeyframes = keyframes;
                break;

            case "GMInstanceTrack":
            case "GMGraphicTrack":
            case "GMSequenceTrack":
                var idKeyframes = new GameMakerTrackIdKeyframes();
                idKeyframes.Read(context);
                TrackKeyframes = idKeyframes;
                break;

            case "GMSpriteFramesTrack":
            case "GMBoolTrack":
                var valueKeyframes = new GameMakerTrackValueKeyframes();
                valueKeyframes.Read(context);
                TrackKeyframes = valueKeyframes;
                break;

            case "GMStringTrack":
                var stringKeyframes = new GameMakerTrackStringKeyframes();
                stringKeyframes.Read(context);
                TrackKeyframes = stringKeyframes;
                break;

            case "GMColourTrack":
            case "GMRealTrack":
                var interpolatedKeyframes = new GameMakerTrackValueInterpolatedKeyframes();
                interpolatedKeyframes.Read(context);
                TrackKeyframes = interpolatedKeyframes;
                break;

            case "GMGroupTrack":
                // TODO: Handle?
                break;

            default:
                throw new InvalidDataException("Unknown track type: " + ModelName.Object!.Value);
        }
    }

    public void Write(SerializationContext context) {
        context.Writer.Write(ModelName);
        context.Writer.Write(Name);
        context.Writer.Write(BuiltinName);
        context.Writer.Write((int)TrackTraits);
        context.Writer.Write(IsCreationTrack, wide: true);

        context.Writer.Write(Tags!.Count);
        context.Writer.Write(OwnedResources!.Count);
        context.Writer.Write(Tracks!.Count);

        foreach (var tag in Tags)
            context.Writer.Write(tag);

        for (var i = 0; i < OwnedResources.Count; i++) {
            context.Writer.Write(OwnedResourceTypes![i]);
            OwnedResources[i]!.Write(context);
        }

        foreach (var track in Tracks)
            track.Write(context);

        switch (ModelName.Object!.Value) {
            case "GMAudioTrack":
            //
            case "GMInstanceTrack":
            case "GMGraphicTrack":
            case "GMSequenceTrack":
            //
            case "GMSpriteFramesTrack":
            case "GMBoolTrack":
            //
            case "GMStringTrack":
            //
            case "GMColourTrack":
            case "GMRealTrack":
                TrackKeyframes!.Write(context);
                break;

            case "GMGroupTrack":
                // TODO: Handle?
                break;

            default:
                throw new InvalidDataException("Unknown track type: " + ModelName.Object!.Value);
        }
    }
}
