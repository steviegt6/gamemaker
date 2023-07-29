using Tomat.GameMaker.IFF.Chunks;
using Tomat.GameMaker.IFF.DataTypes.Models.String;

namespace Tomat.GameMaker.IFF.DataTypes.Models.DebugScript;

public sealed class GameMakerDebugScript : IGameMakerSerializable {
    public GameMakerPointer<GameMakerString> Name { get; set; }

    public void Read(DeserializationContext context) {
        Name = context.ReadPointerAndObject<GameMakerString>();
    }

    public void Write(SerializationContext context) {
        context.Write(Name);
    }
}
