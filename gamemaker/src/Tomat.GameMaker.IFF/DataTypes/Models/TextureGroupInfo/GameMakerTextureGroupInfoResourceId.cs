using Tomat.GameMaker.IFF.Chunks;

namespace Tomat.GameMaker.IFF.DataTypes.Models.TextureGroupInfo; 

public sealed class GameMakerTextureGroupInfoResourceId : IGameMakerSerializable {
    public int Id { get; set; }

    public void Read(DeserializationContext context) {
        Id = context.ReadInt32();
    }

    public void Write(SerializationContext context) {
        context.Write(Id);
    }
}
