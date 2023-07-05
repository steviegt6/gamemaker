using Tomat.GameMaker.IFF.Chunks;

namespace Tomat.GameMaker.IFF.DataTypes.Models;

public sealed class GameMakerKerning : IGameMakerSerializable {
    public short Other { get; set; }

    public short Amount { get; set; }

    public void Read(DeserializationContext context) {
        Other = context.Reader.ReadInt16();
        Amount = context.Reader.ReadInt16();
    }

    public void Write(SerializationContext context) {
        context.Writer.Write(Other);
        context.Writer.Write(Amount);
    }
}
