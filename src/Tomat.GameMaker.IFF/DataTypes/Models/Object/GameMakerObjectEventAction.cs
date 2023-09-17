using System.IO;

namespace Tomat.GameMaker.IFF.DataTypes.Models.Object;

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
        LibId = context.ReadInt32();
        Id = context.ReadInt32();
        Kind = context.ReadInt32();
        UseRelative = context.ReadBoolean(wide: true);
        IsQuestion = context.ReadBoolean(wide: true);
        UseApplyTo = context.ReadBoolean(wide: true);
        ExeType = context.ReadInt32();
        ActionName = context.ReadPointerAndObject<GameMakerString>();
        CodeId = context.ReadInt32();
        ArgumentCount = context.ReadInt32();
        Who = context.ReadInt32();
        Relative = context.ReadBoolean(wide: true);
        IsNot = context.ReadBoolean(wide: true);

        if (context.ReadInt32() != 0)
            throw new InvalidDataException("Expected 0 at the end of a GameMakerObjectEventAction.");
    }

    public void Write(SerializationContext context) {
        context.Write(LibId);
        context.Write(Id);
        context.Write(Kind);
        context.Write(UseRelative, wide: true);
        context.Write(IsQuestion, wide: true);
        context.Write(UseApplyTo, wide: true);
        context.Write(ExeType);
        context.Write(ActionName);
        context.Write(CodeId);
        context.Write(ArgumentCount);
        context.Write(Who);
        context.Write(Relative, wide: true);
        context.Write(IsNot, wide: true);
        context.Write(0);
    }
}
