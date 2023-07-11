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

    public GameMakerList<GameMakerRoomLayerEffectProperty>? EffectProperties { get; set; }

    // Only one of these are not null at a time.
    public GameMakerRoomLayerBackground? Background { get; set; }

    public GameMakerRoomLayerInstances? Instances { get; set; }

    public GameMakerRoomLayerAssets? Assets { get; set; }

    public GameMakerRoomLayerTiles? Tiles { get; set; }

    public GameMakerRoomLayerEffect? Effect { get; set; }

    public void Read(DeserializationContext context) {
        Name = context.ReadPointerAndObject<GameMakerString>();
        Id = context.ReadInt32();
        LayerKind = (GameMakerRoomLayerKind)context.ReadInt32();
        Depth = context.ReadInt32();
        OffsetX = context.ReadSingle();
        OffsetY = context.ReadSingle();
        HSpeed = context.ReadSingle();
        VSpeed = context.ReadSingle();
        Visible = context.ReadBoolean(wide: true);

        if (context.VersionInfo.Version.Major >= 2022) {
            EffectEnabled = context.ReadBoolean(wide: true);
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
        context.Write(Name);
        context.Write(Id);
        context.Write((int)LayerKind);
        context.Write(Depth);
        context.Write(OffsetX);
        context.Write(OffsetY);
        context.Write(HSpeed);
        context.Write(VSpeed);
        context.Write(Visible, wide: true);

        if (context.VersionInfo.Version.Major >= 2022) {
            context.Write(EffectEnabled, wide: true);
            context.Write(EffectType);
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
