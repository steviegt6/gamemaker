using System.Collections.Generic;
using Tomat.GameMaker.IFF.Chunks;

namespace Tomat.GameMaker.IFF.DataTypes.Models.Sequence.Keyframes;

public sealed class GameMakerKeyframe<T> : IGameMakerSerializable where T : IGameMakerSerializable, new() {
    public float Key { get; set; }

    public float Length { get; set; }

    public bool Stretch { get; set; }

    public bool Disabled { get; set; }

    public Dictionary<int, T>? Channels { get; set; }

    public void Read(DeserializationContext context) {
        Key = context.Reader.ReadSingle();
        Length = context.Reader.ReadSingle();
        Stretch = context.Reader.ReadBoolean(wide: true);
        Disabled = context.Reader.ReadBoolean(wide: true);

        var count = context.Reader.ReadInt32();
        Channels = new Dictionary<int, T>(count);

        for (var i = 0; i < count; i++) {
            var channel = context.Reader.ReadInt32();
            var value = new T();
            value.Read(context);
            Channels.Add(channel, value);
        }
    }

    public void Write(SerializationContext context) {
        context.Writer.Write(Key);
        context.Writer.Write(Length);
        context.Writer.Write(Stretch, wide: true);
        context.Writer.Write(Disabled, wide: true);

        context.Writer.Write(Channels!.Count);

        foreach (var (channel, value) in Channels) {
            context.Writer.Write(channel);
            value.Write(context);
        }
    }
}
