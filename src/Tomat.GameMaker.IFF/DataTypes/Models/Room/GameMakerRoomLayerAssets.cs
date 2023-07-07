using Tomat.GameMaker.IFF.Chunks;

namespace Tomat.GameMaker.IFF.DataTypes.Models.Room;

public sealed class GameMakerRoomLayerAssets : IGameMakerSerializable {
    public GameMakerPointer<GameMakerPointerList<GameMakerRoomTile>> LegacyTiles { get; set; }

    public GameMakerPointer<GameMakerPointerList<GameMakerRoomLayerAssetInstance>> Sprites { get; set; }

    // GMS 2.3+
    public GameMakerPointer<GameMakerPointerList<GameMakerRoomLayerAssetInstance>> Sequences { get; set; }

    // Removed in 2.3.2
    public GameMakerPointer<GameMakerPointerList<GameMakerRoomLayerAssetInstance>> NineSlices { get; set; }

    public void Read(DeserializationContext context) {
        LegacyTiles = context.ReadPointerAndObject<GameMakerPointerList<GameMakerRoomTile>>();
        Sprites = context.ReadPointerAndObject<GameMakerPointerList<GameMakerRoomLayerAssetInstance>>();

        if (!context.VersionInfo.IsAtLeast(GM_2_3))
            return;
        
        Sequences = context.ReadPointerAndObject<GameMakerPointerList<GameMakerRoomLayerAssetInstance>>();
        
        if (context.VersionInfo.IsAtLeast(GM_2_3_2))
            NineSlices = context.ReadPointerAndObject<GameMakerPointerList<GameMakerRoomLayerAssetInstance>>();
    }

    public void Write(SerializationContext context) {
        context.Writer.Write(LegacyTiles);
        context.Writer.Write(Sprites);

        if (context.VersionInfo.IsAtLeast(GM_2_3)) {
            context.Writer.Write(Sequences);
            
            if (context.VersionInfo.IsAtLeast(GM_2_3_2))
                context.Writer.Write(NineSlices);
        }
        
        context.MarkPointerAndWriteObject(LegacyTiles);
        context.MarkPointerAndWriteObject(Sprites);

        if (!context.VersionInfo.IsAtLeast(GM_2_3))
            return;

        context.MarkPointerAndWriteObject(Sequences);

        if (!context.VersionInfo.IsAtLeast(GM_2_3_2))
            return;

        // Even if it's 2.3.2 but we don't detect it, this shouldn't break the
        // format.
        if (NineSlices.IsNull)
            context.Writer.Write(0);
        else
            context.MarkPointerAndWriteObject(NineSlices);
    }
}
