using System;
using Tomat.GameMaker.IFF.Chunks;
using Tomat.GameMaker.IFF.DataTypes.Models.String;

namespace Tomat.GameMaker.IFF.DataTypes.Models.Room;

public sealed class GameMakerRoomLayer : IGameMakerSerializable {
    public GameMakerPointer<GameMakerString> Name { get; set; }

    public int Id { get; set; }

    public GameMakerRoomLayerKind LayerKind { get; set; }

    public int Depth { get; set; }

    public float OffsetX { get; set; }

    public float OffsetY { get; set; }

    public float HSpeed { get; set; }

    public float VSpeed { get; set; }

    public bool Visible { get; set; }

    public bool EffectEnabled { get; set; }

    public GameMakerPointer<GameMakerString> EffectType { get; set; }

    public GameMakerList<GameMakerRoomLayerEffectProperty> EffectProperties { get; set; }

    // Only one of these are not null at a time.
    public GameMakerRoomLayerBackground Background { get; set; }

    public GameMakerRoomLayerInstances Instances { get; set; }

    public GameMakerRoomLayerAssets Assets { get; set; }

    public GameMakerRoomLayerTiles Tiles { get; set; }

    public GameMakerRoomLayerEffect Effect { get; set; }

    public void Read(DeserializationContext context) {
        Name = context.ReadPointerAndObject<GameMakerString>();
        Id = context.Reader.ReadInt32();
        LayerKind = (GameMakerRoomLayerKind)context.Reader.ReadInt32();
        Depth = context.Reader.ReadInt32();
        OffsetX = context.Reader.ReadSingle();
        OffsetY = context.Reader.ReadSingle();
        HSpeed = context.Reader.ReadSingle();
        VSpeed = context.Reader.ReadSingle();
        Visible = context.Reader.ReadBoolean(wide: true);

        if (context.VersionInfo.Version.Major >= 2022) {
            EffectEnabled = context.Reader.ReadBoolean(wide: true);
            EffectType = context.ReadPointerAndObject<GameMakerString>();
            EffectProperties = new GameMakerList<GameMakerRoomLayerEffectProperty>();
            EffectProperties.Read(context);
        }

        switch (LayerKind) {
            case GameMakerRoomLayerKind.Background:
                Background = new GameMakerRoomLayerBackground();
                Background.Read(context);
                break;

            case GameMakerRoomLayerKind.Instances:
                Instances = new GameMakerRoomLayerInstances();
                Instances.Read(context);
                break;

            case GameMakerRoomLayerKind.Assets:
                Assets = new GameMakerRoomLayerAssets();
                Assets.Read(context);
                break;

            case GameMakerRoomLayerKind.Tiles:
                Tiles = new GameMakerRoomLayerTiles();
                Tiles.Read(context);
                break;

            case GameMakerRoomLayerKind.Effect:
                Effect = new GameMakerRoomLayerEffect();
                Effect.Read(context);
                break;

            default:
                throw new ArgumentOutOfRangeException(nameof(LayerKind), LayerKind, "Unknown layer kind.");
        }
    }

    public void Write(SerializationContext context) {
        context.Writer.Write(Name);
        context.Writer.Write(Id);
        context.Writer.Write((int)LayerKind);
        context.Writer.Write(Depth);
        context.Writer.Write(OffsetX);
        context.Writer.Write(OffsetY);
        context.Writer.Write(HSpeed);
        context.Writer.Write(VSpeed);
        context.Writer.Write(Visible, wide: true);

        if (context.VersionInfo.Version.Major >= 2022) {
            context.Writer.Write(EffectEnabled, wide: true);
            context.Writer.Write(EffectType);
            EffectProperties!.Write(context);
        }

        switch (LayerKind) {
            case GameMakerRoomLayerKind.Background:
                Background!.Write(context);
                break;

            case GameMakerRoomLayerKind.Instances:
                Instances!.Write(context);
                break;

            case GameMakerRoomLayerKind.Assets:
                Assets!.Write(context);
                break;

            case GameMakerRoomLayerKind.Tiles:
                Tiles!.Write(context);
                break;

            case GameMakerRoomLayerKind.Effect:
                Effect!.Write(context);
                break;

            default:
                throw new ArgumentOutOfRangeException(nameof(LayerKind), LayerKind, "Unknown layer kind.");
        }
    }
}
