using System.Collections.Generic;
using System.IO;

namespace Tomat.GameMaker.IFF.DataTypes.Models.Track;

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
        BuiltinName = context.ReadInt32();
        TrackTraits = (GameMakerTrackTraits)context.ReadInt32();
        IsCreationTrack = context.ReadBoolean(wide: true);

        var tagCount = context.ReadInt32();
        var ownedResourceCount = context.ReadInt32();
        var trackCount = context.ReadInt32();

        Tags = new List<int>(tagCount);
        for (var i = 0; i < tagCount; i++)
            Tags.Add(context.ReadInt32());

        OwnedResources = new List<IGameMakerSerializable>(ownedResourceCount);
        OwnedResourceTypes = new List<GameMakerPointer<GameMakerString>>(ownedResourceCount);

        for (var i = 0; i < ownedResourceCount; i++) {
            var str = context.ReadPointerAndObject<GameMakerString>();
            OwnedResourceTypes.Add(str);

            switch (str.ExpectObject().Value) {
                case "GMAnimCurve":
                    var curve = new GameMakerAnimationCurve();
                    curve.Read(context);
                    OwnedResources.Add(curve);
                    break;

                default:
                    throw new InvalidDataException("Unknown owned resource type: " + str.ExpectObject().Value);
            }
        }

        Tracks = new List<GameMakerTrack>(trackCount);

        for (var i = 0; i < trackCount; i++) {
            var track = new GameMakerTrack();
            track.Read(context);
            Tracks.Add(track);
        }

        switch (ModelName.ExpectObject().Value) {
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
                throw new InvalidDataException("Unknown track type: " + ModelName.ExpectObject().Value);
        }
    }

    public void Write(SerializationContext context) {
        context.Write(ModelName);
        context.Write(Name);
        context.Write(BuiltinName);
        context.Write((int)TrackTraits);
        context.Write(IsCreationTrack, wide: true);

        context.Write(Tags!.Count);
        context.Write(OwnedResources!.Count);
        context.Write(Tracks!.Count);

        foreach (var tag in Tags)
            context.Write(tag);

        for (var i = 0; i < OwnedResources.Count; i++) {
            context.Write(OwnedResourceTypes![i]);
            OwnedResources[i]!.Write(context);
        }

        foreach (var track in Tracks)
            track.Write(context);

        switch (ModelName.ExpectObject().Value) {
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
                throw new InvalidDataException("Unknown track type: " + ModelName.ExpectObject().Value);
        }
    }
}
