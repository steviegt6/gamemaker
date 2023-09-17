using System.Collections.Generic;

namespace Tomat.GameMaker.IFF.DataTypes.Models.Room;

public sealed class GameMakerRoom : IGameMakerSerializable {
    public GameMakerPointer<GameMakerString> Name { get; set; }

    public GameMakerPointer<GameMakerString> Caption { get; set; }

    public int Width { get; set; }

    public int Height { get; set; }

    public int Speed { get; set; }

    public bool Persistent { get; set; }

    public int BackgroundColor { get; set; }

    public bool DrawBackgroundColor { get; set; }

    public int CreationCodeId { get; set; }

    public GameMakerRoomFlags Flags { get; set; }

    public GameMakerPointer<GameMakerPointerList<GameMakerRoomBackground>> Backgrounds { get; set; }

    public GameMakerPointer<GameMakerPointerList<GameMakerRoomView>> Views { get; set; }

    public GameMakerPointer<GameMakerPointerList<GameMakerRoomGameObject>> GameObjects { get; set; }

    public GameMakerPointer<GameMakerPointerList<GameMakerRoomTile>> Tiles { get; set; }

    public bool Physics { get; set; }

    public int Top { get; set; }

    public int Left { get; set; }

    public int Right { get; set; }

    public int Bottom { get; set; }

    public float GravityX { get; set; }

    public float GravityY { get; set; }

    public float PixelsToMeters { get; set; }

    // GMS2+ only
    public GameMakerPointer<GameMakerPointerList<GameMakerRoomLayer>> Layers { get; set; }

    // GMS2.3+ only
    public List<int>? SequenceIds { get; set; }

    private int pos;

    public void Read(DeserializationContext context) {
        Name = context.ReadPointerAndObject<GameMakerString>();
        Caption = context.ReadPointerAndObject<GameMakerString>();
        Width = context.ReadInt32();
        Height = context.ReadInt32();
        Speed = context.ReadInt32();
        Persistent = context.ReadBoolean(wide: true);
        BackgroundColor = context.ReadInt32();
        DrawBackgroundColor = context.ReadBoolean(wide: true);
        CreationCodeId = context.ReadInt32();
        var flags = context.ReadInt32();
        if (context.VersionInfo.IsAtLeast(GM_2_3))
            flags &= ~0x30000;
        else if (context.VersionInfo.IsAtLeast(GM_2))
            flags &= ~0x20000;
        Flags = (GameMakerRoomFlags)flags;
        Backgrounds = context.ReadPointerAndObject<GameMakerPointerList<GameMakerRoomBackground>>();
        Views = context.ReadPointerAndObject<GameMakerPointerList<GameMakerRoomView>>();
        var gameObjectListPointer = context.ReadInt32();
        var tilePointer = context.ReadInt32();
        Tiles = context.ReadPointerAndObject<GameMakerPointerList<GameMakerRoomTile>>(tilePointer);
        Physics = context.ReadBoolean(wide: true);
        Top = context.ReadInt32();
        Left = context.ReadInt32();
        Right = context.ReadInt32();
        Bottom = context.ReadInt32();
        GravityX = context.ReadSingle();
        GravityY = context.ReadSingle();
        PixelsToMeters = context.ReadSingle();

        if (context.VersionInfo.IsAtLeast(GM_2)) {
            Layers = context.ReadPointerAndObject<GameMakerPointerList<GameMakerRoomLayer>>();

            if (context.VersionInfo.IsAtLeast(GM_2_3)) {
                // Read sequence ID list.
                context.Position = context.ReadInt32();
                var sequenceIdCount = context.ReadInt32();
                SequenceIds = new List<int>(sequenceIdCount);
                for (var i = 0; i < sequenceIdCount; i++)
                    SequenceIds.Add(context.ReadInt32());
            }
        }

        // Handle reading game objects, which change lengths in 2.2.2.302,
        // roughly. Calculate the size of them.
        context.Position = gameObjectListPointer;
        var count = context.ReadInt32();
        int eachSize;

        if (count > 1) {
            var first = context.ReadInt32();
            eachSize = context.ReadInt32() - first;
        }
        else {
            eachSize = tilePointer - (context.Position + 4);
        }

        if (eachSize >= 40) {
            context.VersionInfo.RoomsAndObjectsUsePreCreate = true;

            if (eachSize == 48)
                context.VersionInfo.UpdateTo(GM_2_2_2_302);
        }

        context.Position = gameObjectListPointer;
        GameObjects = context.ReadPointerAndObject<GameMakerPointerList<GameMakerRoomGameObject>>(gameObjectListPointer);
        pos = context.Position;
    }

    public void Write(SerializationContext context) {
        context.Write(Name);
        context.Write(Caption);
        context.Write(Width);
        context.Write(Height);
        context.Write(Speed);
        context.Write(Persistent, wide: true);
        context.Write(BackgroundColor);
        context.Write(DrawBackgroundColor, wide: true);
        context.Write(CreationCodeId);
        var flags = (int)Flags;
        if (context.VersionInfo.IsAtLeast(GM_2_3))
            flags |= 0x30000;
        else if (context.VersionInfo.IsAtLeast(GM_2))
            flags |= 0x20000;
        context.Write(flags);
        context.Write(Backgrounds);
        context.Write(Views);
        context.Write(GameObjects);
        context.Write(Tiles);
        context.Write(Physics, wide: true);
        context.Write(Top);
        context.Write(Left);
        context.Write(Right);
        context.Write(Bottom);
        context.Write(GravityX);
        context.Write(GravityY);
        context.Write(PixelsToMeters);
        var sequencePatch = -1;

        if (context.VersionInfo.IsAtLeast(GM_2)) {
            context.Write(Layers);

            if (context.VersionInfo.IsAtLeast(GM_2_3)) {
                sequencePatch = context.Position;
                context.Write(0);
            }
        }

        context.MarkPointerAndWriteObject(Backgrounds);
        context.MarkPointerAndWriteObject(Views);
        context.MarkPointerAndWriteObject(GameObjects);
        context.MarkPointerAndWriteObject(Tiles);

        if (context.VersionInfo.IsAtLeast(GM_2)) {
            context.MarkPointerAndWriteObject(Layers);

            if (context.VersionInfo.IsAtLeast(GM_2_3)) {
                var returnTo = context.Position;
                context.Position = sequencePatch;
                context.Write(returnTo);
                context.Position = returnTo;

                context.Write(SequenceIds!.Count);
                foreach (var sequenceId in SequenceIds!)
                    context.Write(sequenceId);
            }
        }
    }
}
