using System;
using System.Collections.Generic;
using System.Text;
using Tomat.GameMaker.IFF.IO;

namespace Tomat.GameMaker.IFF.Chunks.STAT;

public sealed class GameMakerStatChunk : AbstractChunk,
                                         IStatChunk {
    public const string NAME = "STAT";

    public string Name { get; set; } = null!;

    public Guid Guid { get; set; }

    public StatProviderLatency Latency { get; set; }

    public StatProviderPriority Priority { get; set; }

    public StatProviderEnabledState EnabledState { get; set; }

    public int PopulationSampleRates { get; set; }

    public List<StatEvent> Events { get; set; }

    public GameMakerStatChunk(string name, int size, int startPosition) : base(name, size, startPosition) { }

    public override void Read(DeserializationContext context) {
        Name = context.ReadNullTerminatedString(Encoding.UTF8);
        Guid = context.ReadGuid();
        Latency = (StatProviderLatency)context.ReadInt32();
        Priority = (StatProviderPriority)context.ReadInt32();
        EnabledState = (StatProviderEnabledState)context.ReadInt32();
        PopulationSampleRates = context.ReadInt32();
        var eventCount = context.ReadInt32();
        Events = new List<StatEvent>(eventCount);

        for (var i = 0; i < eventCount; i++) {
            var statEvent = new StatEvent(i);
            statEvent.Read(context);
            Events.Add(statEvent);
        }
    }

    public override void Write(SerializationContext context) {
        context.WriteNullTerminatedString(Name, Encoding.UTF8);
        context.Write(Guid.ToByteArray());
        context.Write((int)Latency);
        context.Write((int)Priority);
        context.Write((int)EnabledState);
        context.Write(PopulationSampleRates);
        context.Write(Events.Count);
        foreach (var statEvent in Events)
            statEvent.Write(context);
    }
}
