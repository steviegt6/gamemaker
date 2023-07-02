using Tomat.GameMaker.IFF.Chunks;

namespace Tomat.GameMaker.IFF.DataTypes.Models;

public abstract class GameMakerTrackKeyframes : IGameMakerSerializable {
    public abstract void Read(DeserializationContext context);

    public abstract void Write(SerializationContext context);
}
