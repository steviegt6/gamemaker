using Tomat.GameMaker.IFF.DataTypes;
using Tomat.GameMaker.IFF.DataTypes.Models.Object;

namespace Tomat.GameMaker.IFF.Chunks.OBJT;

internal sealed class GameMakerObjtChunk : AbstractChunk,
                                           IObjtChunk {
    public const string NAME = "OBJT";

    public GameMakerPointerList<GameMakerObject> Objects { get; set; } = null!;

    public GameMakerObjtChunk(string name, int size, int startPosition) : base(name, size, startPosition) { }

    public override void Read(DeserializationContext context) {
        DoFormatCheck(context);

        Objects = context.ReadPointerList<GameMakerObject>();
    }

    public override void Write(SerializationContext context) {
        context.Write(Objects);
    }

    private void DoFormatCheck(DeserializationContext context) {
        // Do a length check on the first object to see if it's 2022.5+.
        if (context.VersionInfo.IsAtLeast(GM_2_3) && !context.VersionInfo.IsAtLeast(GM_2022_5)) {
            var returnTo = context.Position;

            var objectCount = context.ReadInt32();

            if (objectCount > 0) {
                var firstObjectPointer = context.ReadInt32();
                context.Position = firstObjectPointer + 64;

                var vertexCount = context.ReadInt32();
                var jumpAmount = 12 + (vertexCount * 8);

                if (context.Position + jumpAmount >= returnTo + Size || jumpAmount < 0) {
                    // Failed bounds check; 2022.5+.
                    context.VersionInfo.UpdateTo(GM_2022_5);
                }
                else {
                    context.Position += jumpAmount;

                    var eventCount = context.ReadInt32();

                    if (eventCount != 15) {
                        // Failed event list count check; 2022.5+.
                        context.VersionInfo.UpdateTo(GM_2022_5);
                    }
                    else {
                        var firstEventPointer = context.ReadInt32();

                        // Failed first event pointer check; 2022.5+.
                        if (context.Position + 56 != firstEventPointer)
                            context.VersionInfo.UpdateTo(GM_2022_5);
                    }
                }
            }

            context.Position = returnTo;
        }
    }
}
