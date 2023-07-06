using Tomat.GameMaker.IFF.DataTypes;
using Tomat.GameMaker.IFF.DataTypes.Models.Object;

namespace Tomat.GameMaker.IFF.Chunks.OBJT;

public sealed class GameMakerObjtChunk : AbstractChunk {
    public const string NAME = "OBJT";

    public GameMakerPointerList<GameMakerObject>? Objects { get; set; }

    public GameMakerObjtChunk(string name, int size) : base(name, size) { }

    public override void Read(DeserializationContext context) {
        DoFormatCheck(context);

        Objects = new GameMakerPointerList<GameMakerObject>();
        Objects.Read(context);
    }

    public override void Write(SerializationContext context) {
        Objects!.Write(context);
    }

    private void DoFormatCheck(DeserializationContext context) {
        // Do a length check on the first object to see if it's 2022.5+.
        if (context.VersionInfo.IsAtLeast(GM_2_3) && !context.VersionInfo.IsAtLeast(GM_2022_5)) {
            var returnTo = context.Reader.Position;

            var objectCount = context.Reader.ReadInt32();

            if (objectCount > 0) {
                var firstObjectPointer = context.Reader.ReadInt32();
                context.Reader.Position = firstObjectPointer + 64;

                var vertexCount = context.Reader.ReadInt32();
                var jumpAmount = 12 + (vertexCount * 8);

                if (context.Reader.Position + jumpAmount >= returnTo + Size || jumpAmount < 0) {
                    // Failed bounds check; 2022.5+.
                    context.VersionInfo.UpdateTo(GM_2022_5);
                }
                else {
                    context.Reader.Position += jumpAmount;

                    var eventCount = context.Reader.ReadInt32();

                    if (eventCount != 15) {
                        // Failed event list count check; 2022.5+.
                        context.VersionInfo.UpdateTo(GM_2022_5);
                    }
                    else {
                        var firstEventPointer = context.Reader.ReadInt32();

                        // Failed first event pointer check; 2022.5+.
                        if (context.Reader.Position + 56 != firstEventPointer)
                            context.VersionInfo.UpdateTo(GM_2022_5);
                    }
                }
            }

            context.Reader.Position = returnTo;
        }
    }
}
