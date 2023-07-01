using Tomat.GameMaker.IFF.Chunks;

namespace Tomat.GameMaker.IFF.DataTypes.Models;

public sealed class GameMakerPoint : IGameMakerSerializable {
    public float X { get; set; }

    public float Value { get; set; }

    public float BezierX0 { get; set; }

    public float BezierY0 { get; set; }

    public float BezierX1 { get; set; }

    public float BezierY1 { get; set; }

    public void Read(DeserializationContext context) {
        X = context.Reader.ReadSingle();
        Value = context.Reader.ReadSingle();

        // In 2.3, an int32 with a value of zero would be set here. It cannot be
        // version 2.3 if this value isn't zero.
        if (context.Reader.ReadUInt32() != 0) {
            context.VersionInfo.Update(GameMakerVersionInfo.GM_2_3_1);
            context.Reader.Position -= sizeof(uint);
        }
        else {
            // If BezierX0 equals zero (above), then BezierY0 equals zero as
            // well.
            if (context.Reader.ReadUInt32() == 0)
                context.VersionInfo.Update(GameMakerVersionInfo.GM_2_3_1);
            context.Reader.Position -= sizeof(uint) * 2;
        }

        if (context.VersionInfo.Version >= GameMakerVersionInfo.GM_2_3_1) {
            BezierX0 = context.Reader.ReadSingle();
            BezierY0 = context.Reader.ReadSingle();
            BezierX1 = context.Reader.ReadSingle();
            BezierY1 = context.Reader.ReadSingle();
        }
        else {
            // Skip over the aforementioned should-be-zero int32 values on older
            // versions.
            context.Reader.Position += sizeof(uint);
        }
    }

    public void Write(SerializationContext context) {
        throw new System.NotImplementedException();
    }
}
