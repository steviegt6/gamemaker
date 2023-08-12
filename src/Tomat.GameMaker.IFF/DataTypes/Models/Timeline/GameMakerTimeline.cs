using System.Collections.Generic;
using Tomat.GameMaker.IFF.Chunks;
using Tomat.GameMaker.IFF.DataTypes.Models.Object;
using Tomat.GameMaker.IFF.DataTypes.Models.String;

namespace Tomat.GameMaker.IFF.DataTypes.Models.Timeline;

public sealed class GameMakerTimeline : IGameMakerSerializable {
    public GameMakerPointer<GameMakerString> Name { get; set; }

    public List<(int, GameMakerPointer<GameMakerPointerList<GameMakerObjectEventAction>>)>? Moments { get; set; }

    public void Read(DeserializationContext context) {
        Name = context.ReadPointerAndObject<GameMakerString>();

        var count = context.ReadInt32();
        Moments = new List<(int, GameMakerPointer<GameMakerPointerList<GameMakerObjectEventAction>>)>(count);

        for (var i = count; i > 0; i--) {
            var time = context.ReadInt32();
            Moments.Add((time, context.ReadPointerAndObject<GameMakerPointerList<GameMakerObjectEventAction>>()));
        }
    }

    public void Write(SerializationContext context) {
        context.Write(Name);
        context.Write(Moments!.Count);

        foreach (var (time, actions) in Moments) {
            context.Write(time);
            context.Write(actions);
        }

        foreach (var (_, actions) in Moments)
            context.MarkPointerAndWriteObject(actions);
    }
}
