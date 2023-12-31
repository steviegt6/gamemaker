﻿using Tomat.GameMaker.IFF.DataTypes.Models.Room;

namespace Tomat.GameMaker.IFF.Chunks.ROOM;

internal sealed class GameMakerRoomChunk : AbstractChunk,
                                           IRoomChunk {
    public const string NAME = "ROOM";

    public GameMakerPointerList<GameMakerRoom> Rooms { get; set; } = null!;

    public GameMakerRoomChunk(string name, int size, int startPosition) : base(name, size, startPosition) { }

    public override void Read(DeserializationContext context) {
        Rooms = context.ReadPointerList<GameMakerRoom>();
    }

    public override void Write(SerializationContext context) {
        context.Write(Rooms);
    }
}
