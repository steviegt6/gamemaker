using System.Collections.Generic;
using Tomat.GameMaker.IFF.Chunks;
using Tomat.GameMaker.IFF.DataTypes.Models.String;

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

    public void Read(DeserializationContext context) {
        Name = context.ReadPointerAndObject<GameMakerString>();
        Caption = context.ReadPointerAndObject<GameMakerString>();
        Width = context.Reader.ReadInt32();
        Height = context.Reader.ReadInt32();
        Speed = context.Reader.ReadInt32();
        Persistent = context.Reader.ReadBoolean(wide: true);
        BackgroundColor = context.Reader.ReadInt32();
        DrawBackgroundColor = context.Reader.ReadBoolean(wide: true);
        CreationCodeId = context.Reader.ReadInt32();
        var flags = context.Reader.ReadInt32();
        if (context.VersionInfo.IsAtLeast(GM_2_3))
            flags &= ~0x30000;
        else if (context.VersionInfo.IsAtLeast(GM_2))
            flags &= ~0x20000;
        Flags = (GameMakerRoomFlags)flags;
        Backgrounds = context.ReadPointerAndObject<GameMakerPointerList<GameMakerRoomBackground>>();
        Views = context.ReadPointerAndObject<GameMakerPointerList<GameMakerRoomView>>();
        var gameObjectListPointer = context.Reader.ReadInt32();
        var tilePointer = context.Reader.ReadInt32();
        Tiles = context.ReadPointerAndObject<GameMakerPointerList<GameMakerRoomTile>>(tilePointer);
        Physics = context.Reader.ReadBoolean(wide: true);
        Top = context.Reader.ReadInt32();
        Left = context.Reader.ReadInt32();
        Right = context.Reader.ReadInt32();
        Bottom = context.Reader.ReadInt32();
        GravityX = context.Reader.ReadSingle();
        GravityY = context.Reader.ReadSingle();
        PixelsToMeters = context.Reader.ReadSingle();

        if (context.VersionInfo.IsAtLeast(GM_2)) {
            Layers = context.ReadPointerAndObject<GameMakerPointerList<GameMakerRoomLayer>>();

            if (context.VersionInfo.IsAtLeast(GM_2_3)) {
                // Read sequence ID list.
                context.Reader.Position = context.Reader.ReadInt32();
                var sequenceIdCount = context.Reader.ReadInt32();
                SequenceIds = new List<int>(sequenceIdCount);
                for (var i = 0; i < sequenceIdCount; i++)
                    SequenceIds.Add(context.Reader.ReadInt32());
            }
        }

        // Handle reading game objects, which change lengths in 2.2.2.302,
        // roughly. Calculate the size of them.
        context.Reader.Position = gameObjectListPointer;
        var count = context.Reader.ReadInt32();
        int eachSize;

        if (count > 1) {
            var first = context.Reader.ReadInt32();
            eachSize = context.Reader.ReadInt32() - first;
        }
        else {
            eachSize = tilePointer - (context.Reader.Position + 4);
        }

        if (eachSize >= 40) {
            context.VersionInfo.RoomsAndObjectsUsePreCreate = true;

            if (eachSize == 48)
                context.VersionInfo.UpdateTo(GM_2_2_2_302);
        }

        context.Reader.Position = gameObjectListPointer;
        GameObjects = context.ReadPointerAndObject<GameMakerPointerList<GameMakerRoomGameObject>>(gameObjectListPointer);
    }

    public void Write(SerializationContext context) {
        context.Writer.Write(Name);
        context.Writer.Write(Caption);
        context.Writer.Write(Width);
        context.Writer.Write(Height);
        context.Writer.Write(Speed);
        context.Writer.Write(Persistent, wide: true);
        context.Writer.Write(BackgroundColor);
        context.Writer.Write(DrawBackgroundColor, wide: true);
        context.Writer.Write(CreationCodeId);
        var flags = (int)Flags;
        if (context.VersionInfo.IsAtLeast(GM_2_3))
            flags |= 0x30000;
        else if (context.VersionInfo.IsAtLeast(GM_2))
            flags |= 0x20000;
        context.Writer.Write(flags);
        context.Writer.Write(Backgrounds);
        context.Writer.Write(Views);
        context.Writer.Write(GameObjects);
        context.Writer.Write(Tiles);
        context.Writer.Write(Physics, wide: true);
        context.Writer.Write(Top);
        context.Writer.Write(Left);
        context.Writer.Write(Right);
        context.Writer.Write(Bottom);
        context.Writer.Write(GravityX);
        context.Writer.Write(GravityY);
        context.Writer.Write(PixelsToMeters);
        var sequencePatch = -1;

        if (context.VersionInfo.IsAtLeast(GM_2)) {
            context.Writer.Write(Layers);

            if (context.VersionInfo.IsAtLeast(GM_2_3)) {
                sequencePatch = context.Writer.Position;
                context.Writer.Write(0);
            }
        }

        context.MarkPointerAndWriteObject(Backgrounds);
        context.MarkPointerAndWriteObject(Views);
        context.MarkPointerAndWriteObject(GameObjects);
        context.MarkPointerAndWriteObject(Tiles);

        if (context.VersionInfo.IsAtLeast(GM_2)) {
            context.MarkPointerAndWriteObject(Layers);

            if (context.VersionInfo.IsAtLeast(GM_2_3)) {
                var returnTo = context.Writer.Position;
                context.Writer.Position = sequencePatch;
                context.Writer.Write(returnTo);
                context.Writer.Position = returnTo;

                context.Writer.Write(SequenceIds!.Count);
                foreach (var sequenceId in SequenceIds!)
                    context.Writer.Write(sequenceId);
            }
        }
    }
}
