using Tomat.GameMaker.IFF.DataTypes;
using Tomat.GameMaker.IFF.DataTypes.Models.Room;

namespace Tomat.GameMaker.IFF.Chunks.ROOM;

public sealed class GameMakerRoomChunk : AbstractChunk {
    public const string NAME = "ROOM";

    public GameMakerPointerList<GameMakerRoom>? Rooms { get; set; }

    public GameMakerRoomChunk(string name, int size) : base(name, size) { }

    public override void Read(DeserializationContext context) {
        Rooms = new GameMakerPointerList<GameMakerRoom>();
        Rooms.Read(context);
    }

    public override void Write(SerializationContext context) {
        Rooms!.Write(context);
    }
}
