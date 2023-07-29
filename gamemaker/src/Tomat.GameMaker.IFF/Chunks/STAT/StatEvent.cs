using System.Collections.Generic;
using System.IO;
using System.Text;

namespace Tomat.GameMaker.IFF.Chunks.STAT;

public sealed class StatEvent : IGameMakerSerializable {
    public string Name { get; set; } = null!;

    public string Schema { get; set; } = null!;

    public StatEventLatency Latency { get; set; }

    public StatEventPriority Priority { get; set; }

    public StatEventEnabledState UploadEnabled { get; set; }

    public StatEventPopulationSampleRate PopulationSampleRate { get; set; }

    public int EventIndex { get; set; }

    public int Version { get; set; }

    // public int FieldCount { get; set; }

    // "Implicit clock field", what?
    public int UnknownInt32 { get; set; }

    public List<StatEventField> Fields { get; set; } = null!;

    private readonly int readIndex;

    public StatEvent() : this(-1) { }

    public StatEvent(int readIndex) {
        this.readIndex = readIndex;
    }

    public void Read(DeserializationContext context) {
        Name = context.ReadNullTerminatedString(Encoding.UTF8);
        Schema = context.ReadNullTerminatedString(Encoding.UTF8);
        Latency = (StatEventLatency)context.ReadInt32();
        Priority = (StatEventPriority)context.ReadInt32();
        UploadEnabled = (StatEventEnabledState)context.ReadInt32();
        PopulationSampleRate = (StatEventPopulationSampleRate)context.ReadInt32();
        EventIndex = context.ReadInt32();

        if (readIndex != -1 && EventIndex != readIndex + 1)
            throw new InvalidDataException($"Expected event index {readIndex + 1}, got {EventIndex}.");

        Version = context.ReadInt32();

        var fieldCount = context.ReadInt32(); // 1-indexed for some reason.
        UnknownInt32 = context.ReadInt32();
        Fields = new List<StatEventField>(fieldCount);

        for (var i = 0; i < fieldCount - 1; i++) {
            var field = new StatEventField();
            field.Read(context);
            Fields.Add(field);
        }
    }

    public void Write(SerializationContext context) {
        context.WriteNullTerminatedString(Name, Encoding.UTF8);
        context.WriteNullTerminatedString(Schema, Encoding.UTF8);
        context.Write((int)Latency);
        context.Write((int)Priority);
        context.Write((int)UploadEnabled);
        context.Write((int)PopulationSampleRate);
        context.Write(EventIndex);
        context.Write(Version);
        context.Write(Fields.Count + 1);
        context.Write(UnknownInt32);
        foreach (var field in Fields)
            field.Write(context);
    }
}
