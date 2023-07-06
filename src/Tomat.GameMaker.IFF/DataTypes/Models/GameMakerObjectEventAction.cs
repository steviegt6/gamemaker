using System.IO;
using Tomat.GameMaker.IFF.Chunks;

namespace Tomat.GameMaker.IFF.DataTypes.Models;

// TODO: Lots of unknowns.
public sealed class GameMakerObjectEventAction : IGameMakerSerializable {
    public int LibId { get; set; }

    public int Id { get; set; }

    public int Kind { get; set; }

    public bool UseRelative { get; set; }

    public bool IsQuestion { get; set; }

    public bool UseApplyTo { get; set; }

    public int ExeType { get; set; }

    public GameMakerPointer<GameMakerString> ActionName { get; set; }

    // Id of the GameMakerCode that will be executed.
    public int CodeId { get; set; }

    public int ArgumentCount { get; set; }

    public int Who { get; set; }

    public bool Relative { get; set; }

    public bool IsNot { get; set; }

    public void Read(DeserializationContext context) {
        LibId = context.Reader.ReadInt32();
        Id = context.Reader.ReadInt32();
        Kind = context.Reader.ReadInt32();
        UseRelative = context.Reader.ReadBoolean(wide: true);
        IsQuestion = context.Reader.ReadBoolean(wide: true);
        UseApplyTo = context.Reader.ReadBoolean(wide: true);
        ExeType = context.Reader.ReadInt32();
        ActionName = context.ReadPointerAndObject<GameMakerString>();
        CodeId = context.Reader.ReadInt32();
        ArgumentCount = context.Reader.ReadInt32();
        Who = context.Reader.ReadInt32();
        Relative = context.Reader.ReadBoolean(wide: true);
        IsNot = context.Reader.ReadBoolean(wide: true);

        if (context.Reader.ReadInt32() != 0)
            throw new InvalidDataException("Expected 0 at the end of a GameMakerObjectEventAction.");
    }

    public void Write(SerializationContext context) {
        context.Writer.Write(LibId);
        context.Writer.Write(Id);
        context.Writer.Write(Kind);
        context.Writer.Write(UseRelative, wide: true);
        context.Writer.Write(IsQuestion, wide: true);
        context.Writer.Write(UseApplyTo, wide: true);
        context.Writer.Write(ExeType);
        context.Writer.Write(ActionName);
        context.Writer.Write(CodeId);
        context.Writer.Write(ArgumentCount);
        context.Writer.Write(Who);
        context.Writer.Write(Relative, wide: true);
        context.Writer.Write(IsNot, wide: true);
        context.Writer.Write(0);
    }
}
