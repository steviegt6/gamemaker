using Tomat.GameMaker.IFF.Chunks;

namespace Tomat.GameMaker.IFF.DataTypes.Models.Font;

public sealed class GameMakerKerning : IGameMakerSerializable {
    public short Other { get; set; }

    public short Amount { get; set; }

    public void Read(DeserializationContext context) {
        Other = context.ReadInt16();
        Amount = context.ReadInt16();
    }

    public void Write(SerializationContext context) {
        context.Write(Other);
        context.Write(Amount);
    }
}
