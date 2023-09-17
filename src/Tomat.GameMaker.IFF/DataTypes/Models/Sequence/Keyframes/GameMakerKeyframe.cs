using System.Collections.Generic;

namespace Tomat.GameMaker.IFF.DataTypes.Models.Sequence.Keyframes;

public sealed class GameMakerKeyframe<T> : IGameMakerSerializable where T : IGameMakerSerializable, new() {
    public float Key { get; set; }

    public float Length { get; set; }

    public bool Stretch { get; set; }

    public bool Disabled { get; set; }

    public Dictionary<int, T>? Channels { get; set; }

    public void Read(DeserializationContext context) {
        Key = context.ReadSingle();
        Length = context.ReadSingle();
        Stretch = context.ReadBoolean(wide: true);
        Disabled = context.ReadBoolean(wide: true);

        var count = context.ReadInt32();
        Channels = new Dictionary<int, T>(count);

        for (var i = 0; i < count; i++) {
            var channel = context.ReadInt32();
            var value = new T();
            value.Read(context);
            Channels.Add(channel, value);
        }
    }

    public void Write(SerializationContext context) {
        context.Write(Key);
        context.Write(Length);
        context.Write(Stretch, wide: true);
        context.Write(Disabled, wide: true);

        context.Write(Channels!.Count);

        foreach (var (channel, value) in Channels) {
            context.Write(channel);
            value.Write(context);
        }
    }
}
