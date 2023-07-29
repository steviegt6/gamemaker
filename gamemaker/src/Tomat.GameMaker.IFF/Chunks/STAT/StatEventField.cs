using System.Text;

namespace Tomat.GameMaker.IFF.Chunks.STAT;

public sealed class StatEventField : IGameMakerSerializable {
    // TODO: This field may not be present in some versions.
    public string? Name { get; set; }

    public StatEventFieldFieldType FieldType { get; set; }

    public void Read(DeserializationContext context) {
        // TODO: This field may not be present in some versions.
        Name = context.ReadNullTerminatedString(Encoding.UTF8);
        FieldType = (StatEventFieldFieldType)context.ReadInt32();
    }

    public void Write(SerializationContext context) {
        if (Name is not null)
            context.WriteNullTerminatedString(Name, Encoding.UTF8);

        context.Write((int)FieldType);
    }
}
