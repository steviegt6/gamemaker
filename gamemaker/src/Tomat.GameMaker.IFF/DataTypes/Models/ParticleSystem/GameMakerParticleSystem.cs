using System.Collections.Generic;
using Tomat.GameMaker.IFF.Chunks;
using Tomat.GameMaker.IFF.DataTypes.Models.String;

namespace Tomat.GameMaker.IFF.DataTypes.Models.ParticleSystem;

public sealed class GameMakerParticleSystem : IGameMakerSerializable {
    public GameMakerPointer<GameMakerString> Name { get; set; }

    public int OriginX { get; set; }

    public int OriginY { get; set; }

    public int DrawOrder { get; set; }

    public List<int> EmitterIds { get; set; } = null!;

    public void Read(DeserializationContext context) {
        Name = context.ReadPointerAndObject<GameMakerString>();
        OriginX = context.ReadInt32();
        OriginY = context.ReadInt32();
        DrawOrder = context.ReadInt32();
        var emitterCount = context.ReadInt32();
        EmitterIds = new List<int>(emitterCount);
        for (var i = 0; i < emitterCount; i++)
            EmitterIds.Add(context.ReadInt32());
    }

    public void Write(SerializationContext context) {
        context.Write(Name);
        context.Write(OriginX);
        context.Write(OriginY);
        context.Write(DrawOrder);
        context.Write(EmitterIds.Count);
        foreach (var emitterId in EmitterIds)
            context.Write(emitterId);
    }
}
