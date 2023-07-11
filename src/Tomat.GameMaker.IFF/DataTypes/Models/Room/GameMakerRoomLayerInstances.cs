using System.Collections.Generic;
using Tomat.GameMaker.IFF.Chunks;

namespace Tomat.GameMaker.IFF.DataTypes.Models.Room; 

public sealed class GameMakerRoomLayerInstances : IGameMakerSerializable {
    // IDs correspond to the IDs given in the GameObjects list in the room.
    public List<int>? Instances { get; set; }

    public void Read(DeserializationContext context) {
        var count = context.ReadInt32();
        Instances = new List<int>();
        for (var i = 0; i < count; i++)
            Instances.Add(context.ReadInt32());
    }

    public void Write(SerializationContext context) {
        context.Write(Instances!.Count);
        foreach (var instance in Instances)
            context.Write(instance);
    }
}
